using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.EventSystems;


[Serializable]
public class Dragging_Jelly : Acting
{
    CommonVal_Jelly commonVal;

    Vector3 bfMousePos = Vector3.zero;
    bool isDragging = false;

    [SerializeField]
    private List<Acting> nextActList;


    public override void Enter()
    {
        if(commonVal == null) {
            commonVal = (CommonVal_Jelly)(FSM.commonVal);
        }
    }

    public override void PhysicUpdate()
    {
        if(commonVal == null) {
            commonVal = (CommonVal_Jelly)(FSM.commonVal);
        }
    }

    public override void LogicUpdate()
    {
        if(GameManager.Instance.IsAnyWindowOpend == true) {
            return;
        }

        // 누르는 순간
        if(Input.GetMouseButtonDown((int)MouseState.LBTN)) {
            // Raycast
            // TODO ) FixedUpdate()
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider != null)
            {
                if(hit.collider.gameObject != commonVal.Target) {
                    return;
                }
            }
            else {
                return;
            }
            
            bfMousePos = GameManager.Instance.GetWorldPoint();

            // 최초 클릭
            if(isDragging == false) {
                // sound
                SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Touch);
                // order in layer
                commonVal.Render.sortingOrder = GameManager.Instance.ClickedSortingOrder;
                // jelly
                commonVal.Anim.SetTrigger(GameManager.Instance.TouchTrigger);
                commonVal.JellySC.GetJelatine();
                commonVal.JellySC.Exp = commonVal.JellySC.Exp + 1;
                commonVal.JellySC.ChangeLev();

                isDragging = true;
                commonVal.isFreeze = true;
                return;
            }
        }

        // 누르는 동안 (드래깅)
        if(Input.GetMouseButton((int)MouseState.LBTN)) {
            if(isDragging == false) {
                return;
            }

            Debug.LogWarning("MB D" + isDragging);

            GameManager.Instance.SelectedJelly = commonVal.JellySC;
            // Move jelly
            Vector3 inputPos = GameManager.Instance.GetWorldPoint() - bfMousePos;
            commonVal.Target.transform.Translate(inputPos.x, inputPos.y, 0);

            bfMousePos = GameManager.Instance.GetWorldPoint();
        }

        // 떼는 순간
        if(Input.GetMouseButtonUp((int)MouseState.LBTN)) {
            if(isDragging == false) {
                return;
            }

            isDragging = false;
            commonVal.isFreeze = false;
            // order in layer
            commonVal.Render.sortingOrder = GameManager.Instance.DefaultSortingOrder;

            GameManager.Instance.SelectedJelly = null;
        
            Vector3 offset = GameManager.Instance.GetWorldPoint() - bfMousePos;
            Vector3 inputPos = commonVal.Target.transform.position + offset;

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

            commonVal.Target.transform.position = new Vector3(inputPos.x, inputPos.y, commonVal.Target.transform.position.z);
            bfMousePos = Vector3.zero;
        }
    }

    public override void Exit()
    {
        if(commonVal == null) {
            commonVal = (CommonVal_Jelly)(FSM.commonVal);
        }
    }
}
