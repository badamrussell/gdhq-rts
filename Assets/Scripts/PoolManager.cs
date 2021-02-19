using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager _instance;
    public static PoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("PoolManager is NULL");
            }

            return _instance;
        }
    }

    private Dictionary<string, Queue<GameObject>> _pool = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        _instance = this;
    }

    public GameObject Get(string keyName)
    {
        if (_pool.ContainsKey(keyName))
        {
            if (_pool[keyName].Count > 0)
            {
                GameObject go = _pool[keyName].Dequeue();

                return go;
            }
        }

        return null;
    }

    public void Add(string keyName, GameObject go)
    {
        if (!_pool.ContainsKey(keyName))
        {
            Queue<GameObject> goQue = new Queue<GameObject>();
            _pool.Add(keyName, goQue);
        }

        _pool[keyName].Enqueue(go);
    }

}
