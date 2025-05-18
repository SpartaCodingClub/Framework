#if UNITY_EDITOR
using System.Reflection;
#endif

using System;
using UnityEngine;

public enum State
{
    Destroyed,
    Birth,
    Stand,
    Death
}

public abstract class BaseController : MonoBehaviour
{
#if UNITY_EDITOR
    #region Editor
    private void OnValidate()
    {
        var fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var field in fields)
        {
            if (field.GetCustomAttribute<SerializeField>() == null)
            {
                continue;
            }

            Type type = field.FieldType;
            if (type.IsSubclassOf(typeof(Component)))
            {
                field.SetValue(this, gameObject.FindComponent(type, field.Name));
                continue;
            }

            Component component = gameObject.FindComponent(typeof(Transform), field.Name);
            if (component == null)
            {
                continue;
            }

            field.SetValue(this, component.gameObject);
        }
    }
    #endregion
    //#region Inspector
    //[ShowInInspector]
    //public State CurrentState
    //{
    //    get
    //    {
    //        if (EditorApplication.isPlaying && !PrefabStageUtility.GetCurrentPrefabStage())
    //        {
    //            EditorUtility.SetDirty(this);
    //        }

    //        return state;
    //    }
    //}
    //#endregion
#endif

    public bool IsDead { get { return state == State.Death || state == State.Destroyed; } }

    public event Action OnBirth;
    public event Action OnStand;
    public event Action OnDeath;
    public event Action OnDestroyed;

    private State state;

    private void Awake() => Initialize();
    private void OnDestroy() => Deinitialize();
    private void OnDisable() => Clear();

    protected virtual void Initialize() { }
    protected virtual void Deinitialize() { }

    public virtual void Clear()
    {
        OnBirth = null;
        OnStand = null;
        OnDeath = null;
        OnDestroyed = null;
    }

    public virtual void Birth()
    {
        state = State.Birth;
        OnBirth?.Invoke();
    }

    public virtual void Stand()
    {
        state = State.Stand;
        OnStand?.Invoke();
    }

    public virtual void Death()
    {
        state = State.Death;
        OnDeath?.Invoke();
    }

    public virtual void Destroy()
    {
        state = State.Destroyed;
        OnDestroyed?.Invoke();

        Managers.Resource.Destroy(gameObject);
    }
}