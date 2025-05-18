using UnityEngine;

public class SafeAreaHandler : MonoBehaviour
{
#if UNITY_EDITOR
    #region Editor
    private void OnValidate()
    {
        Awake();
        UpdateSafeArea(Screen.safeArea);
    }
    #endregion
#endif

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        UpdateSafeArea(Screen.safeArea);
        Managers.Camera.OnSafeAreaUpdated += UpdateSafeArea;
    }

    private void OnDisable()
    {
        Managers.Camera.OnSafeAreaUpdated -= UpdateSafeArea;
    }

    private void UpdateSafeArea(Rect safeArea)
    {
        float width = Screen.width;
        float height = Screen.height;

        Vector2 anchorMin = safeArea.position;
        anchorMin.x /= width;
        anchorMin.y /= height;
        rectTransform.anchorMin = anchorMin;

        Vector2 anchorMax = safeArea.position + safeArea.size;
        anchorMax.x /= width;
        anchorMax.y /= height;
        rectTransform.anchorMax = anchorMax;
    }
}