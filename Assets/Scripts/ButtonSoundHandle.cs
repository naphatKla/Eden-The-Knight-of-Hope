using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundHandle : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField] private AudioClip[] mouseHoverSounds;
    [SerializeField] private AudioClip[] mouseClickSounds;
    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance.RandomPlaySound(mouseClickSounds);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.RandomPlaySound(mouseHoverSounds);
    }
}
