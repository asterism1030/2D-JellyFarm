using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Waiting_Jelly : Acting
{
    private float runningTime = 0.0f;

    [SerializeField]
    private List<Acting> nextActList;

    CommonVal_Jelly commonVal;

    public override void Enter()
    {
        if(commonVal == null) {
            commonVal = (CommonVal_Jelly)(FSM.commonVal);
        }

        runningTime = UnityEngine.Random.Range(1.0f, 3.5f);        
    }

    public override void PhysicUpdate()
    {
        if(commonVal.isFreeze == true) {
            return;
        }

        runningTime -= Time.deltaTime;
        
        if(runningTime <= 0.0f) {
            isExit = true;
        }
    }

    public override void LogicUpdate()
    {
        
    }

    public override void Exit()
    {
        if(commonVal.isFreeze == true) {
            return;
        }

        nextAct = nextActList[0];
    }
}
