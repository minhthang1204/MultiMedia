using System.Collections.Generic;
using Main;
using Shapes2D;
using TMPro;
using UnityEngine;

public class MenuLevel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private List<Shape> points;
    [SerializeField] private LevelData levelData;

    public void SetUpLevelMenu(LevelData data)
    {
        this.levelData = data;

        name.text = levelData.levelID.ToString();

        // Show the points of saved data
        foreach (var point in points)
        {
            if (points.IndexOf(point) <= Utils.GetLevelStats(levelData.levelID, levelData.isMainLevel))
            {
                point.settings.outlineSize = 20;
            }
            else
            {
                point.settings.outlineSize = 7;
            }
        }
    }

    public LevelData GetLevelData()
    {
        return levelData;
    }

    // PlayLevel load the level data and start the game
    public void PlayLevel()
    {
        SceneController.instance.selectedLevelData = levelData;
        SceneController.instance.Load("Game");
    }
}