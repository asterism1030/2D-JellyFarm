using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using EnumManager;

public class Jelly : MonoBehaviour
{
    Animator animator = null;

    string jellyName = "";
    int id = 0;     // 0 ~
    int level = 0;  // 0 ~
    int exp = 0;    // 0 ~
    int price = 0;


    // get set
    public int Price { get { return price; } set { price = value; } }
    public int Level { get { return level; } }
    public int Exp { get { return exp; } set { exp = value; } }
    public Animator Anim { get { return animator; } }


    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        if(animator == null) {
            Debug.LogError("animator is null");
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

    }


    void OnDestroy()
    {
        UserInfo.Instance.curJellyNum--;
        Destroy(gameObject);
    }


    // Etc Function
    public void GetJelatine()
    {
        UserInfo.Instance.Jelatine += (id + 1) * (level + 1) * (UserInfo.Instance.clickGroupJelly);

        Debug.Log(id);
        Debug.Log(level);
    }


    public void ChangeLev()
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
