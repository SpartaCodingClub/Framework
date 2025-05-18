using UnityEngine;
using UnityEngine.UI;

public class RawImageHandler : MonoBehaviour
{
#if UNITY_EDITOR
    #region Editor
    private void OnValidate()
    {
        Awake();
        UpdateUVRect();
    }
    #endregion
#endif

    private RectTransform rectTransform;
    private RawImage rawImage;

    private float width;
    private float height;

    private float y;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rawImage = GetComponent<RawImage>();

        Texture texture = rawImage.texture;
        width = texture.width;
        height = texture.height;
    }

    private void OnEnable()
    {
        UpdateUVRect();
        Managers.Camera.OnCameraUpdated += UpdateUVRect;
    }

    private void OnDisable()
    {
        Managers.Camera.OnCameraUpdated -= UpdateUVRect;
    }

    private void Update()
    {
        y -= 0.5f * Time.unscaledDeltaTime;

        Rect uvRect = rawImage.uvRect;
        uvRect.y = Mathf.Repeat(y, 1.0f);
        rawImage.uvRect = uvRect;
    }

    private void UpdateUVRect()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

        Rect rect = rectTransform.rect;
        float u = rect.width / width;
        float v = rect.height / height;
        rawImage.uvRect = new(0.0f, 0.0f, u, v);
    }
}