using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingUIItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image correctIcon;
    [Header("Sound")] [SerializeField] private AudioClip[] mouseHoverSounds;
    [SerializeField] private AudioClip[] mouseClickSounds;
    public CraftingRecipeSO CraftingRecipe { get; private set; }

    public void SetData(CraftingRecipeSO recipe)
    {
        itemImage.sprite = recipe.Result.item.ItemImage;
        nameText.text = recipe.Result.item.name;
        CraftingRecipe = recipe;
    }
    
    public void SetCorrectIcon(bool isCorrect)
    {
        correctIcon.gameObject.SetActive(isCorrect);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance.RandomPlaySound(mouseClickSounds);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.RandomPlaySound(mouseHoverSounds);
    }
}
