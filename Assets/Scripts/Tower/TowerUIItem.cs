using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUIItem : MonoBehaviour
{
    [SerializeField] private Image towerImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image correctIcon;
    [SerializeField] private Image currentIcon;
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
        currentIcon.gameObject.SetActive(false);
    }
    
    public void SetCurrentIcon(bool isCurrent)
    {
        currentIcon.gameObject.SetActive(isCurrent);
        correctIcon.gameObject.SetActive(false);
    }
    
    public void CloseButton()
    {
        closeButton.gameObject.SetActive(true);
        GetComponent<Button>().enabled = false;
        correctIcon.gameObject.SetActive(false);
        currentIcon.gameObject.SetActive(false);
    }
    
    public void OpenButton()
    {
        closeButton.gameObject.SetActive(false);
        GetComponent<Button>().enabled = true;
    }
}