using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

///
//  젤리와 관련된 이벤트
//  매매, 관련 재화, 정보 등
///
public class JellyEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SellEvent()
    {
        GameObject selectedJelly = GameManager.Instance.SelectedJelly;

        if(selectedJelly == null) {
            return;
        }

        SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Sell);

        UserInfo.Instance.Gold += (GameManager.Instance.SelectedJellyPrice * GameManager.Instance.SelectedJellyLev);
        Debug.Log("sell " + GameManager.Instance.SelectedJellyPrice * GameManager.Instance.SelectedJellyLev);

        Destroy(selectedJelly);
        GameManager.Instance.SelectedJellyPrice = 0;
        GameManager.Instance.SelectedJellyLev = 0;
       
    }

    public void BuyEvent(Text t)
    {
        // #00 형태의 텍스트가 입력됨
        string nStr = t.text;
        int num = int.Parse(nStr.Substring(1)) - 1;

        BuyEvent(num);
    }

    // overloading BuyEvent
    void BuyEvent(int num)  // num = index
    {
        if(UserInfo.Instance.Gold < GameManager.Instance.priceList[num]) {
            SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Fail);
            return;
        }

        if(UserInfo.Instance.curJellyNum >= UserInfo.Instance.numGroupJelly) {
            SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Fail);
            return;
        }

        SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Buy);

        UserInfo.Instance.Gold -= GameManager.Instance.priceList[num];
        GameManager.Instance.CreateJelly(num);
    }
    
    // 젤리 수용량
    public void AddAcceptEvent()
    {
        if(UserInfo.Instance.Gold < GameManager.Instance.jellyAcceptPrice) {
            SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Fail);
            return;
        }

        SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Unlock);

        UserInfo.Instance.Gold -= GameManager.Instance.jellyAcceptPrice;
        GameManager.Instance.SetNumGroupJellyText();

    }

    // 젤리 클릭 배율
    public void AddClickEvent()
    {
        if(UserInfo.Instance.Gold < GameManager.Instance.jellyClickPrice) {
            SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Fail);
            return;
        }

        SoundManager.Instance.SetAndPlaySfx(EnumManager.SfxState.Unlock);

        UserInfo.Instance.Gold -= GameManager.Instance.jellyClickPrice;
        GameManager.Instance.SetClickJellyText();

    }

}
