using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


///
//  Todo) 명확한 기능 ?
///
public class TextManager : MonoBehaviour
{
    // Singletone
    private static TextManager instance = null;
    public static TextManager Instance { get { return instance; } private set { instance = value; }}

    [SerializeField]
    private Text Txt;   // TextComponent

    [SerializeField]
    private string defaultStr;

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

    }


    public void ResetText(string newStr = null)
    {
        if(newStr == null) {
            Txt.text = defaultStr;
            return;
        }

        defaultStr = newStr;
        Txt.text = defaultStr;
    }

    // defaultStr + str + num    ex) 생산량 x 1
    public string SetText(int num, string str = null, bool useSpace = true)
    {
        string newStr = SetText(str, useSpace);
        newStr = SetText(num.ToString(), useSpace);

        /*
        if(useSpace == true) {
            if(str != null) {
                newStr = string.Format("{0 1}", newStr, str);
            }
            newStr = string.Format("{0 1}", newStr, num.ToString());
        }
        else if(useSpace == false) {
            if(str != null) {
                newStr = newStr + str;
            }
            newStr = newStr + num.ToString();
        }
        */

        Txt.text = newStr;
        
        return newStr;
    }

    public string SetText(string str = null, bool useSpace = true)
    {
        string newStr = defaultStr;

        if(useSpace == true) {
            if(str != null) {
                newStr = string.Format("{0 1}", newStr, str);
            }
        }
        else if(useSpace == false) {
            if(str != null) {
                newStr = newStr + str;
            }
        }

        Txt.text = newStr;

        return newStr;
    }




}
