using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///
//  각 판넬에 대한 이벤트
///
public class PanelEvent : MonoBehaviour
{
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
        
    }

    // OnClick
    // TODO) 창 밖 눌렀을 때 창 닫기
    public void ShowHideEvent()
    {
        SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Button);

        if(isShow == false) {
            PanelShow();
        }
        else {
            PanelHide();
        }
    }

    void PanelShow()
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

    void PanelHide()
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
