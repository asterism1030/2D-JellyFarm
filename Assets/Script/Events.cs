using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Events : MonoBehaviour
{
    // TODO) GameManager 로 이동 필요
    // TODO) OptionPanel 기능 구현 (영상의 경우 키 입력)
    [SerializeField]
    private Sprite nomalSprite;
    [SerializeField]
    private Sprite selectSprite;

    [SerializeField]
    private Animator animatorPanel;
    
    string doShow = "doShow";
    string doHide = "doHide";

    bool isShow = false;

    // 페이지는 0부터
    // TODO) 별도 스크립트 분리 필요
    int curPageNum = 0;

    public GameObject LockGroup;
    public Image LockjellyImage;
    public Text LockjellyCost;

    public Image UnlockjellyImage;
    public Text UnlockjellyName;
    public Text UnlockjellyCost;

    public Text jellyPage;

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

        SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Sell);

        AI jelly = (AI)selectedJelly.GetComponent(typeof(AI));
        
        GameManager.Instance.Gold += jelly.Price;

        Destroy(selectedJelly);

        Debug.Log("sell" + jelly.Price);
    }

    // OnClick
    // TODO) 창 밖 눌렀을 때 창 닫기
    public void PanelShowHide()
    {
        SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Button);

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

        if(GameManager.Instance.IsAnyWindowOpend == true) {
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


    public void PageDown()
    {
        SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Button);

        if(curPageNum == 0) {
            return;
        }

        curPageNum--;
        ChangeJellyPanelInfo();
    }

    public void PageUp()
    {
        SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Button);

        if(curPageNum == 11) {
            return;
        }

        curPageNum++;
        ChangeJellyPanelInfo();
    }


    // TODO) 판넬 최초 띄울 시 적용이 안됨 (#01)
    private void ChangeJellyPanelInfo()
    {
        // 잠금 판넬
        if(GameManager.Instance.jellyUnlockInfo[curPageNum] == false) {
            LockGroup.SetActive(true);

            LockjellyImage.sprite = GameManager.Instance.jellySpriteList[curPageNum];
            LockjellyCost.text = string.Format("{0:#,#}", GameManager.Instance.jelatineList[curPageNum]);
            jellyPage.text = string.Format("#{0:00}", (curPageNum + 1));

            return;
        }

        // 해금 판넬
        LockGroup.SetActive(false);

        UnlockjellyImage.sprite = GameManager.Instance.jellySpriteList[curPageNum];
        UnlockjellyImage.SetNativeSize();
        UnlockjellyName.text = GameManager.Instance.jellyNameList[curPageNum];;
        UnlockjellyCost.text = string.Format("{0:#,#}", GameManager.Instance.priceList[curPageNum]);

        jellyPage.text = string.Format("#{0:00}", (curPageNum + 1));
    }


    public void UnlockJelly()
    {
        if(GameManager.Instance.Jelatine < GameManager.Instance.jelatineList[curPageNum]) {
            SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Fail);
            return;
        }

        SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Unlock);

        GameManager.Instance.Jelatine -= GameManager.Instance.jelatineList[curPageNum];
        GameManager.Instance.jellyUnlockInfo[curPageNum] = true;

        ChangeJellyPanelInfo();
    }

    public void BuyJelly()
    {
        if(GameManager.Instance.Gold < GameManager.Instance.priceList[curPageNum]) {
            SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Fail);
            return;
        }

        if(GameManager.Instance.curJellyNum >= GameManager.Instance.numGroupJelly) {
            SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Fail);
            return;
        }

        SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Buy);

        GameManager.Instance.Gold -= GameManager.Instance.priceList[curPageNum];
        GameManager.Instance.CreateJelly(curPageNum);
    }

    
    public void AddJellyAccept()
    {
        if(GameManager.Instance.Gold < GameManager.Instance.jellyAcceptPrice) {
            SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Fail);
            return;
        }

        SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Unlock);

        GameManager.Instance.Gold -= GameManager.Instance.jellyAcceptPrice;
        GameManager.Instance.SetNumGroupJellyText();

    }

    public void AddJellyClick()
    {
        if(GameManager.Instance.Gold < GameManager.Instance.jellyClickPrice) {
            SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Fail);
            return;
        }

        SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Unlock);

        GameManager.Instance.Gold -= GameManager.Instance.jellyClickPrice;
        GameManager.Instance.SetClickJellyText();

    }
    
}
