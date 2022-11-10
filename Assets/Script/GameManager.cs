using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Globalization;

public class GameManager : MonoBehaviour
{
    // Singletone
    private static GameManager instance = null;
    public static GameManager Instance { get { return instance; } private set { instance = value; } }


    // Valiables
    public bool IsAnyWindowOpend = false;

    // [SerializeField]
    // private Camera camera;
    private GameObject selectedJelly;

    private int maxNum = 999999999;
    private int[] maxExp = { 1000, 10000, 10000 };

    [SerializeField]
    private RuntimeAnimatorController[] levelAc;

    private Vector3[] pointList = {
            new Vector3(-3.0f, 1.0f, 0.0f),
            new Vector3(3.0f, -1.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 0.0f)
        };

    private Vector3 topLeft = new Vector3(-5.5f, 1.0f, 0.0f);
    private Vector3 bottomRight = new Vector3(5.5f, -2.0f, 0.0f);

    // get set
    public GameObject SelectedJelly { get { return selectedJelly; } set { selectedJelly = value; } }
    public Vector3[] PointList { get { return pointList; } private set { pointList = value; } }
    public Vector3 TopLeft { get { return topLeft; } private set { topLeft = value; } }
    public Vector3 BottomRight { get { return bottomRight; } private set { bottomRight = value; } }
    public int[] MaxExp { get { return maxExp; } private set { maxExp = value; } }
    public RuntimeAnimatorController[] LevelAc { get { return levelAc; } private set { levelAc = value; } }

    
    /*
    임시
    - 저장되는 데이터
    - 젤리 오브젝트 별 ID와 Level (파싱 구현해야 될듯)
    */
    private int jelatine = 100;
    private int gold = 220;

    private Text jelatineTxt;
    private Text goldTxt;

    public string[] jellyList; // index = id
    public string[] jellyLev; // index = lev

    public int[] priceList;

    public int Jelatine { get { return jelatine; } set { jelatine = value; if(jelatine > maxNum) jelatine = maxNum; } }
    public int Gold { get { return gold; } set { gold = value; if(gold > maxNum) gold = maxNum; } }


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
        jelatineTxt.text = string.Format("{0:#,#}", (int)(Mathf.SmoothStep(float.Parse(jelatineTxt.text), jelatine, 0.5f)));
        goldTxt.text = string.Format("{0:N0}", (int)(Mathf.SmoothStep(float.Parse(goldTxt.text), gold, 0.5f)));

        if((int)float.Parse(jelatineTxt.text) > maxNum) {
            jelatineTxt.text = string.Format("{0:#,#}", maxNum);
        }
        if((int)float.Parse(goldTxt.text) > maxNum) {
            goldTxt.text = string.Format("{0:N0}", maxNum);
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

    
}