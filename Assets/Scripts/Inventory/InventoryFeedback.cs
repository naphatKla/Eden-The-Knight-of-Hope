using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryFeedback : MonoBehaviour
{
    [SerializeField] private List<Image> indicatorImages;
    [SerializeField] private Image[] itemImages;
    [SerializeField] private TextMeshProUGUI[] quantityTexts;
    public static InventoryFeedback Instance;

    private void Start()
    {
        Instance = this;
    }

    public void ShowFeedback(Sprite itemSprite, int quantity)
    {
        StartCoroutine(PlayFeedback(itemSprite, quantity));
    }

    private IEnumerator PlayFeedback(Sprite itemSprite, int quantity)
    {
        yield return new WaitUntil(() => indicatorImages.Any(image => !image.gameObject.activeSelf));
        for (int i = 0; true; i++)
        {
            yield return null;
            if (i >= indicatorImages.Count) i = 0;
            if(indicatorImages[i].gameObject.activeSelf) continue;
            indicatorImages[i].gameObject.SetActive(true); 
            itemImages[i].sprite = itemSprite;
            quantityTexts[i].text = quantity > 0 ? $"<color=green>+{quantity}</color>" : $"<color=red>{quantity}</color>";
            yield return new WaitForSeconds(3f);
            indicatorImages[i].gameObject.SetActive(false);
            yield break;
        }
    }
}
