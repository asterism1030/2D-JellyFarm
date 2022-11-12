using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using EnumManager;

////
//  젤리의 AI 동작
//
// 몇초 기다리고 이벤트 실행

// TODO) 젤리들이 겹칠때 우선순위 정리해야할듯
////

namespace EnumManager {
    public enum State
    {
        doNothing,  // 정말 아무것도 안함

        doWaiting, // 일반 대기
        doWalking,
        doDraging,
        doCounting, // 동작 멈추고 시간 잼
    }
}


public class AI : MonoBehaviour //, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    State CURSTATE = State.doWaiting;

    Animator animator = null;
    Renderer render = null; // TODO) sortingOrder 정리

    string jellyName = "";
    int id = 0;     // 0 ~
    int level = 0;  // 0 ~
    int exp = 0;    // 0 ~ 데이터 가져오도록 해야함!
    int price = 0;

    // Timer
    float startTime = 0.0f;
    float timer = 0.0f;
    float waitTime = 0.0f;

    // Walking
    string walkingTrigger = "isWalk";
    float walkingDuration = 2.5f;

    float speedX = 0.0f;
    float speedY = 0.0f;

    // Touch
    string touchTrigger = "doTouch";

    // Draging
    float dragStartTime = 1.0f;
    Vector3 bfMousePos = Vector3.zero;


    // get set
    public int Price { get { return price; } set { price = value; } }


    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        if(animator == null) {
            Debug.LogError("animator is null");
        }

        render = gameObject.GetComponent<Renderer>();
        if(render == null) {
            Debug.LogError("render is null");
        }

        jellyName = this.name;
        if(jellyName == null) {
            Debug.LogError("jellyName is null");
        }

        #if UNITY_EDITOR
        string acName = (animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController).name;
        if(acName == null) {
            Debug.LogError("acName is null");
        }

        int[] info = GameManager.Instance.GetJellyIDLev(jellyName, acName);
        id = info[0];
        level = info[1];
        price = info[2];
        #endif

        GameManager.Instance.curJellyNum++;

        StartCoroutine(Clocking());
    }

    // Update is called once per frame
    void Update()
    {
        
        DoAI();

    }

    // 물리 엔진 동작이 끝난 뒤인 FixedUpdate 에서 호출
    /*
    void FixedUpdate()
    {
        switch(CURSTATE) {
            case State.doDraging:
                {
                    Dragging();
                }
                break;
            default:
                break; 
        }
    }
    */

    void OnDestroy()
    {
        GameManager.Instance.curJellyNum--;
    }


    // Collider 추가 시 아래의 이벤트 이용 가능
    // 참고) UI 의 경우 inspector 창에서 이벤트 트리거 사용 가능 (이건 UI 가 아니므로 X)
    void OnMouseDown()
    {
        if(GameManager.Instance.IsAnyWindowOpend == true) {
            return;
        }

        SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Touch);

        animator.SetTrigger(touchTrigger);
        GetJelatine();
        exp++;
        ChangeLev();

        bfMousePos = GameManager.Instance.GetWorldPoint();
        waitTime = dragStartTime;

        CURSTATE = State.doCounting;
    }


    public void OnMouseDrag()
    {
        if(GameManager.Instance.IsAnyWindowOpend == true) {
            return;
        }

        // order in layer
        render.sortingOrder = 5;

        // Dragging();

        if(CURSTATE == State.doNothing) {
            GameManager.Instance.SelectedJelly = gameObject;
            CURSTATE = State.doDraging;
        }
    }

    public void OnMouseUp()
    {
        if(GameManager.Instance.IsAnyWindowOpend == true) {
            return;
        }

        // order in layer
        render.sortingOrder = 0;
        
        GameManager.Instance.SelectedJelly = null;

        Vector3 offset = GameManager.Instance.GetWorldPoint() - bfMousePos;
        Vector3 inputPos = transform.position + offset;

        // 경계 밖
        if(inputPos.x < GameManager.Instance.TopLeft.x) {
            inputPos.x = GameManager.Instance.TopLeft.x;
        }
        else if(inputPos.x > GameManager.Instance.BottomRight.x) {
            inputPos.x = GameManager.Instance.BottomRight.x;
        }
        if(inputPos.y > GameManager.Instance.TopLeft.y) {
            inputPos.y = GameManager.Instance.TopLeft.y;
        }
        else if(inputPos.y < GameManager.Instance.BottomRight.y) {
            inputPos.y = GameManager.Instance.BottomRight.y;
        }

        transform.position = new Vector3(inputPos.x, inputPos.y, transform.position.z);

        bfMousePos = Vector3.zero;

        CURSTATE = State.doNothing;
    }


    void DoAI()
    {
        // AI 동작
        switch(CURSTATE) {
            case State.doNothing:
                {

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

                    startTime = Time.deltaTime;
                    timer = 0.0f;
                    waitTime = Random.Range(3.0f, 5.0f);

                    CURSTATE = State.doWaiting;
                }
                break;
            case State.doCounting:
                {
                    timer += Time.deltaTime;
                    if(timer <= waitTime) {
                        break;
                    }

                    startTime = Time.deltaTime;
                    timer = 0.0f;
                    waitTime = 0.0f;

                    CURSTATE = State.doNothing;
                }
                break;
            case State.doDraging:
                {
                    Dragging();
                }
                break;
            default:
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

    void Dragging()
    {
        Vector3 inputPos = GameManager.Instance.GetWorldPoint() - bfMousePos;
        transform.Translate(inputPos.x, inputPos.y, 0);
        bfMousePos = GameManager.Instance.GetWorldPoint();
    }


    void GetJelatine()
    {
        GameManager.Instance.Jelatine += (id + 1) * (level + 1) * (GameManager.Instance.clickGroupJelly);
        // GameManager.Instance.Gold += (GameManager.Instance.clickGroupJelly) * 10;

        Debug.Log(id);
        Debug.Log(level);
    }


    void ChangeLev()
    {
        if(level == 2) {
            return;
        }

        int[] expList = GameManager.Instance.MaxExp;

        if(expList[level] >= exp) {
            return;
        }

        SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Grow);

        exp = 0;
        level++;
        level = (level > 2) ? 2 : level;

        GameManager.Instance.ChangeAc(ref animator, level);
    }

    IEnumerator Clocking()
    {
        yield return new WaitForSeconds(5.0f);

        // 1초마다 젤리 경험치 +1
        while(true) {
            exp++;
            ChangeLev();

            // Debug.Log(exp);

            yield return new WaitForSeconds(1.0f);
        }
    }
    
}
