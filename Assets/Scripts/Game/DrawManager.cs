using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace Game
{
    public class DrawManager : MonoSingleton<DrawManager>
    {
        #region Variables

        [SerializeField] private int amountOfLine;
        
        public GameObject linePrefab;
        public LayerMask cantDrawOverLayer;
        public int finishedLineIndex;

        public Gradient lineColor;
        public float linePointsMinDistance;
        public float lineWidth;

        private Line currentLine;
        [SerializeField] private List<Line> lines = new List<Line>();
        private Camera cam;
        [SerializeField] private Tilemap terrain;
        #endregion

        #region Singleton Methods

        protected override void InternalInit()
        {
            if (cam == null) cam = Camera.main;
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

        #region MonoBehavior Methods

        private void Update()
        {
            if(GameManager.instance.isWin) return;
            
            if (Input.GetMouseButtonDown(0)&&!IsMouseOverGUI())
                BeginDraw();

            if (currentLine != null)
                Draw();

            if (Input.GetMouseButtonUp(0))
                EndDraw();
        }

        #endregion

        #region Methods

        private bool IsMouseOverGUI()
        {
            return  EventSystem.current.IsPointerOverGameObject();
            
        }
        // Begin Draw ----------------------------------------------
        private void BeginDraw()
        {
            Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

            if (terrain.HasTile(terrain.WorldToCell(mousePosition))) return;
            Debug.Log("Start Draw");
            currentLine = Instantiate(linePrefab, this.transform).GetComponent<Line>();

            //Set line properties
            currentLine.UsePhysics(false);
            currentLine.SetLineColor(lineColor);
            currentLine.SetPointsMinDistance(linePointsMinDistance);
            currentLine.SetLineWidth(lineWidth);
        }

        // Draw ----------------------------------------------------
        private void Draw()
        {
            Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

            //Check if mousePos hits any collider with layer "CantDrawOver", if true cut the line by calling EndDraw( )
            RaycastHit2D hit = Physics2D.CircleCast(mousePosition, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer);

            if (hit || GameManager.instance.drawPointLeft<=0)
            {
                EndDraw();
            }
            else
            {
                currentLine.AddPoint(mousePosition);
            }

        }

        // End Draw ------------------------------------------------
        private void EndDraw()
        {
            if (currentLine != null)
            {
                if (currentLine.pointsCount < 2)
                {
                    //If line has one point
                    Destroy(currentLine.gameObject);
                }
                else
                {
                    //Add the line to "CantDrawOver" layer
                    currentLine.gameObject.layer = finishedLineIndex;

                    //Activate Physics on the line
                    currentLine.UsePhysics(true);
                    lines.Add(currentLine);
                    
                    currentLine = null;
                }

                amountOfLine++;
                
                if (amountOfLine == 1)
                {
                    Debug.Log("Begin");
                    GameManager.instance.BeginGame();
                }
            }
        }

        public void AdjustDrawPointLeft(int adjust)
        {
            if (GameManager.instance.drawPointLeft<=0) return;
            
            GameManager.instance.drawPointLeft += adjust;
            
            GameUIManager.instance.DrawPoint(adjust);
            GameManager.instance.CheckMilestone();
        }

        public void DeleteAllLine()
        {
            currentLine = null;
            amountOfLine = 0;
            
            foreach (var line in lines)
            {
                line.Delete();
            }
            
            lines.Clear();
        }
        #endregion
    }
}