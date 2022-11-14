using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

using System;
using System.Globalization;

public class GameManager : MonoBehaviour
{
    // Singletone
    private static GameManager instance = null;
    public static GameManager Instance { get { return instance; } private set { instance = value; } }


    // Valiables
    public bool IsAnyWindowOpend = false;

    [SerializeField]
    private GameObject JellyToCp;

    // jelly
    private GameObject selectedJelly;
    private int selectedJellyPrice;
    private int selectedJellyLev;

    [SerializeField]
    private RuntimeAnimatorController[] levelAc;

    // get set
    public GameObject SelectedJelly { get { return selectedJelly; } set { selectedJelly = value; } }
    public int SelectedJellyPrice { get { return selectedJellyPrice; } set { selectedJellyPrice = value; } }
    public int SelectedJellyLev { get { return selectedJellyLev; } set { selectedJellyLev = value; } }

    public RuntimeAnimatorController[] LevelAc { get { return levelAc; } private set { levelAc = value; } }

    
    /*
    임시
    - 저장되는 데이터
    - 젤리 오브젝트 별 ID와 Level (파싱 구현해야 될듯)
    */

    public Sprite[] jellySpriteList;
    public string[] jellyNameList;

    private Text jelatineTxt;
    private Text goldTxt;

    public string[] jellyList; // index = id
    public string[] jellyLev; // index = lev

    public int[] priceList;
    public int[] jelatineList; // = { 100, 200, 1000, 2000, 3000, 5000,
                               //  10000, 12000, 13000, 14000, 15000, 100000 };

    // 원랜 배열 값이나 임시
    public int jellyAcceptPrice = 100;
    public int jellyClickPrice = 100;

    // functions
    void Awake()
    {
        if(Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        jelatineTxt = GameObject.Find("Canvas/LeftTxt/JelatineTxt").GetComponent<Text>();
        goldTxt = GameObject.Find("Canvas/RightTxt/GoldTxt").GetComponent<Text>();
    }

    void LateUpdate()
    {
        // 추후에 깔끔하게 정리해야할듯
        // 숫자가 맞아 떨어지지 않음 (220 + 500 = 720 이어야하는데 719 임)
        // {매개변수:자료형 소수점자릿수}, 사용자 지정 숫자 형식 문자열, {0:#,#} = {0:N0}
        jelatineTxt.text = string.Format("{0:#,#}", (int)(Mathf.SmoothStep(float.Parse(jelatineTxt.text), UserInfo.Instance.Jelatine, 0.5f)));
        goldTxt.text = string.Format("{0:N0}", (int)(Mathf.SmoothStep(float.Parse(goldTxt.text), UserInfo.Instance.Gold, 0.5f)));

        if((int)float.Parse(jelatineTxt.text) > GameInfo.Instance.MaxJelatine) {
            jelatineTxt.text = string.Format("{0:#,#}", GameInfo.Instance.MaxJelatine);
        }
        if((int)float.Parse(goldTxt.text) > GameInfo.Instance.MaxGold) {
            goldTxt.text = string.Format("{0:N0}", GameInfo.Instance.MaxGold);
        }
    }

    public int[] GetJellyIDLev(string jellyName, string acNum)
    {
        int[] info = new int[3];

        // id
        for(int i = 0; i < jellyList.Length; i++) {
            if(jellyName == jellyList[i]) {
                info[0] = i;
                break;
            }
        }

        // lev
        for(int i = 0; i < jellyLev.Length; i++) {
            if(acNum == jellyLev[i]) {
                info[1] = i;
                break;
            }
        }

        // price
        info[2] = priceList[info[0]];

        return info;
    }


    public void ChangeAc(ref Animator anim, int level)
    {
        anim.runtimeAnimatorController = levelAc[level];
    }

    public Vector3 GetWorldPoint()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    public void CreateJelly(int num)
    {
        if(JellyToCp == null) {
            Debug.LogError("JellyToCp is null");
            return;
        }

        GameObject newJelly = Instantiate(JellyToCp, GameInfo.Instance.PointList[Random.Range(0, 2)], Quaternion.identity) as GameObject;
        newJelly.name = "jelly " + num.ToString();
        (newJelly.GetComponent<Renderer>() as SpriteRenderer).sprite = jellySpriteList[num];

        newJelly.SetActive(true);
    }
    
}