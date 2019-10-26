using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Starts each round and spawns  the rest of the game 
//handles the timing of spawning everything
public class GameManager : MonoBehaviour
{
    //Public
    
    
    //Private
    private float _timer;
    private GameObject _p1;
    private GameObject _p2;
    private GridMove _p1Script;
    private GridMove _p2Script;
    
    //Singleton
    public static GameManager Instance;
    
    // Start is called before the first frame update
    void Start()
    {
        //Set up the singleton
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        
        GameStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GameStart()
    {
        //Instantiate color manager
        GameObject colorManager = Instantiate(Resources.Load<GameObject>("Prefabs/ColorManager"));
        colorManager.name = "ColorManager";
        
        //Instantiate Grid Manager
        GameObject grid = Instantiate(Resources.Load<GameObject>("Prefabs/GridManager"));
        grid.name = "GridManager";

        //Instantiate players
        _p1 = Instantiate(Resources.Load<GameObject>("Prefabs/Brush1"));
        _p1Script = _p1.GetComponent<GridMove>();
        _p1Script.SetState(GridMove.PlayerState.Idle);
        _p2 = Instantiate(Resources.Load<GameObject>("Prefabs/Brush2"));
        _p2Script = _p2.GetComponent<GridMove>();
        _p2Script.SetState(GridMove.PlayerState.Idle);

        //Launch the game
        _p1Script.SetState(GridMove.PlayerState.Painting);
        _p2Script.SetState(GridMove.PlayerState.Painting);
    }

    public void PlayerDeath()
    {
        //Pause both players
        /*_p1Script.SetState(GridMove.PlayerState.Idle);
        _p2Script.SetState(GridMove.PlayerState.Idle);*/
        
        //Change colors
        ColorManager.Instance.NewColors();
        _p1Script.SetColor(ColorManager.Instance.Mat1);
        _p2Script.SetColor(ColorManager.Instance.Mat1);
        
        /*//Reset player position
        _p1.transform.position = _p1Script.StartPos;
        _p2.transform.position = _p2Script.StartPos;
        
        //Launch
        _p1Script.SetState(GridMove.PlayerState.Painting);
        _p2Script.SetState(GridMove.PlayerState.Painting);*/
        
        Destroy(_p1);
        Destroy(_p2);
        
        //Instantiate players
        _p1 = Instantiate(Resources.Load<GameObject>("Prefabs/Brush1"));
        _p1Script = _p1.GetComponent<GridMove>();
        _p1Script.SetState(GridMove.PlayerState.Idle);
        _p2 = Instantiate(Resources.Load<GameObject>("Prefabs/Brush2"));
        _p2Script = _p2.GetComponent<GridMove>();
        _p2Script.SetState(GridMove.PlayerState.Idle);

        //Launch the game
        _p1Script.SetState(GridMove.PlayerState.Painting);
        _p2Script.SetState(GridMove.PlayerState.Painting);
    }
}
