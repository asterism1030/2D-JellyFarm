using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserInfo : MonoBehaviour
{
    // Singletone
    private static UserInfo instance = null;
    public static UserInfo Instance { get { return instance; } private set { instance = value; }}

    //User Info
    public int curJellyNum = 0;

    public int numGroupJelly = 2;
    public int clickGroupJelly = 1;

    private int jelatine = 200;
    private int gold = 10000;

    private bool[] jellyUnlockInfo = { true, true, false, false, false, false,
                                     false, false, false, false, false, false };

    // get set
    public int CurJellyNum { get { return curJellyNum; } set { curJellyNum = value; } }
    public int NumGroupJelly { get { return numGroupJelly; } set { numGroupJelly = value; } }
    public int ClickGroupJelly { get { return clickGroupJelly; } set { clickGroupJelly = value; } }
    public int Jelatine { get { return jelatine; } set { jelatine = value; if(jelatine > GameManager.Instance.MaxNum) jelatine = GameManager.Instance.MaxNum; } }
    public int Gold { get { return gold; } set { gold = value; if(gold > GameManager.Instance.MaxNum) gold = GameManager.Instance.MaxNum; } }

    public bool JellyUnlockInfo(int index) {
        return jellyUnlockInfo[index];
    }
    public void JellyUnlockInfo(int index, bool isLock) {
        if(index < jellyUnlockInfo.Length) jellyUnlockInfo[index] = isLock;
    }


    
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
