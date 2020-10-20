using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(StateManager))]
public class PlayerController : Character
{
    private StateManager manager;
    /// <summary>
    /// The storage location for the players movement infromation
    /// </summary>
    [HideInInspector]
    public Movement movement;

    protected override void AwakeExtra()
    {
        manager = GetComponent<StateManager>();
    }

    private void Start()
    {
        manager.CallStart(this);
    }

    private void Update()
    {
        manager.DoState(this);
    }
}
