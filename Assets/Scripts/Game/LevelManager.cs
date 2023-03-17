using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoSingleton<LevelManager>
{
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
    #endregion

    #region Variable

    [Header("Load")]

    public string mainLevelFolderName;
    public string customLevelFolderName;

    [Header("Tilemap")] private int selectedTileIndex;
    
    [SerializeField] private int currentBackgroundIndex;
    [SerializeField] private List<GameObject> backgrounds = new List<GameObject>();

    public Tilemap terrainTilemap;
    public List<CustomTile> tiles = new List<CustomTile>();

    [Header("Game Objects")] public List<GameObject> gamePrefabs = new List<GameObject>();
    public Transform gameElementParent;

    #endregion

    #region Methods

    public LevelData GetLevelData(int levelID, bool isMainLevel)
    {
        //Get folder whether if the level is for main game or a custom one
        string folderName = "";
        if (isMainLevel)
        {
            folderName = mainLevelFolderName;
        }
        else
        {
            folderName = customLevelFolderName;
        }
        
        //Get the level file
        try
        {
            string json = File.ReadAllText(Application.streamingAssetsPath + "/Levels/" + folderName + "/Level" + levelID + ".json");
            //Convert json to class instance
            LevelData levelData = JsonUtility.FromJson<LevelData>(json);

            return levelData;
        }
        catch(FileNotFoundException)
        {
            return null;
        }
    }
    public void LoadLevel(LevelData levelData)
    {
        //Load background
        GameManager.instance.isMainLevel = levelData.isMainLevel;
        currentBackgroundIndex = levelData.backgroundIndex;
        LoadBackground(currentBackgroundIndex);
        
        //Load tilemap
        foreach (var data in levelData.layers)
        {
            terrainTilemap.ClearAllTiles();

            for (int i = 0; i < data.tilePositions.Count; i++)
            {
                TileBase tile = tiles.Find(t => t.id == data.tiles[i]).tile;
                if (tile) terrainTilemap.SetTile(data.tilePositions[i], tile);
            }
        }

        //Load game object element
        //Delete all current child
        foreach (Transform child in gameElementParent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < levelData.gameObjectData.gameObjectsInGame.Count; i++)
        {
            //Check if game object is in prefab list, if true instantiate gameobject
            foreach (GameObject gamePrefab in gamePrefabs)
            {
                string prefabDataName = levelData.gameObjectData.gameObjectsInGame[i];
                if (prefabDataName.Substring(0,prefabDataName.Length-7) == gamePrefab.name)
                {
                    //Vector3Int pos = defaultTilemap.WorldToCell(levelData.gameObjectData.gameObjectPositions[i]);
                    Vector3 pos = levelData.gameObjectData.gameObjectPositions[i];
                    Instantiate(gamePrefab, pos, Quaternion.identity,
                        gameElementParent.transform);
                    break;
                }
            }
        }
    }
    
    public void LoadBackground(int index)
    {
        foreach (GameObject background in backgrounds)
        {
            background.SetActive(false);
        }
        
        backgrounds[index].SetActive(true);
    }

    public void ReloadGameObjects()
    {
        foreach (Transform child in gameElementParent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < GameManager.instance.currentLevelData.gameObjectData.gameObjectsInGame.Count; i++)
        {
            //Check if game object is in prefab list, if true instantiate gameobject
            foreach (GameObject gamePrefab in gamePrefabs)
            {
                string prefabDataName = GameManager.instance.currentLevelData.gameObjectData.gameObjectsInGame[i];
                if (prefabDataName.Substring(0,prefabDataName.Length-7) == gamePrefab.name)
                {
                    //Vector3Int pos = defaultTilemap.WorldToCell(GameManager.instance.currentLevelData.gameObjectData.gameObjectPositions[i]);
                    Vector3 pos = GameManager.instance.currentLevelData.gameObjectData.gameObjectPositions[i];
                    Instantiate(gamePrefab, pos, Quaternion.identity,
                        gameElementParent.transform);
                    break;
                }
            }
        }
    }

  
    #endregion

}