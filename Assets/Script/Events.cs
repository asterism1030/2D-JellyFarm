using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Events : MonoBehaviour
{
    // TODO) GameManager 로 이동 필요
    [SerializeField]
    private Sprite nomalSprite;
    [SerializeField]
    private Sprite selectSprite;

    [SerializeField]
    private Animator animatorPanel;
    
    string doShow = "doShow";
    string doHide = "doHide";

    bool isShow = false;

    // Start is called before the first frame update
    void Start()
    {
        // animator = Panel.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO) 키 이벤트 따로 클래스
        // if(Input.GetKey(KeyCode.Escape)) {
        //     isShow = false;
        //     PanelShowHide();
        // }
    }

    // Event Trigger
    public void SellEvent()
    {
        GameObject selectedJelly = GameManager.Instance.SelectedJelly;

        if(selectedJelly == null) {
            return;
        }

        AI jelly = (AI)selectedJelly.GetComponent(typeof(AI));
        
        GameManager.Instance.Gold += jelly.Price;
        Destroy(selectedJelly);

        Debug.Log("sell" + jelly.Price);
    }

    // OnClick
    // TODO) 다른 버튼 눌렀을때 창 닫기
    public void PanelShowHide()
    {
        if(isShow == false) {
            PanelShow();
        }
        else {
            PanelHide();
        }
    }

    public void PanelShow()
    {
        if(animatorPanel == null) {
            Debug.Log("animatorPanel is null");
            return;
        }

        GameManager.Instance.IsAnyWindowOpend = true;

        animatorPanel.SetTrigger(doShow);
        this.GetComponent<Image>().sprite = selectSprite;

        isShow = true;
    }

    public void PanelHide()
    {
        if(animatorPanel == null) {
            Debug.Log("animatorPanel is null");
            return;
        }

        GameManager.Instance.IsAnyWindowOpend = false;

        animatorPanel.SetTrigger(doHide);
        this.GetComponent<Image>().sprite = nomalSprite;

        isShow = false;
    }

}
