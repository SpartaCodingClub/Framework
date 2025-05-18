using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class UIManager
{
    public enum Type
    {
        Popup,
        Scene,
        SubItem,
        WorldSpace,
        Count
    }

    public UI_Scene CurrentSceneUI { get; private set; }

    private readonly Transform transform = new GameObject(nameof(UIManager), typeof(InputSystemUIInputModule)).transform;
    private readonly Transform[] children = new Transform[(int)Type.Count];
    private readonly Queue<UI_Popup> popups = new();

    public void Initialize()
    {
        transform.SetParent(Managers.Instance.transform);

        var names = Enum.GetNames(typeof(Type));
        for (int i = 0; i < children.Length; i++)
        {
            Transform child = new GameObject(names[i]).transform;
            child.SetParent(transform);
            children[i] = child;
        }
    }

    public void Clear()
    {
        // TODO: Clear Popups
        CurrentSceneUI.Destroy();
    }

    public void Show<T>(Action<T> onComplete = null) where T : UI_Base
    {
        Type type = GetType(typeof(T));
        if (type == Type.Count)
        {
            Debug.LogWarning($"{Define.FAILED_TO_}{nameof(Show)}<{typeof(T).Name}>()");
            return;
        }

        Managers.Resource.InstantiateAsync<T>(children[(int)type], onComplete: @base =>
        {
            switch (type)
            {
                case Type.Popup:
                    Show_Popup(@base as UI_Popup);
                    break;
                case Type.Scene:
                    CurrentSceneUI = @base as UI_Scene;
                    break;
            }

            onComplete?.Invoke(@base);
        });
    }

    private void Show_Popup(UI_Popup popup)
    {
        // TODO: Show Popups
    }

    private Type GetType(System.Type type)
    {
        return type switch
        {
            _ when type.IsSubclassOf(typeof(UI_Popup)) => Type.Popup,
            _ when type.IsSubclassOf(typeof(UI_Scene)) => Type.Scene,
            _ when type.IsSubclassOf(typeof(UI_SubItem)) => Type.SubItem,
            _ when type.IsSubclassOf(typeof(UI_WorldSpace)) => Type.WorldSpace,
            _ => Type.Count
        };
    }
}