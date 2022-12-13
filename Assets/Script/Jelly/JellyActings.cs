using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
/*
namespace FSM_Jelly
{   
    public class Walking : Acting
    {
        public override void Init()
        {
            if(fsm.Values == null) {
                Debug.LogError("FSMValue is null");
                return;
            }

            fsm.Values.speedX = 0.0f;
            fsm.Values.speedY = 0.0f;

            fsm.Values.Anim.SetBool(GameManager.Instance.walkingTrigger, false);
        }

        public override void Set()
        {
            if(fsm.Values == null) {
                Debug.LogError("FSMValue is null");
                return;
            }

            fsm.Values.speedX = UnityEngine.Random.Range(-0.2f, 0.2f);
            fsm.Values.speedY = UnityEngine.Random.Range(-0.2f, 0.2f);

            fsm.Values.Anim.SetBool(GameManager.Instance.walkingTrigger, true);
        }

        public override void Run()
        {
            if(fsm.Values == null) {
                Debug.LogError("FSMValue is null");
                return;
            }

            Vector3 pos = fsm.Values.Target.transform.position;

            // 경계 밖
            if(pos.x < GameInfo.Instance.TopLeft.x || pos.x > GameInfo.Instance.BottomRight.x
            || pos.y > GameInfo.Instance.TopLeft.y || pos.y < GameInfo.Instance.BottomRight.y) {
                int index = UnityEngine.Random.Range(0, 2);
                fsm.Values.Target.transform.position = Vector3.MoveTowards(fsm.Values.Target.transform.position, GameInfo.Instance.PointList[index], Time.deltaTime);
            }
            else {
                fsm.Values.Target.transform.Translate(Time.deltaTime * fsm.Values.speedX, Time.deltaTime * fsm.Values.speedY, 0);
            }
        }

        public override void Exit()
        {
            if(fsm.Values == null) {
                Debug.LogError("FSMValue is null");
                return;
            }

            fsm.Values.speedX = 0.0f;
            fsm.Values.speedY = 0.0f;

            fsm.Values.Anim.SetBool(GameManager.Instance.walkingTrigger, false);
        }
    }

    public class Waiting : Acting
    {

    }
}

public class JellyActings {}

*/