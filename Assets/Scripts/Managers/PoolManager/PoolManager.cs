using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class PoolManager
{
    private readonly Transform transform = new GameObject(nameof(PoolManager)).transform;
    private readonly Dictionary<string, Pool> pools = new();

    public void Initialize()
    {
        transform.SetParent(Managers.Instance.transform);
    }

    public void Clear()
    {
        foreach (Transform child in transform)
        {
            Object.Destroy(child.gameObject);
        }

        pools.Clear();
    }

    public GameObject TryGet(string key)
    {
        if (pools.TryGetValue(key, out var pool))
        {
            return pool.Get();
        }

        return null;
    }

    public void Release(Poolable poolable)
    {
        string key = poolable.name;
        if (pools.TryGetValue(key, out var pool) == false)
        {
            pool = new(key, transform);
            pools.Add(key, pool);
        }

        pool.Release(poolable);
    }
}