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

    public Material Mat1;
    public Material Mat2;

    public Color color1;
    
    //Color picking int
    private int _colorNum;
    
    //Color layering int
    public int SortNum;
    
    //Determines if you are finished
    private bool _lastRound;
    
    // Start is called before the first frame update
    void Start()
    {
        //Set up the singleton
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        
        SortNum = 1;
        
        //StartingColors();
        color1 = PaintMaterial[0].color;
    }

    public void StartingColors()
    {
        _colorNum = 0;
        Mat1 = PaintMaterial[_colorNum];
        Mat2 = PaintMaterial[_colorNum + 1];
        _colorNum++;
        
        _lastRound = false;
    }

    public void NewColors()
    {
        if (_lastRound)
        {
            GameManager.Instance.GameEnd();
        }
        else if (_colorNum == PaintMaterial.Length - 1)
        {
            Mat1 = PaintMaterial[_colorNum];
            Mat2 = PaintMaterial[0];
            _lastRound = true;
        }
        else
        {
            Mat1 = PaintMaterial[_colorNum];
            Mat2 = PaintMaterial[_colorNum + 1];
            _colorNum++;
        }
    }

    public void IncreaseSort()
    {
        SortNum++;
    }
}
