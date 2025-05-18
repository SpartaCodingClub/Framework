using UnityEngine;

public abstract class Scene_Base : MonoBehaviour
{
    private void Awake() => Initialize();

    protected virtual void Initialize()
    {
        if (Managers.Instance == null)
        {
            new GameObject(nameof(Managers), typeof(Managers));
        }
        else
        {
            Camera MainCamera = gameObject.FindComponent<Camera>(nameof(MainCamera));
            Destroy(MainCamera.gameObject);
        }

        Managers.Scene.CurrentScene = this;
    }

    public virtual void Clear()
    {
        string key = GetType().Name;
        Managers.Resource.Release(GetLabel(key));
    }

    protected string GetLabel(string key)
    {
        return key[(key.IndexOf('_') + 1)..];
    }
}