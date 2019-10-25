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
        Instantiate(Resources.Load<GameObject>("Prefabs/ColorManager"));
        
        //Instantiate Grid Manager
        Instantiate(Resources.Load<GameObject>("Prefabs/GridManager"));

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
