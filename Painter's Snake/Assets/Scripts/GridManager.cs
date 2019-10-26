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
    private GridMove _brush1Script;
    private GridMove _brush2Script;
    
    //Singleton
    public static GridManager Instance;
    
    // Start is called before the first frame update
    void Start()
    {
        //Set up the singleton
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        
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
        
        /*GameObject newBrush1 = Instantiate(Brush1);
        GameObject newBrush2 = Instantiate(Brush2);
        _brush1Script = newBrush1.GetComponent<GridMove>();
        _brush2Script = newBrush2.GetComponent<GridMove>();*/
    }
    
    //Called when a new round begins
    public void NextRound()
    {
    }
}
