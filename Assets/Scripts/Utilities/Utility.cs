using DG.Tweening;
using System;
using UnityEngine;

public class Utility
{
    public static Component FindComponent(GameObject gameObject, Type type, string name)
    {
        var components = gameObject.GetComponentsInChildren(type, true);
        foreach (var component in components)
        {
            if (component.name == name)
            {
                return component;
            }
        }

        Debug.LogWarning($"{Define.FAILED_TO_}{nameof(FindComponent)}({gameObject.name}, {type.Name}, {name})");
        return null;
    }

    public static T GetOrAddComponent<T>(GameObject gameObject) where T : Component
    {
        if (gameObject.TryGetComponent<T>(out var component))
        {
            return component;
        }

        return gameObject.AddComponent<T>();
    }

    public static Sequence RecyclableSequence(bool isIndependentUpdate = true)
    {
        return DOTween.Sequence().Pause().SetAutoKill(false).SetUpdate(isIndependentUpdate);
    }

    public static void SetPivot(RectTransform rectTransform, Vector2 pivot)
    {
        Vector2 deltaPivot = pivot - rectTransform.pivot;
        rectTransform.pivot = pivot;

        Vector2 size = rectTransform.rect.size;
        float x = size.x * deltaPivot.x;
        float y = size.y * deltaPivot.y;
        rectTransform.anchoredPosition += new Vector2(x, y);
    }
}