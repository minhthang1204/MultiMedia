using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MilestoneBar : MonoBehaviour
{
    #region Components

    [SerializeField] private Image milestoneBarFill;

    #endregion

    #region Variables

    [SerializeField] private float parentTransformWidth;
    [SerializeField] private List<RectTransform> milestoneIndicators;

    [SerializeField] private float milestoneBarStep;
    [SerializeField] private Ease disableEase;
    [SerializeField] private float disableTweenTime;

    #endregion

    #region Mono

    #endregion

    #region Methods

    public void SetupMilestoneProgressBar(int maxPoint)
    {
        milestoneBarStep = (float)1 / maxPoint;
        milestoneBarFill.fillAmount = 1;
    }

    // TODO: SetupMilestoneIndicators 
    public void SetupMilestoneIndicators(int[] pointMilestones, int currentMilestone)
    {
        for (int i = 0; i < pointMilestones.Length; i++)
        {
            Vector2 rect = milestoneIndicators[i].anchoredPosition;
            rect.x = parentTransformWidth * pointMilestones[i] / GameManager.instance.maxDrawPointAmount;
            milestoneIndicators[i].anchoredPosition = rect;
            if (i > currentMilestone)
            {
                milestoneIndicators[i].transform.DOScale(Vector3.zero, disableTweenTime).SetEase(disableEase);
            }
            else
            {
                milestoneIndicators[i].transform.DOScale(new Vector3(0.5f,0.5f,1), disableTweenTime).SetEase(disableEase);
            }
        }
    }

    // UseInk: decrease the fill amount of the milestone bar
    public void UseInk(int numOfPoint)
    {
        float finalUsedAmount = milestoneBarStep * numOfPoint;
        if (milestoneBarFill.fillAmount > finalUsedAmount)
        {
            milestoneBarFill.fillAmount += finalUsedAmount;
        }
        else
        {
            milestoneBarFill.fillAmount = 0;
        }
    }

    // Disable the indicator -> scale to 0
    public void DecreaseMilestone(int milestoneNum)
    {
        milestoneIndicators[milestoneNum].transform.DOScale(Vector3.zero, disableTweenTime).SetEase(disableEase);
    }

    #endregion
}