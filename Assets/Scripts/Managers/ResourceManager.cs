using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

public class ResourceManager
{
    private readonly Dictionary<string, HashSet<string>> keys = new();
    private readonly Dictionary<string, AsyncOperationHandle> operations = new();

    public AsyncOperationHandle LoadAssetAsync<T>(string key, Action<T> onComplete = null) where T : Object
    {
        if (operations.TryGetValue(key, out var operation))
        {
            onComplete?.Invoke(operation.Result as T);
            return operation;
        }

        operation = Addressables.LoadAssetAsync<T>(key);
        operation.Completed += operation => onComplete?.Invoke(operation.Result as T);
        operations.Add(key, operation);

        return operation;
    }

    public AsyncOperationHandle CreateGenericGroupOperation(AsyncOperationHandle<IList<IResourceLocation>> groupLocation)
    {
        var locations = groupLocation.Result;
        var keys = new HashSet<string>(locations.Count);
        var operations = new List<AsyncOperationHandle>(locations.Count);
        foreach (var location in locations)
        {
            string key = location.PrimaryKey;
            if (keys.Add(key) == false)
            {
                continue;
            }

            var operation = LoadAssetAsync<Object>(key);
            operations.Add(operation);
        }

        string label = groupLocation.DebugName;
        this.keys.Add(label, keys);

        var groupOperation = Addressables.ResourceManager.CreateGenericGroupOperation(operations);
        this.operations.Add(label, groupOperation);

        return groupOperation;
    }

    public void Release(string label)
    {
        if (operations.Remove(label, out var groupOperation) == false)
        {
            Debug.LogWarning($"{Define.FAILED_TO_}{nameof(Release)}({label})");
            return;
        }

        if (this.keys.Remove(label, out var keys))
        {
            foreach (var key in keys)
            {
                operations.Remove(key);
            }
        }

        Addressables.Release(groupOperation);
    }

    public void InstantiateAsync<T>(Transform parent, Vector2 localPosition = new(), Action<T> onComplete = null) where T : BaseController
    {
        string key = typeof(T).Name;
        InstantiateAsync(key, parent, localPosition, gameObject =>
        {
            var @base = gameObject.GetComponent<T>();
            @base.Birth();

            onComplete?.Invoke(@base);
        });
    }

    public void InstantiateAsync(string key, Transform parent, Vector2 localPosition = new(), Action<GameObject> onComplete = null)
    {
        GameObject gameObject = Managers.Pool.TryGet(key);
        if (gameObject != null)
        {
            InstantiateCallback(gameObject, parent, localPosition, onComplete);
            return;
        }

        LoadAssetAsync<GameObject>(key, original =>
        {
            GameObject gameObject = Instantiate(original);
            InstantiateCallback(gameObject, parent, localPosition, onComplete);
        });
    }

    public GameObject Instantiate(GameObject original)
    {
        GameObject gameObject = Object.Instantiate(original);
        gameObject.name = original.name;

        return gameObject;
    }

    private void InstantiateCallback(GameObject gameObject, Transform parent, Vector2 localPosition = new(), Action<GameObject> onComplete = null)
    {
        Transform transform = gameObject.transform;
        transform.SetParent(parent);
        transform.localPosition = localPosition;

        onComplete?.Invoke(gameObject);
    }

    public void Destroy(GameObject gameObject)
    {
        if (gameObject.TryGetComponent<Poolable>(out var poolable))
        {
            Managers.Pool.Release(poolable);
            return;
        }

        Object.Destroy(gameObject);
    }
}