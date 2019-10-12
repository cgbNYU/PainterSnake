using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Holds the color state of the node
public class NodeManager : MonoBehaviour
{
    //Public
    public enum ColorState
    {
        Color1,
        Color2,
        Empty
    }

    public ColorState NodeColor;
    
    // Start is called before the first frame update
    void Start()
    {
        NodeColor = ColorState.Empty;
    }

    public void ColorChange(ColorState newColor)
    {
        NodeColor = newColor;
    }
}
