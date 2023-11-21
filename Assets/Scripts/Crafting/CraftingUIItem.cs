using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUIItem : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image correctIcon;
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
}
