using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class GridMove : MonoBehaviour
{
    //Public
    public float Speed;
    public float GridDist;
    public Vector3 StartingMove;
    public Vector3 StartPos;
    public ColorState StartingColor;
    public int PlayerNum;
    public GameObject PaintTrail;
    
    //Private
    private Vector3 _moveDir;
    private Vector3 _prevDir;
    private Vector3 _target;
    private Material _currentColor;
    private LineRenderer _lineRenderer;
    private bool _colorSwitch;
    //private GridManager _grid;
    private int _colorId;
    
    
    //Enumerator
    public enum ColorState
    {
        Color1,
        Color2
    }

    //private ColorState _playerColor;
    
    public enum PlayerState
    {
        Painting,
        Dead,
        Idle
    }

    private PlayerState _playerState;
    
    //Rewired
    private Rewired.Player _rewiredPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position;
        _moveDir = StartingMove;
        _prevDir = _moveDir;
        _rewiredPlayer = ReInput.players.GetPlayer(PlayerNum);
        _colorSwitch = false;
        _currentColor = ColorManager.Instance.Mat1;
        //_grid = GameObject.Find("GridManager").GetComponent<GridManager>();
        _target = transform.position;
        _playerState = PlayerState.Painting;
        //NewTrail();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_playerState)
        {
            case PlayerState.Painting:
                Move();
                //TransformLine();
                ChangeColor();
                break;
            case PlayerState.Dead:
                break;
            case PlayerState.Idle:
                break;
            default:
                Debug.Log("Player State error");
                break;
        }     
    }

    private void Move()
    {
        if (_rewiredPlayer.GetButtonDown("Up") && _moveDir != Vector3.down)
        {
            //set move dir to up
            _moveDir = Vector3.up;
        }
        else if (_rewiredPlayer.GetButtonDown("Down") && _moveDir != Vector3.up)
        {
            //set move dir to down
            _moveDir = Vector3.down;
        }
        else if (_rewiredPlayer.GetButtonDown("Left") && _moveDir != Vector3.right)
        {
            _moveDir = Vector3.left;
        }
        else if (_rewiredPlayer.GetButtonDown("Right") && _moveDir != Vector3.left)
        {
            _moveDir = Vector3.right;
        }

        if (transform.position == _target)
        {
            _target += _moveDir * GridDist;
            DropTrail();
            if (_prevDir != _moveDir)
            {
                //UpdateLine();
                //TurnPoint();
                //NewTrail();
                _prevDir = _moveDir;
            }
        }
        
        transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * Speed);
        
    }
    
    //Update the latest line position to be the transform of the Brush
    public void TransformLine()
    {
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, transform.position);
    }
    
    //Add a new position to the location of the player when turning
    public void UpdateLine()
    {
        _lineRenderer.positionCount += 1;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, transform.position);
    }
    
    public void TurnPoint()
    {
        GameObject newTrail = Instantiate(PaintTrail, transform.position, transform.rotation);
        //newTrail.transform.parent = transform;
        _lineRenderer = newTrail.GetComponent<LineRenderer>();
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.material = _currentColor;
        _lineRenderer.sortingOrder = ColorManager.Instance.SortNum;
        ColorManager.Instance.SortNum++;
    }

    private void DropTrail()
    {
        GameObject newTrail = (GameObject)Instantiate(Resources.Load("Prefabs/PaintQuad"), transform.position, transform.rotation);
        newTrail.GetComponent<Renderer>().material = _currentColor;
    }

    //Hit  button to change your character's color
    private void ChangeColor()
    {
        if (_rewiredPlayer.GetButtonDown("ColorChange"))
        {
            _colorSwitch = true;
        }
        if (transform.position == _target && _colorSwitch)
        {
            NewTrail();
        }
    
    }

    //creates a new trail for the new color
    private void NewTrail()
    {
        GameObject newTrail = Instantiate(PaintTrail, transform.position, transform.rotation);
        //newTrail.transform.parent = transform;
        _lineRenderer = newTrail.GetComponent<LineRenderer>();
        _lineRenderer.SetPosition(0, transform.position);
        if (_currentColor == ColorManager.Instance.Mat1)
        {
            _colorSwitch = false;
            _currentColor = ColorManager.Instance.Mat2;
            _lineRenderer.material = _currentColor;
            _lineRenderer.sortingOrder = ColorManager.Instance.SortNum;
            ColorManager.Instance.SortNum++;
        }
        else
        {
            _colorSwitch = false;
            _currentColor = ColorManager.Instance.Mat1;
            _lineRenderer.material = _currentColor;
            _lineRenderer.sortingOrder = ColorManager.Instance.SortNum;
            ColorManager.Instance.SortNum++;
        }
    }

    //Called when the Brush triggers a node
    private void NodeColorChange(NodeManager node)
    {
        if (_playerState == PlayerState.Painting)
        {
            if (_currentColor == node.NodeColor)
            {
                //die
                Debug.Log("Die " + _currentColor);
                GameManager.Instance.PlayerDeath();
            }
            else
            {
                node.ColorChange(_currentColor);
            }
        }
    }

    public void Respawn()
    {
        transform.position = StartPos;
        _moveDir = StartingMove;
        _prevDir = _moveDir;
        _colorSwitch = false;
        _currentColor = ColorManager.Instance.Mat1;
        _target = transform.position;
        _playerState = PlayerState.Painting;
        NewTrail();
    }

    //Called from external functions to change player movement state
    public void SetState(PlayerState newState)
    {
        _playerState = newState;
    }

    //Called externally to change colors
    public void SetColor(Material newColor)
    {
        _currentColor = newColor;
    }

    //Trigger checks for hitting nodes
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Node"))
        {
            NodeColorChange(other.GetComponent<NodeManager>());
        }
    }
}