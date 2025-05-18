using System;
using UnityEngine;

public class CameraManager
{
    private readonly float TARGET_RATIO = Define.TARGET_RESOLUTION.x / Define.TARGET_RESOLUTION.y;
    private readonly float TARGET_SIZE = Camera.main.orthographicSize;

    public event Action OnCameraUpdated;
    public event Action<Rect> OnSafeAreaUpdated;

    private float aspectRatio;
    private Rect safeArea;

    private readonly Camera main = Camera.main;

    public void Initialize()
    {
        main.name = nameof(CameraManager);
        main.transform.SetParent(Managers.Instance.transform);
    }

    public void Update()
    {
        Update_Camera();
        Update_SafeArea();
    }

    private void Update_Camera()
    {
        float aspectRatio = (float)Screen.width / Screen.height;
        if (aspectRatio == this.aspectRatio)
        {
            return;
        }
        this.aspectRatio = aspectRatio;

        float targetRatio = TARGET_RATIO / aspectRatio;
        main.orthographicSize = TARGET_SIZE * targetRatio;

        OnCameraUpdated?.Invoke();
    }

    private void Update_SafeArea()
    {
        Rect safeArea = Screen.safeArea;
        if (safeArea == this.safeArea)
        {
            return;
        }
        this.safeArea = safeArea;

        OnSafeAreaUpdated?.Invoke(safeArea);
    }
}