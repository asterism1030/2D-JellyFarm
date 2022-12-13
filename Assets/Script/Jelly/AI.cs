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
////
namespace EnumManager {
    public enum State
    {
        doNothing,  // 아무것도 안함
        doWaiting, // 일반 대기
        doWalking,
        doCounting, // 동작 멈추고 시간 잼
    }
}


public class AI : MonoBehaviour
{
    State CURSTATE = State.doWaiting;

    Animator animator = null;
    Renderer render = null;

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
    public int Level { get { return level; } }


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

        UserInfo.Instance.curJellyNum++;

        StartCoroutine(Clocking());
    }


    void Update()
    {
        
        DoAI();

    }


    void OnDestroy()
    {
        UserInfo.Instance.curJellyNum--;
        Destroy(gameObject);
    }


    void OnMouseDown()
    {
        if(GameManager.Instance.IsAnyWindowOpend == true) {
            return;
        }

        SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Touch);
        
        // order in layer
        render.sortingOrder = 5;

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

        if(CURSTATE == State.doCounting) {
            return;
        }

        // GameManager.Instance.SelectedJelly = this;

        Vector3 inputPos = GameManager.Instance.GetWorldPoint() - bfMousePos;
        transform.Translate(inputPos.x, inputPos.y, 0);
        bfMousePos = GameManager.Instance.GetWorldPoint();
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
        if(inputPos.x < GameInfo.Instance.TopLeft.x) {
            inputPos.x = GameInfo.Instance.TopLeft.x;
        }
        else if(inputPos.x > GameInfo.Instance.BottomRight.x) {
            inputPos.x = GameInfo.Instance.BottomRight.x;
        }
        if(inputPos.y > GameInfo.Instance.TopLeft.y) {
            inputPos.y = GameInfo.Instance.TopLeft.y;
        }
        else if(inputPos.y < GameInfo.Instance.BottomRight.y) {
            inputPos.y = GameInfo.Instance.BottomRight.y;
        }

        transform.position = new Vector3(inputPos.x, inputPos.y, transform.position.z);
        bfMousePos = Vector3.zero;

        CURSTATE = State.doWalking;
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
                    bool isTimerRunning = AITimer(walkingDuration);

                    if(isTimerRunning == true) {
                        break;
                    }

                    speedX = Random.Range(-0.2f, 0.2f);
                    speedY = Random.Range(-0.2f, 0.2f);

                    animator.SetBool(walkingTrigger, true);
                    CURSTATE = State.doWalking;
                }
                break;
            case State.doWalking:
                {
                    bool isTimerRunning = AITimer(Random.Range(3.0f, 5.0f));

                    if(isTimerRunning == true) {
                        Walking();
                        break;
                    }

                    animator.SetBool(walkingTrigger, false);
                    CURSTATE = State.doWaiting;
                }
                break;
            case State.doCounting:
                {
                    AITimer(0.0f);
                    CURSTATE = State.doNothing;
                }
                break;
            default:
                break;
        }
    }


    // Use In doAI
    // return isAITimer is running
    bool AITimer(float setTime)
    {
        timer += Time.deltaTime;
        if(timer <= waitTime) {
            return true;
        }

        startTime = Time.deltaTime;
        timer = 0.0f;
        waitTime = setTime;

        return false;
    }

    void Walking()
    {
        Vector3 pos = transform.position;

        // 경계 밖
        if(pos.x < GameInfo.Instance.TopLeft.x || pos.x > GameInfo.Instance.BottomRight.x
        || pos.y > GameInfo.Instance.TopLeft.y || pos.y < GameInfo.Instance.BottomRight.y) {
            int index = Random.Range(0, 2);
            transform.position = Vector3.MoveTowards(transform.position, GameInfo.Instance.PointList[index], Time.deltaTime);
        }
        else {
            transform.Translate(Time.deltaTime * speedX, Time.deltaTime * speedY, 0);
        }
    }


    // Etc Function
    void GetJelatine()
    {
        UserInfo.Instance.Jelatine += (id + 1) * (level + 1) * (UserInfo.Instance.clickGroupJelly);

        Debug.Log(id);
        Debug.Log(level);
    }


    void ChangeLev()
    {
        if(level == 2) {
            return;
        }

        int[] expList = GameInfo.Instance.MaxExp;

        if(expList[level] >= exp) {
            return;
        }

        SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Grow);

        exp = 0;
        level++;
        level = (level > 2) ? 2 : level;

        GameManager.Instance.ChangeAc(ref animator, level);
    }

    // IEnumerator
    IEnumerator Clocking()
    {
        yield return new WaitForSeconds(5.0f);

        // 1초마다 젤리 경험치 +1, 골드 생산
        while(true) {
            exp++;
            ChangeLev();
            UserInfo.Instance.Gold += (level * price) / 10;

            yield return new WaitForSeconds(1.0f);
        }
    }
    
}
