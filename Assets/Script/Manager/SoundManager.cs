using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnumManager {
    public enum SfxState
    {
        Button,
        Buy,
        Clear,
        Fail,
        Grow,
        PauseIn,
        PauseOut,
        Sell,
        Touch,
        Unlock
    }
}

public class SoundManager : MonoBehaviour
{
    // Singletone
    private static SoundManager instance = null;
    public static SoundManager Instance { get { return instance; } private set { instance = value; } }


    [SerializeField]
    private AudioSource bgm;
    
    [SerializeField]
    private AudioSource sfx;

    [SerializeField]
    private AudioClip[] audioClips; // Index = SfxState


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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetAndPlaySfx(EnumManager.SfxState state)
    {
        sfx.clip = audioClips[(int)state];
        sfx.Play();
    }


}
