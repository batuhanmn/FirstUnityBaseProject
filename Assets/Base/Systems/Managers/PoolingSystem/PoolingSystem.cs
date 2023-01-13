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

public class PoolingSystem : MonoBehaviour
{
    public static PoolingSystem Instance;
    private bool isLoaded;
    private PoolContainer container;
    private Dictionary<string, Queue<GameObject>> PoolDictionary = new Dictionary<string, Queue<GameObject>>();

    private void Start()
    {
        Init();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
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
        _obj.SetActive(true);
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

        for (int i = 0; i < container.PoolObjectList.Count; i++)
        {
            for (int j = 0; j < container.PoolObjectList[i].Count; j++)
            {
                CreateObject(container.PoolObjectList[i].Name);
            }
        }

    }

    private void CreateObject(string ID)
    {
        GameObject obj = Instantiate(container.PoolObjects[ID].Object, transform);
        obj.SetActive(false);

        if (!PoolDictionary.ContainsKey(ID))
        {
            PoolDictionary[ID] = new Queue<GameObject>();
        }

        PoolDictionary[ID].Enqueue(obj);
    }


    private void Load()
    {
        container = Resources.Load<PoolContainer>("PoolingSystemData/PoolContainer");
        isLoaded = true;
    }
}