using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using Rewired;
using Unity.Collections;

public class FourWayMove : MonoBehaviour
{
    
    //Public
    public float Speed;
    public Vector3 StartDir;
    public int PlayerNum;
    public GameObject NewPoint;

    public Vector3 MoveDir => moveDir;

    //Private
    private Vector3 moveDir;
    private Player rewiredPlayer;
    private LineRenderer _lineRenderer;
    private bool _keyPressed;
    
    // Start is called before the first frame update
    void Start()
    {
        moveDir = StartDir;
        rewiredPlayer = ReInput.players.GetPlayer(PlayerNum); //assing player number in inspector
        _lineRenderer = GetComponent<LineRenderer>(); //get the line renderer
        _keyPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        TransformLine();
    }

    public void Move()
    {
        if (rewiredPlayer.GetAxis("Horizontal") > 0 && moveDir != Vector3.left && !_keyPressed)
        {
            _keyPressed = true;
            moveDir = Vector3.right;
            //TurnPoint();
            UpdateLine();
        }
        else if (rewiredPlayer.GetAxis("Horizontal") < 0 && moveDir != Vector3.right && !_keyPressed)
        {
            _keyPressed = true;
            moveDir = Vector3.left;
            //TurnPoint();
            UpdateLine();
        }
        else if (rewiredPlayer.GetAxis("Vertical") > 0 && moveDir != Vector3.down && !_keyPressed)
        {
            _keyPressed = true;
            moveDir = Vector3.up;
            //TurnPoint();
            UpdateLine();
        }
        else if (rewiredPlayer.GetAxis("Vertical") < 0 && moveDir != Vector3.up && !_keyPressed)
        {
            _keyPressed = true;
            moveDir = Vector3.down;
            //TurnPoint();
            UpdateLine();
        }

        if (rewiredPlayer.GetAxis("Vertical") == 0 && rewiredPlayer.GetAxis("Horizontal") == 0)
        {
            _keyPressed = false;
        }
        transform.position += moveDir * Speed * Time.deltaTime;
    }

    //Add a new position to the location of the player when turning
    public void UpdateLine()
    {
        _lineRenderer.positionCount += 1;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, transform.position);
    }

    public void TurnPoint()
    {
        GameObject newPoint = Instantiate(NewPoint, transform.position, transform.rotation);
        LineRenderer newLine = newPoint.GetComponent<LineRenderer>();
        newLine.SetPosition(0, transform.position);
        _lineRenderer = newLine;
    }

    //Update the latest line position to be the transform of the Brush
    public void TransformLine()
    {
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, transform.position);
    }
}
