using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUIItem : MonoBehaviour
{
    [SerializeField] private Image towerImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image correctIcon;
    [SerializeField] private Image closeButton;
    public TowerSO TowerRecipe { get; private set; }

    public void SetData(TowerSO recipe)
    {
        towerImage.sprite = recipe.towerImage;
        nameText.text = recipe.towerName;
        TowerRecipe = recipe;
    }
    
    public void SetCorrectIcon(bool isCorrect)
    {
        correctIcon.gameObject.SetActive(isCorrect);
    }
    
    public void CloseButton()
    {
        closeButton.gameObject.SetActive(true);
        GetComponent<Button>().enabled = false;
    }
    
    public void OpenButton()
    {
        closeButton.gameObject.SetActive(false);
        GetComponent<Button>().enabled = true;
    }
}