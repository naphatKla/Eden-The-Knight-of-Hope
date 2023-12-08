using System;
using System.Collections.Generic;
using Interaction;
using UnityEngine;
using UnityEngine.UI;

public class CraftingTable : InteractableObject
{
    [Header("================= Crafting ================= ")] 
    [SerializeField] private Image craftingUI;
    [SerializeField] Button materialButton;
    [SerializeField] Button armorButton;
    [SerializeField] Button weaponButton;
    [SerializeField] Button potionButton;
    [SerializeField] Button closeButton;
    [Space]

    [SerializeField] private CraftingUIPage materialPage;
    [SerializeField] private List<CraftingItem> materialCraftingItems;
    [Space]
    
    [SerializeField] private CraftingUIPage armorPage;
    [SerializeField] private List<CraftingItem> armorCraftingItems;
    [Space]

    [SerializeField] private CraftingUIPage weaponPage;
    [SerializeField] private List<CraftingItem> weaponCraftingItems;
    [Space]
    
    [SerializeField] private CraftingUIPage potionPage;
    [SerializeField] private List<CraftingItem> potionItems;
    [Space]
    
    private float _lastOpenTime;
    [SerializeField] private LayerMask playerLayer;
    [Header("Sound")] [SerializeField] private AudioClip[] openCraftTableSounds;
    [SerializeField] private AudioClip[] closeCraftTableSounds;

    protected override void Start()
    {
        base.Start();
        materialButton.onClick.AddListener(() => {materialPage.ShowPage(true); armorPage.ShowPage(false); weaponPage.ShowPage(false); potionPage.ShowPage(false); materialPage.UpdatePage();});
        armorButton.onClick.AddListener(() => {materialPage.ShowPage(false); armorPage.ShowPage(true); weaponPage.ShowPage(false); potionPage.ShowPage(false); armorPage.UpdatePage();});
        weaponButton.onClick.AddListener(() => {materialPage.ShowPage(false); armorPage.ShowPage(false); weaponPage.ShowPage(true); potionPage.ShowPage(false); weaponPage.UpdatePage();});
        potionButton.onClick.AddListener(() => {materialPage.ShowPage(false); armorPage.ShowPage(false); weaponPage.ShowPage(false); potionPage.ShowPage(true); potionPage.UpdatePage();});

        closeButton.onClick.AddListener(CloseCraftingUI);
        materialPage.Initialize(materialCraftingItems);
        armorPage.Initialize(armorCraftingItems);
        weaponPage.Initialize(weaponCraftingItems);
        potionPage.Initialize(potionItems);
    }
    
    protected override void Update()
    {
        base.Update();
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
        materialPage.UpdatePage();
        armorPage.UpdatePage();
        weaponPage.UpdatePage();
        potionPage.UpdatePage();
        _lastOpenTime = Time.time;
        SoundManager.Instance.RandomPlaySound(openCraftTableSounds);
    }

    private void CloseCraftingUI()
    {
        if (craftingUI.gameObject.activeSelf == false) return;
        craftingUI.gameObject.SetActive(false);
        SoundManager.Instance.RandomPlaySound(closeCraftTableSounds);
    }
}
