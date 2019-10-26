using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Holds the color state of the node
public class NodeManager : MonoBehaviour
{
    //Public
    public Material NodeColor;
    
    //Private
    private GridManager _grid;
    
    // Start is called before the first frame update
    void Start()
    {
        NodeColor = null;
    }

    public void ColorChange(Material newColor)
    {
        NodeColor = newColor;
    }
}
