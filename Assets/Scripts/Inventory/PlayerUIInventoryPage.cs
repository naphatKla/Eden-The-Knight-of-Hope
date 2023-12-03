using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class PlayerUIInventoryPage : BaseUIInventoryPage
    {
        [SerializeField] private UIInventoryDescription itemDescription; //
        [SerializeField] private GameObject equipmentPanel; //
        [Header("Button")] [SerializeField] private Button equipmentButton; //
        [SerializeField] private Button descriptionButton; //
        [SerializeField] private TextMeshProUGUI descriptionText;
        public Image sellZone;
        [SerializeField] private Button sellButton;
        [SerializeField] private Button sellAllButton;
        [SerializeField] private Image confirmSellPanel;
        [SerializeField] private TextMeshProUGUI confirmSellText;
        [SerializeField] private Button confirmSellButton;
        [SerializeField] private Button cancelSellButton;
        public TextMeshProUGUI totalPointText;
        [SerializeField] private TextMeshProUGUI costText;
        private BaseUIInventoryItem _selectedItem;
        public bool isSellPageOn;

        public override void Awake()
        {
            mouseFollower.Toggle(false);
            itemDescription?.ResetDescription();
            equipmentButton?.onClick.AddListener(() => {itemDescription.gameObject.SetActive(false); equipmentPanel.SetActive(true);});
            descriptionButton?.onClick.AddListener(() => {itemDescription.gameObject.SetActive(true); equipmentPanel.SetActive(false);});
            sellButton.onClick.AddListener(SellItem);
            sellAllButton.onClick.AddListener(SellAllItem);
            cancelSellButton.onClick.AddListener(()=>confirmSellPanel.gameObject.SetActive(false));
        }
        
        private void Update()
        {
            if(!Input.GetMouseButtonDown(1)) return;
            if(isSellPageOn) return;
            if (itemDescription.gameObject.activeSelf)
            {
                itemDescription.gameObject.SetActive(false); 
                equipmentPanel.SetActive(true);
            }
            else
            {
                itemDescription.gameObject.SetActive(true); 
                equipmentPanel.SetActive(false);
            }
        }

        #region Methods

        /// <summary>
        /// Reset the selection in the inventory UI page.
        /// </summary>
        public override void ResetSelection()
        {
            itemDescription.ResetDescription();
            DeselectAllItem();
        }
        
        /// <summary>
        /// Update the description in the inventory UI page.
        /// </summary>
        /// <param name="itemIndex">Item index target.</param>
        /// <param name="itemImage">Item sprite.</param>
        /// <param name="name">Item name.</param>
        /// <param name="description">Item description.</param>
        public void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            itemDescription.SetDescription(itemImage, name, description);
            DeselectAllItem();
            otherPages.ForEach(page => page.DeselectAllItem());
            listOfUIItems[itemIndex].Select();
        }

        protected override void HandleItemSelection(BaseUIInventoryItem inventoryItemBaseUI)
        {
            _selectedItem = inventoryItemBaseUI;
            costText.text = _selectedItem.ItemData? $"$ {_selectedItem.ItemData.Cost}" : $"$ 0";
            base.HandleItemSelection(inventoryItemBaseUI);
        }

        public override void Hide()
        {
            base.Hide();
            sellButton.gameObject.SetActive(false);
            sellAllButton.gameObject.SetActive(false);
            confirmSellPanel.gameObject.SetActive(false);
            equipmentButton.gameObject.SetActive(true);
            descriptionButton.gameObject.SetActive(true);
            costText.text = "$ 0";
            isSellPageOn = false;
        }
        
        public void ShowSellButton()
        {
            sellZone.gameObject.SetActive(true);
            sellButton.gameObject.SetActive(true);
            sellAllButton.gameObject.SetActive(true);
            equipmentPanel.SetActive(false);
            itemDescription.gameObject.SetActive(true);
            equipmentButton.gameObject.SetActive(false);
            descriptionButton.gameObject.SetActive(false);
            isSellPageOn = true;
        }
        
        private void SellItem()
        {
            if (!_selectedItem || !_selectedItem.ItemData)
            {
                descriptionText.text = "<color=red>Please select an item";
                return;
            }
            
            confirmSellPanel.gameObject.SetActive(true);
            confirmSellText.text = $"Are you sure to sell\n <color=yellow>x1 {_selectedItem.ItemData.name}</color> (${_selectedItem.ItemData.Cost} coins)";
            confirmSellButton.onClick.RemoveAllListeners();
            confirmSellButton.onClick.AddListener(()=> {
                GameManager.Instance.AddPoint(_selectedItem.ItemData.Cost);
                PlayerInventoryController.Instance.InventoryData.RemoveItem(_selectedItem.Index,1);
                if (_selectedItem.isEmpty)
                {
                    _selectedItem = null;
                    PlayerInventoryController.Instance.InventoryData.InformAboutChange();
                }
                confirmSellPanel.gameObject.SetActive(false);
            });
        }
        
        private void SellAllItem()
        {
            if (!_selectedItem || !_selectedItem.ItemData)
            {
                descriptionText.text = "<color=red>Please select an item";
                return;
            }
            
            confirmSellPanel.gameObject.SetActive(true);
            confirmSellText.text = $"Are you sure to sell\n <color=yellow>x{_selectedItem.Quantity} {_selectedItem.ItemData.name}</color> (${_selectedItem.ItemData.Cost*_selectedItem.Quantity} coins)";
            confirmSellButton.onClick.RemoveAllListeners();
            confirmSellButton.onClick.AddListener(() => {             
                GameManager.Instance.AddPoint(_selectedItem.ItemData.Cost * _selectedItem.Quantity);
                PlayerInventoryController.Instance.InventoryData.RemoveItem(_selectedItem.Index,_selectedItem.Quantity);
                if (_selectedItem.isEmpty)
                {
                    _selectedItem = null;
                    PlayerInventoryController.Instance.InventoryData.InformAboutChange();
                }
                confirmSellPanel.gameObject.SetActive(false);
            });
        }

        public override void ResetAllItems()
        {
            if (_selectedItem)
            {
                foreach (var item in listOfUIItems)
                    item.ResetData();
                return;
            }
     
            base.ResetAllItems();
            itemDescription.ResetDescription();
            costText.text = "$ 0";
        }

        #endregion
    }
}