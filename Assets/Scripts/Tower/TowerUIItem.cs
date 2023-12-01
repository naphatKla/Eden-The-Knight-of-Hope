using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUIItem : MonoBehaviour
{
    [SerializeField] private Image towerImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image correctIcon;
    public TowerSO TowerRecipe { get; private set; }

    public void SetData(TowerSO recipe)
    {
        towerImage.sprite = recipe.TowerImage;
        nameText.text = recipe.towerName;
        TowerRecipe = recipe;
    }
    
    public void SetCorrectIcon(bool isCorrect)
    {
        correctIcon.gameObject.SetActive(isCorrect);
    }
}