using System.Collections;
using System.Collections.Generic;
using Rewired.Platforms.XboxOne;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    //Public
    public Vector3[,] MoveGrid = new Vector3[18,10];
    public GameObject Node;
    public int GridX;
    public int GridY;
    public float GridDist;
    public Vector2 StartPoint;
    public Material[] PaintMaterials;
    public GridMove Player1Script;
    public GridMove Player2Script;
    
    //Private
    private List<GameObject> _nodeList = new List<GameObject>();
    private List<Material> _newColors = new List<Material>();
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnNodes();
        _newColors.AddRange(PaintMaterials); //put all the colors into _newColors
    }

    //Called at the very start of the game to set up the grid
    private void SpawnNodes()
    {
        for (int x = 0; x < GridX; x++)
        {
            for (int y = 0; y < GridY; y++)
            {
                var newNode = Instantiate(Node);
                newNode.transform.position = new Vector3(StartPoint.x + (x * GridDist), StartPoint.y + (y * GridDist));
                newNode.transform.parent = transform;
                _nodeList.Add(newNode);
            }
        }
    }

    //Called when a new round begins
    public void NextRound()
    {
        //Randomly pick which of the 2 colors will remain
        int whichColor = Random.Range(1, 2);
        if (whichColor % 2 == 0)
        {
            //Color 1 remains
            int colorPick = Random.Range(0, _newColors.Count - 1);
            Material newColor = _newColors[colorPick];
            _newColors.Remove(newColor);
            _newColors.Add(Player1Script.Color1Mat);
            Player1Script.MaterialChange(Player1Script.Color1Mat, newColor);
            Player2Script.MaterialChange(Player2Script.Color1Mat, newColor);
        }
        else
        {
            //Color 2 remains
            int colorPick = Random.Range(0, _newColors.Count - 1);
            Material newColor = _newColors[colorPick];
            _newColors.Remove(newColor);
            _newColors.Add(Player1Script.Color2Mat);
            Player1Script.MaterialChange(newColor, Player1Script.Color2Mat);
            Player2Script.MaterialChange(newColor, Player2Script.Color2Mat);
        }
    }

    //Called in NextRound when it is time to check which color remains
    //for the color that no longer remains those nodes get marked empty
    private void UpdateNodes()
    {
        
    }
}
