using System.Collections.Generic;
using Interaction;
using UnityEngine;
using UnityEngine.UI;

public class CraftingTable : InteractableObject
{
    [Header("================= Crafting ================= ")] 
    [SerializeField] private Image craftingUI;
    [SerializeField] Button armorButton;
    [SerializeField] Button weaponButton;
    [SerializeField] Button materialButton;
    [Space]

    [SerializeField] private CraftingUIPage armorPage;
    [SerializeField] private List<CraftingItem> armorCraftingItems;
    [Space]

    [SerializeField] private CraftingUIPage weaponPage;
    [SerializeField] private List<CraftingItem> weaponCraftingItems;
    [Space]
  
    [SerializeField] private CraftingUIPage materialPage;
    [SerializeField] private List<CraftingItem> materialCraftingItems;
    private float _lastOpenTime;
    [SerializeField] private LayerMask playerLayer;
    
    protected override void Start()
    {
        base.Start();
        /*armorButton.onClick.AddListener(() => {armorPage.gameObject.SetActive(true); weaponPage.gameObject.SetActive(false); materialPage.gameObject.SetActive(false);});
        weaponButton.onClick.AddListener(() => {armorPage.gameObject.SetActive(false); weaponPage.gameObject.SetActive(true); materialPage.gameObject.SetActive(false);});
        materialButton.onClick.AddListener(() => {armorPage.gameObject.SetActive(false); weaponPage.gameObject.SetActive(false); materialPage.gameObject.SetActive(true);});*/
        armorPage.Initialize(armorCraftingItems);
        weaponPage.Initialize(weaponCraftingItems);
        materialPage.Initialize(materialCraftingItems);
    }
    
    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CloseCraftingUI();
        if(_lastOpenTime + 1.5f > Time.time) return;
        if (Physics2D.OverlapBoxNonAlloc(transform.position, new Vector2(10,10), 0,new Collider2D[1],playerLayer) == 0)
            CloseCraftingUI();
    }

    protected override void InteractAction()
    {
        OpenCraftingUI();
    }

    private void OpenCraftingUI()
    {
        if (UIManager.Instance.CheckIsAnyUIOpen()) return;
        craftingUI.gameObject.SetActive(true);
        armorPage.UpdatePage();
        weaponPage.UpdatePage();
        materialPage.UpdatePage();
        _lastOpenTime = Time.time;
    }

    private void CloseCraftingUI()
    {
        craftingUI.gameObject.SetActive(false);
    }
}
