using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(StateManager))]
public class PlayerController : Character
{
    StateManager manager;

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
