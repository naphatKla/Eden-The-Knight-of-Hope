using System.Collections;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource audioSource;
    
    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
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
        
        audioSource.clip = audioClip;
        audioSource.Play();
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
        
        audioSource.clip = audioClips[randomIndex];
        audioSource.Play();
    }
    public void StopMusic()
    {
        audioSource.Stop();
    }
    
    public void PauseMusic()
    {
        audioSource.Pause();
    }
    
    public void ResumeMusic()
    {
        audioSource.UnPause();
    }
    
    public void SetMusicVolume(float volume)
    {
        audioSource.volume = volume;
    }

    IEnumerator PlayWithFadeIn(AudioClip audioClip)
    {
        float timeCount = 3;
        if (audioSource.isPlaying)
        {
            while (timeCount > 0)
            {
                timeCount -= Time.deltaTime;
                audioSource.volume = timeCount / 3;
                yield return null;
            }
        }
        
        StopMusic();
        audioSource.clip = audioClip;
        audioSource.Play();
        timeCount = 0;
        while (timeCount < 3)
        {
            timeCount += Time.deltaTime;
            audioSource.volume = timeCount / 3;
            yield return null;
        }
    }
}
