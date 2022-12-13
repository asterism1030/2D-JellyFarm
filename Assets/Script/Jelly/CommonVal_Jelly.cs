using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CommonVal_Jelly : CommonValues
{
    [HideInInspector]
    public bool isFreeze = false;

   [SerializeField]
    private GameObject target;

    Animator animator = null;
    Renderer render = null;
    Jelly jellyScript;

    public GameObject Target { get { return target; } }
    public Animator Anim { get { return animator; } }
    public Jelly JellySC { get { return jellyScript; } }
    public Renderer Render { get { return render; } }

    void OnEnable()
    {
        if(animator == null) {
            animator = target.GetComponent<Animator>();
        }
        if(render == null) {
            render = target.GetComponent<Renderer>();
        }
        if(jellyScript == null) {
            jellyScript = target.GetComponent<Jelly>();
        }
    }

}
