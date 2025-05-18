#if UNITY_EDITOR
using UnityEngine.UI;
#endif

using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UI_Base : BaseController
{
#if UNITY_EDITOR
    #region Editor
    protected virtual void Reset()
    {
        GetComponent<Canvas>().vertexColorAlwaysGammaSpace = true;

        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = Define.TARGET_RESOLUTION;
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

        DestroyImmediate(GetComponent<GraphicRaycaster>());

        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        gameObject.GetOrAddComponent<Poolable>();
    }
    #endregion
#endif

    protected CanvasGroup canvasGroup;

    private readonly SequenceHandler sequenceHandler = new();

    protected void BindEvents(GameObject gameObject, bool bindSequences = false, params Action[] events) => gameObject.GetOrAddComponent<EventHandler>().BindEvents(bindSequences, events);
    protected void BindSequences(State type, params Func<Sequence>[] sequences) => sequenceHandler.BindSequences(type, sequences);

    protected override void Initialize()
    {
        base.Initialize();
        sequenceHandler.Initialize();

        canvasGroup = GetComponent<CanvasGroup>();
    }

    protected override void Deinitialize()
    {
        base.Deinitialize();
        sequenceHandler.Deinitialize();
    }

    public override void Birth()
    {
        base.Birth();
        canvasGroup.interactable = false;

        sequenceHandler.Pause();
        sequenceHandler.Birth.Restart();
    }

    public override void Stand()
    {
        base.Stand();
        canvasGroup.interactable = true;

        sequenceHandler.Pause();
        sequenceHandler.Stand.Restart();
    }

    public override void Death()
    {
        base.Death();
        canvasGroup.interactable = false;

        sequenceHandler.Pause();
        sequenceHandler.Death.Restart();
    }

    public override void Destroy()
    {
        base.Destroy();
        canvasGroup.interactable = false;

        sequenceHandler.Pause();
    }
}