using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singletone
    private static GameManager instance = null;
    public static GameManager Instance { get { return instance; } private set { instance = value; } }


    // Valiables
    private Vector3[] pointList = {
            new Vector3(-3.0f, 1.0f, 0.0f),
            new Vector3(3.0f, -1.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 0.0f)
        };

    private Vector3 topLeft = new Vector3(-5.5f, 1.0f, 0.0f);
    private Vector3 bottomRight = new Vector3(5.5f, -2.0f, 0.0f);

    // get set
    public Vector3[] PointList { get { return pointList; } private set { pointList = value; } }
    public Vector3 TopLeft { get { return topLeft; } private set { topLeft = value; } }
    public Vector3 BottomRight { get { return bottomRight; } private set { bottomRight = value; } }
    
    /*
    임시
    - 저장되는 데이터
    */
    private int jelatine = 100;
    private int gold = 200;

    // [SerializeField]
    private Text jelatineTxt;

    // [SerializeField]
    private Text goldTxt;


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
        // -1 해야만 올바른 값 나옴 이유 모름
        jelatineTxt.text = string.Format("{0:n0}", Mathf.SmoothStep(float.Parse(jelatineTxt.text), jelatine - 1, 0.5f));
        goldTxt.text = string.Format("{0:n0}", Mathf.SmoothStep(float.Parse(goldTxt.text), gold - 1, 0.5f));
    }
}