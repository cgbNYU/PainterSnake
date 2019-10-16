using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public GameObject Brush1;
    public GameObject Brush2;
    public Material Color1Mat;
    public Material Color2Mat;
    
    //Private
    private List<GameObject> _nodeList = new List<GameObject>();
    private List<Material> _newColors = new List<Material>();
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnNodes();
        FirstRound();
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

    //Called for the first round
    private void FirstRound()
    {
        _newColors.AddRange(PaintMaterials); //put all the colors into _newColors
        
        int color1Pick = Random.Range(0, _newColors.Count - 1);
        Material newColor1 = _newColors[color1Pick];
        _newColors.Remove(newColor1);
        Color1Mat = newColor1;

        int color2Pick = Random.Range(0, _newColors.Count - 1);
        Material newColor2 = _newColors[color2Pick];
        _newColors.Remove(newColor2);
        Color2Mat = newColor2;
        Instantiate(Brush1);
        Instantiate(Brush2);
    }
    
    //Called when a new round begins
    public void NextRound()
    {
        //Randomly pick which of the 2 colors will remain
        int whichColor = Random.Range(1, 2);
        if (whichColor % 2 == 0)
        {
            //Color 1 remains
            int colorPick = Random.Range(0, _newColors.Count - 1); //pick a random value
            Material newColor = _newColors[colorPick]; //use that value to grab a color from the list
            _newColors.Remove(newColor); //remove that color from the list
            _newColors.Add(Color2Mat); //add the color being replaced back into the list
            Color2Mat = newColor; //change the material to the new color
            UpdateNodes(NodeManager.ColorState.Color2);
        }
        else
        {
            //Color 2 remains
            int colorPick = Random.Range(0, _newColors.Count - 1);
            Material newColor = _newColors[colorPick];
            _newColors.Remove(newColor);
            _newColors.Add(Color1Mat);
            Color1Mat = newColor;
            UpdateNodes(NodeManager.ColorState.Color1);
        }
    }

    //Called in NextRound when it is time to check which color remains
    //for the color that no longer remains those nodes get marked empty
    private void UpdateNodes(NodeManager.ColorState swappedColor)
    {
        foreach (GameObject node in _nodeList)
        {
            var nodeColor = node.GetComponent<NodeManager>().NodeColor;
            if (nodeColor == swappedColor)
            {
                nodeColor = NodeManager.ColorState.Empty;
            }
        }
    }
}
