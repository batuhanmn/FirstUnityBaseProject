using System;
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
    private PoolContainer _container;
    private Dictionary<string, Queue<GameObject>> _poolDictionary = new Dictionary<string, Queue<GameObject>>();
    private GameObject _poolContainer;
    private void Start()
    {
       
        Init();
    }
    
    public GameObject Instantiate(string id, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (!isLoaded) Load();
        if (!_poolDictionary.ContainsKey(id))
        {
            _poolDictionary[id] = new Queue<GameObject>();
        }

        if (_poolDictionary[id].Count == 0)
        {
            CreateObject(id);
        }

        GameObject obj = _poolDictionary[id].Dequeue();

        IPoolable poolable = obj.GetComponent<IPoolable>();
        if (poolable != null) poolable.OnGetFromPool();

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.transform.SetParent(parent);
        obj.SetActive(true);
        return obj;
    }

    public void Destroy(string id, GameObject @object)
    {
        IPoolable poolable = @object.GetComponent<IPoolable>();
        if (poolable != null) poolable.OnReturnToPool();
        @object.SetActive(false);
        @object.transform.SetParent(transform);
        _poolDictionary[id].Enqueue(@object);
    }

    private void Init()
    {
        if (!isLoaded) Load();

        for (int i = 0; i < _container.PoolObjectList.Count; i++)
        {
            for (int j = 0; j < _container.PoolObjectList[i].Count; j++)
            {
                CreateObject(_container.PoolObjectList[i].Name);
            }
        }

    }

    private void CreateObject(string id)
    {
        GameObject obj = Instantiate(_container.PoolObjects[id].Object, _poolContainer.transform);
        obj.SetActive(false);

        if (!_poolDictionary.ContainsKey(id))
        {
            _poolDictionary[id] = new Queue<GameObject>();
        }

        _poolDictionary[id].Enqueue(obj);
    }


    private void Load()
    {
        _container = Resources.Load<PoolContainer>("PoolingSystemData/PoolContainer");
        if (_poolContainer==null)
        {
            _poolContainer = new GameObject("PoolContainer");
        }
        isLoaded = true;
    }
}