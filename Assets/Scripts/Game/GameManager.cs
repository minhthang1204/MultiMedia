using System.Collections.Generic;
using Game;
using Main;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    #region Mono

    protected override void InternalInit()
    {
        if (SceneController.instance.selectedLevelData == null) return;
        
        // setup the selected level
        LevelData selectedLevelData = SceneController.instance.selectedLevelData;
        SetupNewGame(selectedLevelData);
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
        // play sound
        AudioController.instance.PlayGameBackground();
    }

    #endregion

    #region Variables

    [Header("Level Elements")] [SerializeField]
    private List<Ball> balls = new List<Ball>();

    [SerializeField] private Transform gameObjectParent;

    public bool isPause;
    public bool isWin;

    [Header("Level Stats")] public LevelData currentLevelData;
    public bool isMainLevel; // check if the level is the workshop or not

    public int maxDrawPointAmount;
    public int drawPointLeft;

    public int[] pointMilestones = new int[3]; // 3 points milestones
    public int currentMilestone;

    #endregion

    #region Methods

    public void SetupNewGame(LevelData levelData)
    {
        currentLevelData = levelData;

        ImportLevelData();

        drawPointLeft = maxDrawPointAmount;
        LevelManager.instance.LoadLevel(currentLevelData);
        SetupBalls();
        ResetGameStats();
        GameUIManager.instance.SetupUINewLevel(pointMilestones, currentMilestone, maxDrawPointAmount);
    }

    public void ResetGameStats()
    {
        isWin = false;
        currentMilestone = 2;
        drawPointLeft = maxDrawPointAmount;
    }

    public void CheckMilestone()
    {
        for (int i = pointMilestones.Length - 1; i >= 0; i--)
        {
            if (drawPointLeft < pointMilestones[i])
            {
                if (i == currentMilestone)
                {
                    currentMilestone--;
                    GameUIManager.instance.DecreaseMilestone(i);
                }
            }
        }
    }

    public void ImportLevelData()
    {
        //Import max draw point
        maxDrawPointAmount = currentLevelData.maxDrawPointAmount;
        //Import milestone
        pointMilestones = currentLevelData.pointMilestones;
    }

    public void SetupBalls()
    {
        foreach (Transform child in gameObjectParent.transform)
        {
            if (child.gameObject.CompareTag("Ball"))
            {
                balls.Add(child.GetComponent<Ball>());
            }
        }

        foreach (Ball ball in balls)
        {
            if (ball == null) continue;
            ball.IsInteractive(false);
            ball.isExplode = false;
        }
    }

    #endregion

    #region Core Game Methods

    public void BeginGame()
    {
        foreach (Ball ball in balls)
        {
            if (ball == null) continue;
            ball.IsInteractive(true);
        }
    }

    public void PauseGame()
    {
        if (isPause) return;

        isPause = true;
        Time.timeScale = 0; // stop animation related to the Unity's time system
        GameUIManager.instance.OpenPauseMenu();
    }

    public void UnPause(bool isClosePanel)
    {
        if (!isPause) return;

        isPause = false;
        Time.timeScale = 1;
        if (isClosePanel)
        {
            GameUIManager.instance.ClosePauseMenu();
        }
    }

    public void RestartGame()
    {
        balls = new List<Ball>();
        LevelManager.instance.ReloadGameObjects();
        ResetGameStats();
        SetupBalls();
        DrawManager.instance.DeleteAllLine();
        GameUIManager.instance.SetupUINewLevel(pointMilestones, currentMilestone, maxDrawPointAmount);
    }
    
    public void WinGame()
    {
        if (isWin) return;

        isWin = true;
        Utils.SaveLevelStats(currentLevelData.levelID, currentLevelData.isMainLevel, currentMilestone);
        GameUIManager.instance.ShowWinDialog();
    }

    #endregion
}