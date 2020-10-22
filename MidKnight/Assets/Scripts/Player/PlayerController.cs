using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(StateManager))]
public class PlayerController : Character
{
    private StateManager manager;
    [SerializeField]
    private GameManager gm;
    /// <summary>
    /// The storage location for the players movement infromation
    /// </summary>
    [HideInInspector]
    public Movement movement;
    private bool moveRight;

    public bool MoveRight
    {
        set
        {
            moveRight = value;
        }
    }

    protected override void AwakeExtra()
    {
        manager = GetComponent<StateManager>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        manager.CallStart(this);
    }

    private void Update()
    {
        manager.DoState(this);
    }

    public override void Move(Vector3 moveVec)
    {
        if (gm == null)
        {   //If we don't have a gameManager, move along the x axis only
            moveVec.z = 0;
            cc.Move(moveVec);
            return;
        }
        //Find our distance along the path

        //Move moveVec along the path and store its final position

        //Set moveVec to be the difference between our current position and the final position

        cc.Move(moveVec);
        //Update the direction of movement after we move
        movement.Direction = gm.GetPathDirection(transform.position, moveRight);
    }
}
