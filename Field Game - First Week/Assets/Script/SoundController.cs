using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    AudioSource audioSource = null;
    [SerializeField] AudioClip[] bgm = null;
    [SerializeField] float bgmFastSpeed = 2.0f;


    List<AudioSource> sfxSources = null;
    [SerializeField] AudioClip[] sfxSounds = null;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        sfxSources = new List<AudioSource>();
        foreach (var source in transform.GetChild(0).GetComponents<AudioSource>())
        {
            sfxSources.Add(source);
        }
    }
    public void PlayBGM(string soundName)
    {
        audioSource.pitch = 1.0f;
        audioSource.volume = 1.0f;
        for(int i = 0; i < bgm.Length; i++)
        {
            if(bgm[i].name == soundName)
            {
                audioSource.clip = bgm[i];
                switch (soundName)
                {
                    case "GameOver":
                    case "Success":
                        audioSource.loop = false;
                        break;
                }
                audioSource.Play();
                return;
            }
        }
        Debug.LogError("There's no BGM: " + soundName);
    }

    public void PlaySFX(string soundName)
    {
        AudioSource temp = null;
        for (int i = 0; i < sfxSounds.Length; i++)
        {
            if (soundName == sfxSounds[i].name)
            {
                for (int j = 0; j < sfxSources.Count; j++)
                {
                    if (!sfxSources[j].isPlaying)
                    {
                        temp = sfxSources[j];
                    }
                }
                if (temp == null)
                {
                    temp = transform.GetChild(0).gameObject.AddComponent<AudioSource>();
                    sfxSources.Add(temp);
                }
                temp.clip = sfxSounds[i];
                temp.loop = false;
                switch (soundName)
                {
                    case "Hit":
                        temp.pitch = 1.3f;
                        temp.volume = 0.5f;
                        break;
                    case "Waring":
                        temp.pitch = 2.0f;
                        temp.volume = 0.3f;
                        break;
                    default:
                        temp.pitch = 1f;
                        temp.volume = 1f;
                        break;

                }
                temp.Play();
                return;
            }
        }
        Debug.LogError("There isn't Effect Sound: " + soundName);
    }
    public void SetBgmSpeed(bool isFast)
    {
        if (isFast)
        {
            audioSource.pitch = bgmFastSpeed;
        }
        else
            audioSource.pitch = 1.0f;
    }
}
