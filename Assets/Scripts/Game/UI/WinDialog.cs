using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Main;
using Shapes2D;
using UnityEngine;

public class WinDialog : MonoBehaviour
{
    [SerializeField] private List<Shape> milestones = new List<Shape>();
    [SerializeField] private List<GameObject> milestoneStarExplosion = new List<GameObject>();
    [SerializeField] private Vector3 milestoneExpandScale;
    [SerializeField] private float milestoneExpandTweenTime;
    [SerializeField] private Ease milestoneExpandEaseIn;
    [SerializeField] private Ease milestoneExpandEaseOut;
    [SerializeField] private float delayTime;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject menuButton;
    public void PlayMilestoneAnimation()
    {
        if (GameManager.instance.currentLevelData.isMainLevel)
        {
            nextButton.SetActive(true);
            menuButton.SetActive(false);
        }
        else
        {
            nextButton.SetActive(false);
            menuButton.SetActive(true);
        }
        ResetMilestoneAnimation();
        StartCoroutine(ShowMilestone(GameManager.instance.currentMilestone));
    }

    IEnumerator ShowMilestone(int milestone)
    {
    yield return new WaitForSeconds(delayTime*1.5f);
        for (int i = 0; i <= milestone; i++)
        {
            AudioController.instance.PlayWinMilestone();
            milestoneStarExplosion[i].SetActive(true);
            milestones[i].transform.DOScale(milestoneExpandScale, milestoneExpandTweenTime)
                .SetEase(milestoneExpandEaseIn).OnComplete(
                    () =>
                    {
                        milestones[i].settings.fillColor = new Color32(213, 177, 149, 255);
                    });

            milestones[i].transform.DOScale(new Vector3(1, 1, 1), milestoneExpandTweenTime/2)
                .SetEase(milestoneExpandEaseOut).SetDelay(milestoneExpandTweenTime);
            yield return new WaitForSeconds(delayTime);
            
            //play sound
        }
    }

    public void ResetMilestoneAnimation()
    {
        foreach (var milestone in milestones)
        {
            milestone.settings.fillColor = new Color32(255, 237, 220, 255);
        }
        foreach (var star in milestoneStarExplosion)
        {
            star.SetActive(false);
        }
    }

    public void NextLevel()
    {
        GameUIManager.instance.CloseWinDialog( () =>
        {
            LevelData newLevel = LevelManager.instance.GetLevelData(GameManager.instance.currentLevelData.levelID + 1,
                GameManager.instance.currentLevelData.isMainLevel);
            if (newLevel == null)
            {
                ExitToMenu();
                return;
            }
            GameManager.instance.SetupNewGame(newLevel);
            GameManager.instance.RestartGame();
        });
    }

    public void Retry()
    {
        GameUIManager.instance.CloseWinDialog( () =>
        {
            GameManager.instance.SetupNewGame(LevelManager.instance.GetLevelData(GameManager.instance.currentLevelData.levelID, GameManager.instance.currentLevelData.isMainLevel));
            GameManager.instance.RestartGame();
        });
    }

    public void ExitToMenu()
    {
        SceneController.instance.isFromCustomMap = !GameManager.instance.isMainLevel;
        SceneController.instance.Load("Menu");
    }
}