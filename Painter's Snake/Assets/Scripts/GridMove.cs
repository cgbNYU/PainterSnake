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
    public ColorState StartingColor;
    public int PlayerNum;
    public GameObject PaintTrail;
    
    //Private
    private Vector3 _moveDir;
    private Vector3 _prevDir;
    private Vector3 _target;
    private LineRenderer _lineRenderer;
    private bool _colorSwitch;
    private GridManager _grid;
    private Vector3 _startPos;
    
    
    //Enumerator
    public enum ColorState
    {
        Color1,
        Color2
    }

    private ColorState _playerColor;
    
    public enum PlayerState
    {
        Painting,
        Dead
    }

    private PlayerState _playerState;
    
    //Rewired
    private Rewired.Player _rewiredPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;
        _moveDir = StartingMove;
        _prevDir = _moveDir;
        _rewiredPlayer = ReInput.players.GetPlayer(PlayerNum);
        _colorSwitch = false;
        _playerColor = StartingColor;
        _grid = GameObject.Find("GridManager").GetComponent<GridManager>();
        _target = transform.position;
        _playerState = PlayerState.Painting;
        NewTrail();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_playerState)
        {
            case PlayerState.Painting:
                Move();
                TransformLine();
                ChangeColor();
                break;
            case PlayerState.Dead:
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
            if (_prevDir != _moveDir)
            {
                UpdateLine();
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
        if (_playerColor == ColorState.Color1)
        {
            _colorSwitch = false;
            _playerColor = ColorState.Color2;
            _lineRenderer.material = _grid.Color2Mat;
        }
        else
        {
            _colorSwitch = false;
            _playerColor = ColorState.Color1;
            _lineRenderer.material = _grid.Color1Mat;
        }
    }

    //Called when the Brush triggers a node
    private void NodeColorChange(NodeManager node)
    {
        if (_playerColor == ColorState.Color1)
        {
            if (node.NodeColor == NodeManager.ColorState.Color2 || node.NodeColor == NodeManager.ColorState.Empty)
            {
                node.ColorChange(NodeManager.ColorState.Color1);
            }
            else
            {
                //die
                _playerState = PlayerState.Dead;
                _grid.NextRound();
                Debug.Log("Die " + _playerColor);
            }
        }
        else
        {
            if (node.NodeColor == NodeManager.ColorState.Color1 || node.NodeColor == NodeManager.ColorState.Empty)
            {
                node.ColorChange(NodeManager.ColorState.Color2);
            }
            else
            {
                //die
                _playerState = PlayerState.Dead;
                _grid.NextRound();
                Debug.Log("Die " + _playerColor);
            }
        }
    }

    public void Respawn()
    {
        transform.position = _startPos;
        _moveDir = StartingMove;
        _prevDir = _moveDir;
        _colorSwitch = false;
        _playerColor = StartingColor;
        _target = transform.position;
        _playerState = PlayerState.Painting;
        NewTrail();
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