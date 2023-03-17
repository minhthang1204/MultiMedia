using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class LevelEditorManager : MonoSingleton<LevelEditorManager>
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

    private void Start()
    {
        AudioController.instance.PlayEditorBackground();
        StartCoroutine(CustomUpdate());
    }

    #endregion

    #region Variables

    [Header("Tool Panel")] [SerializeField]
    public EditorMode currentMode = EditorMode.Tile;

    [Header("Components")] [SerializeField]
    private Camera cam;

    [SerializeField] private float customUpdateRate;

    [Header("Save & Load")]

    public string mainLevelFolderName;
    public string customLevelFolderName;

    [Header("Tilemap")] 
    [SerializeField] private List<GameObject> backgrounds = new List<GameObject>();

    private TileBase currentTile
    {
        get { return tiles[EditorPanel.instance.tileOption.GetCurrentValue()].tile; }
    }

    public Tilemap terrainTilemap;
    public List<CustomTile> tiles = new List<CustomTile>();

    [Header("Game Objects")]
    [SerializeField]
    private List<string> prefabTags = new List<string>();

    public List<GameObject> gamePrefabs = new List<GameObject>();
    public Transform gameElementParent;
    public int numOfBall = 0;

    public enum EditorMode
    {
        Tile = 10,
        Ball = 20
    }

    //public Dictionary<int, Tilemap> layers = new Dictionary<int, Tilemap>();

    #endregion

    #region Methods

    IEnumerator CustomUpdate()
    {
        while (true)
        {
            if (currentMode == EditorMode.Tile)
            {
                Vector3Int pos = terrainTilemap.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition));

                if (Input.GetMouseButton(0) && !terrainTilemap.HasTile(pos) && !IsMouseOverGUI())
                {
                    PlaceTile(pos);
                }

                if (Input.GetMouseButton(1) && terrainTilemap.HasTile(pos) && !IsMouseOverGUI())
                {
                    DeleteTile(pos);
                }
            }
            else if (currentMode == EditorMode.Ball)
            {
                Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,20);

                if (Input.GetMouseButtonDown(0) && !IsMouseOverGUI())
                {
                    if (hit.collider == null)
                    {
                        PlacePrefab(mousePos);
                    }
                }
                else if (Input.GetMouseButtonDown(1) && !IsMouseOverGUI())
                {
                    if (hit.collider != null && prefabTags.Contains(hit.collider.gameObject.tag))
                    {
                        DeletePrefab(hit.collider.gameObject);
                    }
                }
            }

            yield return new WaitForSeconds(customUpdateRate);
        }
    }

    public void SelectEditorMode(EditorMode mode)
    {
        currentMode = mode;
    }

    private void PlaceTile(Vector3Int pos)
    {
        terrainTilemap.SetTile(pos, currentTile);
    }

    private void DeleteTile(Vector3Int pos)
    {
        terrainTilemap.SetTile(pos, null);
    }

    private void PlacePrefab(Vector2 pos)
    {
        if (numOfBall >= 2) return;

        int currentPrefabIndex = EditorPanel.instance.ballOption.GetCurrentValue();
        Instantiate(gamePrefabs[currentPrefabIndex], pos, Quaternion.identity,
            gameElementParent.transform);
        if (gamePrefabs[currentPrefabIndex].gameObject.CompareTag("Ball"))
        {
            numOfBall++;
        }
    }

    private void DeletePrefab(GameObject gameObject)
    {
        if (gameObject.CompareTag("Ball"))
        {
            numOfBall--;
        }
        
        Destroy(gameObject);
    }

    public void SaveLevel(bool isMainLevel, int id, int maxDrawPoint, int milestone1, int milestone2, int milestone3,
        int background)
    {
        LevelData levelData = new LevelData();

        //Save id
        levelData.levelID = id;

        //Save isMainLevel
        levelData.isMainLevel = isMainLevel;

        //Save milestone
        levelData.pointMilestones = new []{milestone1,milestone2,milestone3};

        //Save maximum point 
        levelData.maxDrawPointAmount = maxDrawPoint;

        //Save background
        levelData.backgroundIndex = background;

        //Save tilemap

        levelData.layers.Add(new LayerData(terrainTilemap.name));
        
        foreach (var layerData in levelData.layers)
        {
            BoundsInt bounds = terrainTilemap.cellBounds;

            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                for (int y = bounds.min.y; y < bounds.max.y; y++)
                {
                    TileBase temp = terrainTilemap.GetTile(new Vector3Int(x, y, 0));

                    CustomTile tempTile = tiles.Find(t => t.tile == temp);

                    if (tempTile != null)
                    {
                        layerData.tiles.Add(tempTile.id);
                        layerData.tilePositions.Add(new Vector3Int(x, y, 0));
                    }
                }
            }
        }

        //Save gameobject in tilemap

        foreach (Transform child in gameElementParent.transform)
        {
            levelData.gameObjectData.gameObjectsInGame.Add(child.name);
            levelData.gameObjectData.gameObjectPositions.Add(child.transform.position);
        }

        //Save to specific folder depending on if the level is for main game or custom
        string folderName = "";
        if (isMainLevel)
        {
            folderName = mainLevelFolderName;
        }
        else
        {
            folderName = customLevelFolderName;
        }
        

        //Convert to Json and save to folder
        string json = JsonUtility.ToJson(levelData, true);
        File.WriteAllText(Application.streamingAssetsPath + "/Levels/" + folderName + "/Level" + id + ".json", json);
    }
    
    public void LoadBackground()
    {
        foreach (GameObject background in backgrounds)
        {
            background.SetActive(false);
        }

        backgrounds[EditorPanel.instance.backgroundOption.GetCurrentValue()].SetActive(true);
    }

    #endregion
    private bool IsMouseOverGUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    
}

[System.Serializable]
public class LevelData
{
    public bool isMainLevel;
    public int levelID;

    public int maxDrawPointAmount;
    public int[] pointMilestones = new int[3];

    public int backgroundIndex;
    public List<LayerData> layers = new List<LayerData>();
    public GameObjectData gameObjectData = new GameObjectData();
}

[System.Serializable]
public class LayerData
{
    public string layerID;
    public List<string> tiles = new List<string>();
    public List<Vector3Int> tilePositions = new List<Vector3Int>();

    public LayerData(string name)
    {
        layerID = name;
    }
}

[System.Serializable]
public class GameObjectData
{
    public List<string> gameObjectsInGame = new List<string>();
    public List<Vector3> gameObjectPositions = new List<Vector3>();
}