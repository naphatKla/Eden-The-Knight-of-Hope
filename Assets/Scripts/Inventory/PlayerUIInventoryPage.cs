using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class PlayerUIInventoryPage : BaseUIInventoryPage
    {
        [SerializeField] private UIInventoryDescription itemDescription; //
        [SerializeField] private GameObject equipmentPanel; //
        [Header("Button")] [SerializeField] private Button equipmentButton; //
        [SerializeField] private Button descriptionButton; //

        public override void Awake()
        {
            mouseFollower.Toggle(false);
            itemDescription?.ResetDescription();
            equipmentButton?.onClick.AddListener(() => {itemDescription.gameObject.SetActive(false); equipmentPanel.SetActive(true);});
            descriptionButton?.onClick.AddListener(() => {itemDescription.gameObject.SetActive(true); equipmentPanel.SetActive(false);});
        }

        private void Update()
        {
            if(!Input.GetMouseButtonDown(1)) return;
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
            listOfUIItems[itemIndex].Select();
        }
        #endregion
    }
}