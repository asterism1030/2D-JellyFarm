using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo : MonoBehaviour
{
    // Singletone
    private static GameInfo instance = null;
    public static GameInfo Instance { get { return instance; } private set { instance = value; } }
    
    // data
    private int maxGold = 999999999;
    private int maxJelatine = 999999999;

    private int[] maxExp = { 10, 100, 110 };

    private Vector3[] pointList = {
            new Vector3(-3.0f, 1.0f, 0.0f),
            new Vector3(3.0f, -1.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 0.0f) };
    
    private Vector3 topLeft = new Vector3(-5.5f, 1.0f, 0.0f);
    private Vector3 bottomRight = new Vector3(5.5f, -2.0f, 0.0f);

    // text
    private string jellyAcceptStr = "젤리 수용량 ";
    private string jellyClickStr = "클릭 생산량 x ";


    // get set
    public int MaxGold  { get{ return maxGold; } private set{ maxGold = value; } }
    public int MaxJelatine  { get{ return maxJelatine; } private set{ maxJelatine = value; } }
    public int[] MaxExp { get { return maxExp; } private set { maxExp = value; } }
    public Vector3[] PointList { get { return pointList; } private set { pointList = value; } }
    public Vector3 TopLeft { get { return topLeft; } private set { topLeft = value; } }
    public Vector3 BottomRight { get { return bottomRight; } private set { bottomRight = value; } }
    public string JellyAcceptStr { get { return jellyAcceptStr; } private set { jellyAcceptStr = value; } }
    public string JellyClickStr { get { return jellyClickStr; } private set { jellyClickStr = value; } }


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
}
