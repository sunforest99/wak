using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMng : MonoBehaviour
{
    [SerializeField]
    AudioSource _audio;
    [SerializeField]
    AudioSource _effect;


    [SerializeField]
    AudioClip[] audioClip;
    [SerializeField]
    AudioClip[] effectClip;


    void Start()
    {
        
    }

}
