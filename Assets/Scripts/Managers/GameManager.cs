using UnityEngine;

public class GameManager
{
    public void Initialize()
    {
#if UNITY_EDITOR
        Application.runInBackground = true;
#endif

        Application.targetFrameRate = 60;
    }
}