using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolObject
{
    public string Name;
    public int Count;
    public GameObject Object;
}

public class PoolingSystem : Singleton<PoolingSystem>
{
    private bool isLoaded;
    private PoolContainer container;
    private Dictionary<string, Queue<GameObject>> PoolDictionary = new Dictionary<string, Queue<GameObject>>();

    private void OnEnable()
    {
        Init();
    }

    public GameObject Instantiate(string ID, Vector3 Position, Quaternion Rotation, Transform Parent = null)
    {
        if (!isLoaded) Load();
        if (!PoolDictionary.ContainsKey(ID))
        {
            PoolDictionary[ID] = new Queue<GameObject>();
        }

        if (PoolDictionary[ID].Count == 0)
        {
            CreateObject(ID);
        }

        GameObject _obj = PoolDictionary[ID].Dequeue();

        IPoolable poolable = _obj.GetComponent<IPoolable>();
        if (poolable != null) poolable.OnGetFromPool();

        _obj.transform.position = Position;
        _obj.transform.rotation = Rotation;
        _obj.transform.SetParent(Parent);
        return _obj;
    }

    public void Destroy(string ID, GameObject Object)
    {
        IPoolable poolable = Object.GetComponent<IPoolable>();
        if (poolable != null) poolable.OnReturnToPool();
        Object.SetActive(false);
        Object.transform.SetParent(transform);
        PoolDictionary[ID].Enqueue(Object);
    }

    private void Init()
    {
        if (!isLoaded) Load();
        foreach (var item in container.PoolObjects)
        {
            for (int i = 0; i < item.Value.Count; i++)
            {
                CreateObject(item.Key);
            }
        }
    }

    private void CreateObject(string ID)
    {
        GameObject obj = Instantiate(container.PoolObjects[ID].Object, transform);
        obj.SetActive(false);
        PoolDictionary[ID].Enqueue(obj);
    }

    private void Load()
    {
        isLoaded = true;
        container = Resources.Load<PoolContainer>("PoolingSystemData/PoolContainer");
    }

}
