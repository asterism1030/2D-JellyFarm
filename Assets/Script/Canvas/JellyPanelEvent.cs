using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JellyPanelEvent : MonoBehaviour
{
    int curPageNum = 0; // 0 ~

    [Header("Locked")]
    public GameObject LockGroup;
    public Image LockjellyImage;
    public Text LockjellyCost;

    [Header("Unlocked")]
    public Image UnlockjellyImage;
    public Text UnlockjellyName;
    public Text UnlockjellyCost;

    public Text jellyPage;


    void Start()
    {
        RefreshPanel();
    }

    public void PageDown()
    {
        SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Button);

        if(curPageNum == 0) {
            return;
        }

        curPageNum--;
        RefreshPanel();
    }

    public void PageUp()
    {
        SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Button);

        if(curPageNum == 11) {
            return;
        }

        curPageNum++;
        RefreshPanel();
    }


    // TODO) 판넬 최초 띄울 시 적용이 안됨 (#01)
    private void RefreshPanel()
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


    public void UnlockJellyPanel()
    {
        if(GameManager.Instance.Jelatine < GameManager.Instance.jelatineList[curPageNum]) {
            SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Fail);
            return;
        }

        SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Unlock);

        GameManager.Instance.Jelatine -= GameManager.Instance.jelatineList[curPageNum];
        GameManager.Instance.jellyUnlockInfo[curPageNum] = true;

        RefreshPanel();
    }

}