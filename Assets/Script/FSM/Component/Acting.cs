using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class Acting : MonoBehaviour
{
    // values
    private FSMMachine fsm;

    [HideInInspector]
    public bool isExit = false;

    protected Acting nextAct;

    // get set
    public FSMMachine FSM { get{ return fsm; } set{ if(value != null) fsm = value; } }
    public string NextAct { get{ if(nextAct != null) return nextAct.GetType().Name; else return null; } }

    // function
    public abstract void Enter();

    public abstract void PhysicUpdate();

    public abstract void LogicUpdate();

    public abstract void Exit();

    public void Init()
    {
        isExit = false;
    }


    protected virtual void Awake() {}       // = Init
    protected virtual void OnEnable() {}
    protected virtual void Start() {}       // = Enter
    
    protected virtual void FixedUpdate() {} // = PhysicUpdate
    protected virtual void Update() {}      // = LogicUpdate
    protected virtual void LateUpdate() {}
    
}