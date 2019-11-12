using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Starts each round and spawns  the rest of the game 
//handles the timing of spawning everything
public class GameManager : MonoBehaviour
{
    //Public
    public float StartDelay;
    public Text TitleText;
    public GameObject NewPaintButton;
    public GameObject ContinueButton;
    public GameObject QuitButton;
    public GameObject SavePaintingButton;
    public GameObject PaintHolder;
    public GameObject SplatHolder;
    
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
        
        SavePaintingButton.SetActive(false);
        AudioManager.Instance.PlaySound(AudioManager.Instance.BackgroundMusic);
    }

    public void GameStart()
    {
        //Turn off menu
        TitleText.text = "";
        NewPaintButton.SetActive(false);
        ContinueButton.SetActive(false);
        QuitButton.SetActive(false);
        SavePaintingButton.SetActive(false);
        
        //Instantiate Holders
        if (PaintHolder != null)
        {
            Destroy(PaintHolder);
        }
        PaintHolder = Instantiate(Resources.Load<GameObject>("Prefabs/PaintHolder"));
        PaintHolder.name = "PaintHolder";

        if (SplatHolder != null)
        {
            Destroy(SplatHolder);
        }
        SplatHolder = Instantiate(Resources.Load<GameObject>("Prefabs/SplatHolder"));
        SplatHolder.name = "SplatHolder";
        
        //Instantiate color manager
        if (ColorManager.Instance != null)
        {
            Destroy(ColorManager.Instance);
        }
        GameObject colorManager = Instantiate(Resources.Load<GameObject>("Prefabs/ColorManager"));
        colorManager.name = "ColorManager";
        colorManager.GetComponent<ColorManager>().SortNum = 0;
        
        //Instantiate Grid Manager
        if (GridManager.Instance != null)
        {
            Destroy(GridManager.Instance);
        }
        GameObject grid = Instantiate(Resources.Load<GameObject>("Prefabs/GridManager"));
        grid.name = "GridManager";

        //Instantiate players
        _p1 = Instantiate(Resources.Load<GameObject>("Prefabs/Brush1"));
        _p1Script = _p1.GetComponent<GridMove>();
        _p1Script.SetState(GridMove.PlayerState.Idle);
        _p2 = Instantiate(Resources.Load<GameObject>("Prefabs/Brush2"));
        _p2Script = _p2.GetComponent<GridMove>();
        _p2Script.SetState(GridMove.PlayerState.Idle);
        
        StartCoroutine(StartRound());
    }

    public void PlayerDeath(int playerId)
    {
        ColorManager.Instance.IncreaseSort();
        //Player 1 died
        if (playerId == 0)
        {
            Destroy(_p1);
            //Instantiate players
            _p1 = Instantiate(Resources.Load<GameObject>("Prefabs/Brush1"));
            _p1Script = _p1.GetComponent<GridMove>();
            _p1Script.SetState(GridMove.PlayerState.Idle);
        }
        else
        {
            Destroy(_p2);
            _p2 = Instantiate(Resources.Load<GameObject>("Prefabs/Brush2"));
            _p2Script = _p2.GetComponent<GridMove>();
            _p2Script.SetState(GridMove.PlayerState.Idle);
        } 
        
        //Change colors
        ColorManager.Instance.NewColors();
        _p1Script.SetColor(ColorManager.Instance.Mat1);
        _p2Script.SetColor(ColorManager.Instance.Mat1);

        StartCoroutine(StartRound());
    }

    private IEnumerator StartRound()
    {
        _timer = StartDelay;
        while (_timer > 0)
        {
            _timer -= Time.deltaTime;
            yield return null;
        }
        //Launch the game
        _p1Script.SetState(GridMove.PlayerState.Painting);
        _p2Script.SetState(GridMove.PlayerState.Painting);
    }

    public void GameEnd()
    {
        //Stop the players from painting
        _p1Script.SetState(GridMove.PlayerState.Idle);
        _p2Script.SetState(GridMove.PlayerState.Idle);
        
        //Remove player heads
        Destroy(_p1);
        Destroy(_p2);
        
        //Offer option to screenshot
        //Offer option to keep playing
        TitleText.text = "Game Over";
        NewPaintButton.SetActive(true);
        ContinueButton.SetActive(true);
        QuitButton.SetActive(true);
        SavePaintingButton.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
