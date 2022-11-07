using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////
//  젤리의 AI 동작
//
// 몇초 기다리고 이벤트 실행
////

enum State
{
    doNothing,
    doWaiting,
    doWalking,
    // doSomething
}


public class AI : MonoBehaviour
{
    State CURSTATE = State.doNothing;

    Animator animator;

    // Timer
    float startTime = 0.0f;
    float timer = 0.0f;
    float waitTime = 0.0f;

    // Walking
    string walkingTrigger = "isWalk";
    float walkingDuration = 2.5f;

    float speedX = 0.0f;
    float speedY = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        if(animator == null) {
            Debug.LogError("animator is null");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        DoAI();
        
    }


    void DoAI()
    {
        // AI 동작
        switch(CURSTATE) {
            case State.doNothing:
                {
                    startTime = Time.deltaTime;
                    timer = 0.0f;
                    waitTime = Random.Range(3.0f, 5.0f);

                    CURSTATE = State.doWaiting;
                }
                break;
            case State.doWaiting:
                {
                    timer += Time.deltaTime;
                    if(timer <= waitTime) {
                        break;
                    }

                    startTime = Time.deltaTime;
                    timer = 0.0f;
                    waitTime = walkingDuration;

                    speedX = Random.Range(-0.2f, 0.2f);
                    speedY = Random.Range(-0.2f, 0.2f);

                    animator.SetBool(walkingTrigger, true);

                    CURSTATE = State.doWalking;
                }
                break;
            case State.doWalking:
                {
                    timer += Time.deltaTime;
                    if(timer <= waitTime) {
                        Walking();
                        break;
                    }

                    animator.SetBool(walkingTrigger, false);
                    CURSTATE = State.doNothing;
                }
                break;
        }
    }


    void Walking()
    {
        Vector3 pos = transform.position;

        // 경계 밖
        if(pos.x < GameManager.Instance.TopLeft.x || pos.x > GameManager.Instance.BottomRight.x
        || pos.y > GameManager.Instance.TopLeft.y || pos.y < GameManager.Instance.BottomRight.y) {
            int index = Random.Range(0, 2);
            transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.PointList[index], Time.deltaTime);
        }
        else {
            transform.Translate(Time.deltaTime * speedX, Time.deltaTime * speedY, 0);
        }
    }

    
}
