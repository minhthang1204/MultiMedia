using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoSingleton<GameUIManager>
{
    #region UI Elements

    [SerializeField] private MilestoneBar milestoneBar;
    [SerializeField] private Image fadeBackground;

    [SerializeField] private PauseMenu pausePanel;
    [SerializeField] private WinDialog winDialog;
    [SerializeField] private float dialogTweenTime;

    [SerializeField] private Ease openPausePanelTweenEase;
    [SerializeField] private Ease closePausePanelTweenEase;

    #endregion

    #region Mono

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

    private void Start()
    {
    }

    #endregion

    #region Methods

    public void SetupUINewLevel(int[] milestonePoints, int currentMilestone, int maxPoint)
    {
        milestoneBar.SetupMilestoneIndicators(milestonePoints, currentMilestone);
        milestoneBar.SetupMilestoneProgressBar(maxPoint);
    }

    public void DrawPoint(int numOfPoint)
    {
        milestoneBar.UseInk(numOfPoint);
    }

    public void DecreaseMilestone(int milestoneNum)
    {
        milestoneBar.DecreaseMilestone(milestoneNum);
        AudioController.instance.PlayLoseMilestone();
    }

    public void OpenPauseMenu()
    {
        pausePanel.OpenPausePanel();
        ShowDialog(pausePanel.transform,true);
    }

    public void ClosePauseMenu()
    {
        CloseDialog(pausePanel.transform,true);
    }

    public void ShowWinDialog()
    {
        ShowDialog(winDialog.transform, false);
        winDialog.PlayMilestoneAnimation();
    }
    
    public void CloseWinDialog(Action method)
    {
        CloseDialog(winDialog.transform,false,method);
    }

    public void ShowDialog(Transform dialog,bool isUnscale, Action method = null)
    {
        fadeBackground.gameObject.SetActive(true);
        fadeBackground.DOFade(0.7f, dialogTweenTime).SetEase(openPausePanelTweenEase)
            .SetUpdate(UpdateType.Normal, isUnscale);
        dialog.transform.DOScale(new Vector3(1, 1, 1), dialogTweenTime).SetEase(openPausePanelTweenEase)
            .SetUpdate(UpdateType.Normal, isUnscale).OnComplete(() =>
            {
                method?.Invoke();
            });
    }

    public void CloseDialog(Transform dialog,bool isUnscale,Action method = null)
    {
        fadeBackground.DOFade(0f, dialogTweenTime).SetEase(closePausePanelTweenEase).SetUpdate(UpdateType.Normal, isUnscale);
        dialog.transform.DOScale(Vector3.zero, dialogTweenTime).SetEase(closePausePanelTweenEase)
            .SetUpdate(UpdateType.Normal, isUnscale).OnComplete(() =>
            {
                fadeBackground.gameObject.SetActive(false);
                method?.Invoke();
            });
    }

    #endregion
}