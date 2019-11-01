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
    private GameObject _colorQuad;
    
    // Start is called before the first frame update
    void Start()
    {
        NodeColor = null;
        _colorQuad = null;
    }

    public void ColorChange(Material newColor, GameObject newQuad)
    {
        if (_colorQuad != null)
        {
            Destroy(_colorQuad);
        }
        NodeColor = newColor;
        _colorQuad = newQuad;
    }
}
