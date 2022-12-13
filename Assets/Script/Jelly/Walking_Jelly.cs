using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class Walking_Jelly : Acting
{
    private float runningTime = 0.0f;
    private float speedX = 0.0f;
    private float speedY = 0.0f;

    [SerializeField]
    private List<Acting> nextActList;

    CommonVal_Jelly commonVal;


    public override void Enter()
    {
        if(commonVal == null) {
            commonVal = (CommonVal_Jelly)(FSM.commonVal);
        }

        commonVal.Anim.SetBool(GameManager.Instance.WalkigTrigger, true);
        runningTime = UnityEngine.Random.Range(2.0f, 4.5f);

        speedX = UnityEngine.Random.Range(-0.2f, 0.2f);
        speedY = UnityEngine.Random.Range(-0.2f, 0.2f);
    }

    public override void PhysicUpdate()
    {
        runningTime -= Time.deltaTime;
        if(runningTime <= 0.0f) {
            isExit = true;
        }
    }

    public override void LogicUpdate()
    {
        CheckToAnimState(true);

        Vector3 pos = commonVal.Target.transform.position;

        // 경계 밖
        if(pos.x < GameInfo.Instance.TopLeft.x || pos.x > GameInfo.Instance.BottomRight.x
        || pos.y > GameInfo.Instance.TopLeft.y || pos.y < GameInfo.Instance.BottomRight.y) {
            int index = UnityEngine.Random.Range(0, 2);
            commonVal.Target.transform.position = Vector3.MoveTowards(commonVal.Target.transform.position, GameInfo.Instance.PointList[index], Time.deltaTime);
        }
        else {
            commonVal.Target.transform.Translate(Time.deltaTime * speedX, Time.deltaTime * speedY, 0);
        }
    }

    public override void Exit()
    {
        CheckToAnimState(false);
        
        nextAct = nextActList[0];
    }

    void CheckToAnimState(bool isTriggered)
    {
        if(commonVal.isFreeze == true) {
            commonVal.Anim.SetBool(GameManager.Instance.WalkigTrigger, false);
            return;
        }

        if(isTriggered == false) {
            commonVal.Anim.SetBool(GameManager.Instance.WalkigTrigger, false);
        }
        else if(commonVal.Anim.GetBool(GameManager.Instance.WalkigTrigger) == false && isTriggered == true) {
            commonVal.Anim.SetBool(GameManager.Instance.WalkigTrigger, true);
        }
        
    }
}
