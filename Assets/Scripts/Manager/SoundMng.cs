using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EFFECT_ORDER {
    SOLDIER_SWING,
}

public class SoundMng : MonoBehaviour
{
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioSource _effect;

    public float _audioVolume = 1;
    public float _effectVolume = 1;

    // 대부분의 오디오 클립은 자기 자신이 가지고 있음.
    // UI 효과음이나 캐릭터 같이 모든 씬에 존재하는 특별한 것이 아니라면 여기에 포함시키지 말 것
    [SerializeField] AudioClip[] effectClip;


    private static SoundMng _instance = null;
    public static SoundMng I
    {
        get
        {
            if (_instance.Equals(null))
            {
                Debug.LogError("Instance is null");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
    }

    public void changeAudioVolume(float vol)
    {
        _audioVolume = vol;
        _audio.volume = vol;
    }
    public void changeEffectVolume(float vol)
    {
        _effectVolume = vol;
        _effect.volume = vol;
    }

    public void PlayAudio(AudioClip audioClip)
    {
        _audio.loop = true; //BGM 사운드이므로 루프설정
        _audio.volume = _audioVolume;
        _audio.clip = audioClip;
        _audio.Play();
    }
    public void PlayEffect(AudioClip effClip)
    {
        _effect.PlayOneShot(effClip, _effectVolume);
    }
}
