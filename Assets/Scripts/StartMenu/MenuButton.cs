using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour , IPointerClickHandler , IPointerEnterHandler
{
    [SerializeField] private AudioClip mouseEnter;
    [SerializeField] private AudioClip mouseClick;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySound(mouseClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySound(mouseEnter);
    }
}
