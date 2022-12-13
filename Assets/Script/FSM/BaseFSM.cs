using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BaseFSM : MonoBehaviour
{
    protected Dictionary<string, Acting> idleActs = new Dictionary<string, Acting>();
    
    [Header("---------------------------------------------------    동작")]
    [SerializeField]
    private List<Acting> idleActingList;    // EntryState (특정 조건 만족시 동작)

    [SerializeField]
    protected List<Acting> anyActingList;   // AnyState (매번 동작)


    void Awake()
    {
        foreach(Acting act in idleActingList) {
            idleActs.Add(act.GetType().Name, act);
        }
    }

}
