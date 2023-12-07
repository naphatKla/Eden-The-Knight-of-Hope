using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public AudioSource musicSource;
    public static SoundManager Instance;
    
    private void Awake()
    {
        Instance = this;
        audioSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        audioSource.loop = true;
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
    
    public void RandomPlaySound(AudioClip[] audioClips)
    {
        if (audioClips.Length <= 0) return;
        int randomIndex = Random.Range(0, audioClips.Length);
        audioSource.PlayOneShot(audioClips[randomIndex]);
    }
    
    public void PlayBackGroundMusic(AudioClip audioClip, bool isFadeIn = false)
    {
        if (isFadeIn)
        {
            StartCoroutine(PlayWithFadeIn(audioClip));
            return;
        }
        
        musicSource.clip = audioClip;
        musicSource.Play();
    }
    
    public void RandomPlayBackGroundMusic(AudioClip[] audioClips, bool isFadeIn = false)
    {
        if (audioClips.Length <= 0) return;
        int randomIndex = Random.Range(0, audioClips.Length);
        
        if (isFadeIn)
        {
            StartCoroutine(PlayWithFadeIn(audioClips[randomIndex]));
            return;
        }
        
        musicSource.clip = audioClips[randomIndex];
        musicSource.Play();
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }
    
    public void PauseMusic()
    {
        musicSource.Pause();
    }
    
    public void ResumeMusic()
    {
        musicSource.UnPause();
    }
    
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    IEnumerator PlayWithFadeIn(AudioClip audioClip)
    {
        float timeCount = 3;
        if (musicSource.isPlaying)
        {
            while (timeCount > 0)
            {
                timeCount -= Time.deltaTime;
                musicSource.volume = timeCount / 3;
                yield return null;
            }
        }
        
        StopMusic();
        musicSource.clip = audioClip;
        musicSource.Play();
        timeCount = 0;
        while (timeCount < 3)
        {
            timeCount += Time.deltaTime;
            musicSource.volume = timeCount / 3;
            yield return null;
        }
    }
}
