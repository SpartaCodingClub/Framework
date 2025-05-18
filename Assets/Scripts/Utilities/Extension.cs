using System;
using UnityEngine;

public static class Extension
{
    public static T FindComponent<T>(this GameObject gameObject, string name) where T : Component
    {
        return FindComponent(gameObject, typeof(T), name) as T;
    }

    public static Component FindComponent(this GameObject gameObject, Type type, string name)
    {
        return Utility.FindComponent(gameObject, type, name);
    }

    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        return Utility.GetOrAddComponent<T>(gameObject);
    }

    public static void SetPivot(this RectTransform rectTransform, Vector2 pivot)
    {
        Utility.SetPivot(rectTransform, pivot);
    }
}