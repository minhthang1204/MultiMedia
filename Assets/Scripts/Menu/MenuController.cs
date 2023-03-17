using System.Collections.Generic;
using System.IO;
using Main;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    #region Variables
    [Header("Canvas")]
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private List<GameObject> subCanvas;
    [SerializeField] private GameObject currentCanvas;
    
    [Header("Level Select")]
    [SerializeField] string mainLevelFolderName;
    [SerializeField] string customLevelFolderName;
    
    [SerializeField] private List<MenuLevel> mainGameLevels;
    [SerializeField] private List<MenuLevel> customGameLevels;

    [Header("Options")] [SerializeField] private Toggle audioToggle;
    #endregion


    private void Awake()
    {   
        // show only the main menu canvas

        foreach (var canvas in subCanvas) 
        {
            canvas.gameObject.SetActive(false);
        }
        
        mainMenuCanvas.SetActive(true);
        currentCanvas = mainMenuCanvas;
        
        LevelSelectCanvasLoad();
        CustomLevelSelectCanvasLoad();

        if (SceneController.instance.isFromCustomMap)
        {
            SceneController.instance.isFromCustomMap = false;
            OpenCanvas("WorkshopCanvas");
        }
        
        
    }

    private void Start()
    {
        AudioController.instance.PlayMenuBackground();
        CheckAudio();
    }

    public void OpenCanvas(string newCanvasName)
    {
        currentCanvas.SetActive(false);
        foreach (var canvas in subCanvas)
        {
            if (canvas.gameObject.name == newCanvasName)
            {
                canvas.SetActive(true);
                currentCanvas = canvas;
                return;
            }
        }
    }

    public void BackToMenuCanvas()
    {
        currentCanvas.gameObject.SetActive(false);
        mainMenuCanvas.gameObject.SetActive(true);
        currentCanvas = mainMenuCanvas;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    #region Canvas Load Method

    private void LevelSelectCanvasLoad()
    {
        foreach (var mainGameLevel in mainGameLevels)
        {
            mainGameLevel.gameObject.SetActive(false);
        }
        
        // build the level's path
        var info = new DirectoryInfo(Application.streamingAssetsPath + "/Levels/" + mainLevelFolderName);
        var fileInfo = info.GetFiles("*.json");
        
        for(int i = 0; i<fileInfo.Length;i++)
        {
            {
                string json = File.ReadAllText(fileInfo[i].FullName);
                
                LevelData levelData = JsonUtility.FromJson<LevelData>(json);
                
                mainGameLevels[levelData.levelID-1].gameObject.SetActive(true);
                mainGameLevels[levelData.levelID-1].SetUpLevelMenu(levelData);
            }
        }
        
    }
    
    private void CustomLevelSelectCanvasLoad()
    {
        foreach (var customGameLevel in customGameLevels)
        {
            customGameLevel.gameObject.SetActive(false);
        }
        
        var info = new DirectoryInfo(Application.streamingAssetsPath + "/Levels/" + customLevelFolderName);
        var fileInfo = info.GetFiles("*.json");
        
        for(int i = 0; i<fileInfo.Length;i++)
        {
            {
                string json = File.ReadAllText(fileInfo[i].FullName);
                
                LevelData levelData = JsonUtility.FromJson<LevelData>(json);
                
                customGameLevels[levelData.levelID-1].gameObject.SetActive(true);
                customGameLevels[levelData.levelID-1].SetUpLevelMenu(levelData);
            }
        }
        
    }
    #endregion
    public void PlayLevelEditor()
    {
        // load the LevelEditor Scene
        SceneController.instance.Load("LevelEditor");
    }

    public void MuteAudio(bool isMute)
    {
        if (!isMute)
        {
            AudioController.instance.Mute();
        }
        else
        {
            AudioController.instance.Unmute();
        }
        
    }

    private void CheckAudio()
    {   
        // update the audio's image
        if (AudioController.instance.CheckAudioStatus())
        {
            audioToggle.isOn = false;
        }
        else
        {
            audioToggle.isOn = true;
        }
    }

}
