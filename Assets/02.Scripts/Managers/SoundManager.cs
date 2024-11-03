using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum SoundType
{
    BGM,
    SFX
}
public class SoundManager : Singleton<SoundManager>
{
    public AudioListener audioListener;
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicMasterSlider;
    [SerializeField] private Slider musicBGMSlider;
    [SerializeField] private Slider musicSFXSlider;

    public List<AudioClip> sounds =  new List<AudioClip>();

    private void Start()
    {   
        musicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicBGMSlider.onValueChanged.AddListener(SetMusicVolume);
        musicSFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }


    public void PlaySound(string name, SoundType soundType)
    {
        AudioClip clipToPlay = sounds.Find(clip => clip.name == name);

        if (clipToPlay != null)  // 사운드를 찾았을 경우
        {
            if (soundType == SoundType.BGM)
            {
                bgmSource.clip = clipToPlay;
                bgmSource.Play();
            }
            else if (soundType == SoundType.SFX)
            {
                AudioSource tempSource = gameObject.AddComponent<AudioSource>();
                tempSource.clip = clipToPlay;
                tempSource.Play();

                Destroy(tempSource, clipToPlay.length);
            }
        }
    }

}
