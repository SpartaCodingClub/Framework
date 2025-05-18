using UnityEngine;

public class InputManager
{
    private int pointerId;
    private float eventTime;

    public bool IsValidEvent(int pointerId)
    {
        float eventInterval = Time.unscaledTime - eventTime;
        if (eventInterval < Define.EVENT_INTERVAL)
        {
            return false;
        }

        if (this.pointerId != 0)
        {
            return false;
        }
        this.pointerId = pointerId;

        return true;
    }

    public void EventClear()
    {
        pointerId = 0;
        eventTime = Time.unscaledTime;
    }
}