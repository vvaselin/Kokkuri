using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public static BGMController instance;
    public AudioClip TitleBGM;
    public AudioClip BattleBGM;

    AudioSource bgm;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        bgm = GetComponent<AudioSource>();
    }

    public void Play()
    {
        bgm.loop = true;
        bgm.Play();
    }

    public void ChangeBGM(string bgm)
    {
        switch (bgm)
        {
            case "Title":
                this.bgm.clip = TitleBGM;
                break;
            case "Battle":
                this.bgm.clip = BattleBGM;
                break;
            default:
                Debug.LogError("Invalid BGM name");
                return;
        }
        this.bgm.Play();
    }
    public void Stop()
    {
        bgm.Stop();
    }
}
