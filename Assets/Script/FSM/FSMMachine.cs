using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class FSMMachine : BaseFSM
{
    enum State { NONE, ENTER, LOGICUPDATE, PHYSICUPDATE, EXIT };

    State curState = State.NONE;
    [Header("---------------------------------------------------    현재 동작    --------")]
    public string curAct = null;

    [Header("---------------------------------------------------    공통 변수    --------")]
    public CommonValues commonVal;

    void Start()
    {
        foreach(Acting act in idleActs.Values) {
            act.FSM = this;
        }
        foreach(Acting act in anyActingList) {
            act.FSM = this;
        }
    }

    void FixedUpdate()
    {
        // AnyState 동작
        foreach(Acting act in anyActingList) {
            act.Enter();
            act.PhysicUpdate();
        }

        // Entry -> Idle 을 거치는 동작
        if(curAct == null || idleActs.ContainsKey(curAct) == false) {
            return;
        }

        switch(curState) {
            case State.NONE:                            // = idle
            {
                idleActs[curAct].Init();
                curState = State.ENTER;
            }
            break;
            case State.ENTER:                           // Enter to Node
            {
                idleActs[curAct].Enter();
                curState = State.PHYSICUPDATE;
            }
            break;
            case State.PHYSICUPDATE:
            {

                idleActs[curAct].PhysicUpdate();
                curState = State.LOGICUPDATE;
            }
            break;
        }
        
    }

    void Update()
    {
        // AnyState 동작
        foreach(Acting act in anyActingList) {
            act.LogicUpdate();
            act.Exit();
        }

        // Entry -> Idle 을 거치는 동작
        if(curAct == null || !idleActs.ContainsKey(curAct)) {
            return;
        }

        switch(curState) {
            case State.LOGICUPDATE:
            {
                idleActs[curAct].LogicUpdate();

                // isExit
                if(idleActs[curAct].isExit == true) {
                    curState = State.EXIT;
                }
                else {
                    curState = State.PHYSICUPDATE;
                }

            }
            break;
            case State.EXIT:
            {
                idleActs[curAct].Exit();

                if(idleActs[curAct].NextAct != null) {
                    curAct = idleActs[curAct].NextAct;
                    curState = State.NONE;
                }
                
            }
            break;
        }
    }
}
