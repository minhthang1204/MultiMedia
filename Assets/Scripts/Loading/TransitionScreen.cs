using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Shapes2D;
using UnityEngine;

public class TransitionScreen : MonoSingleton<TransitionScreen>
{
    #region Variables

    [SerializeField] private Shape circle;
    [SerializeField] private float transitionDuration;
    [SerializeField] private Vector3 newLocalScale;
    [SerializeField] private Ease easeIn;
    [SerializeField] private Ease easeOut;
    #endregion

    #region Method

    protected override void InternalInit()
    {
        
    }

    protected override void InternalOnDestroy()
    {
        
    }

    protected override void InternalOnDisable()
    {
        
    }

    protected override void InternalOnEnable()
    {
        
    }

    [ContextMenu("Intro")]
    public void Intro(Action action = null)
    {
        float value = 1;
        DOTween.To(() => value, x => value = x, 0, transitionDuration).SetUpdate(UpdateType.Normal,true).SetEase(easeIn).OnUpdate(() =>
        {
            circle.settings.innerCutout = new Vector2(value, value);
        }).OnComplete(() =>
        {
            action?.Invoke();
        });
        
    }

    public void Outro(Action action = null)
    {
        float value = circle.settings.innerCutout.x;
        DOTween.To(() => value, x => value = x, 1, transitionDuration).SetUpdate(UpdateType.Normal,true).SetEase(easeIn).OnUpdate(() =>
        {
            circle.settings.innerCutout = new Vector2(value, value);
        }).OnComplete(() =>
        {
            action?.Invoke();
        });
    }

    public float GetTweenTime()
    {
        return transitionDuration;
    }
    #endregion
}
