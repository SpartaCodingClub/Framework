using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    #region Birth
    private Sequence Birth(RectTransform rectTransform)
    {
        rectTransform.SetPivot(Define.PIVOT_CENTER);

        return Utility.RecyclableSequence()
            .Append(rectTransform.DOScale(rectTransform.localScale * 0.9f, 0.2f));
    }
    #endregion
    #region Death
    private Sequence Death(RectTransform rectTransform)
    {
        return Utility.RecyclableSequence()
            .Append(rectTransform.DOScale(rectTransform.localScale, 0.2f).SetEase(Ease.OutBack));
    }
    #endregion

    private event Action OnPointerClicked;

    private CanvasGroup canvasGroup;
    private SequenceHandler sequenceHandler;

    private int pointerId;

    private void OnDestroy() => sequenceHandler?.Deinitialize();

    public void OnPointerDown(PointerEventData eventData) { if (canvasGroup && canvasGroup.interactable) PointerDown(eventData); }
    public void OnPointerUp(PointerEventData eventData) { if (canvasGroup && canvasGroup.interactable) PointerUp(eventData); }
    public void OnPointerClick(PointerEventData eventData) { if (canvasGroup && canvasGroup.interactable) PointerClick(eventData); }

    public void BindEvents(bool bindSequences = false, params Action[] events)
    {
        if (canvasGroup == null)
        {
            Initialize(bindSequences);
        }

        foreach (var @event in events)
        {
            OnPointerClicked += @event;
        }

        gameObject.GetOrAddComponent<GraphicRaycaster>();
    }

    private void Initialize(bool bindSequences = false)
    {
        if (bindSequences)
        {
            sequenceHandler = new();
            sequenceHandler.Initialize();

            RectTransform rectTransform = GetComponent<RectTransform>();
            sequenceHandler.BindSequences(State.Birth, () => Birth(rectTransform));
            sequenceHandler.BindSequences(State.Death, () => Death(rectTransform));
        }

        canvasGroup = transform.root.GetComponent<CanvasGroup>();
    }

    private void PointerDown(PointerEventData eventData)
    {
        if (Managers.Input.IsValidEvent(eventData.pointerId) == false)
        {
            return;
        }

        if (sequenceHandler != null)
        {
            sequenceHandler.Death.Pause();
            sequenceHandler.Birth.Restart();
        }

        pointerId = eventData.pointerId;
    }

    private void PointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId != pointerId)
        {
            return;
        }

        if (sequenceHandler != null)
        {
            sequenceHandler.Birth.Pause();
            sequenceHandler.Death.Restart();
        }

        Managers.Input.EventClear();
    }

    private void PointerClick(PointerEventData eventData)
    {
        if (eventData.pointerId != pointerId)
        {
            return;
        }

        OnPointerClicked?.Invoke();
    }
}