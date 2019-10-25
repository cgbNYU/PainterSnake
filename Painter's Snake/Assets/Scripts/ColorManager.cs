using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Holds the enumerator for all the colors used by the Nodes and players\
//Colors are listed generically, but colors will be assigned when people pick a palette
public class ColorManager : MonoBehaviour
{
    //Enumerator
    public enum ColorState
    {
        Color1,
        Color2,
        Color3,
        Color4,
        Color5
    }
    
    //Singleton
    public static ColorManager Instance = null;
    
    //Materials
    public Material[] PaintMaterial;
    
    // Start is called before the first frame update
    void Start()
    {
        //Set up the singleton
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
}
