using Inventory;
using UnityEngine;

namespace Interaction
{
    public class GatheringResource : InteractableObject
    {
        public GatheringResourceSO resourceData;
        
        public void SetData(GatheringResourceSO data)
        {
            resourceData = data;
            countdownTime = resourceData.gatheringTime;
            GetComponent<SpriteRenderer>().sprite = resourceData.sprite[Random.Range(0, resourceData.sprite.Length)];
        }
        protected override void Start()
        {
            base.Start();
            gameObject.AddComponent<PolygonCollider2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void InteractAction()
        {
            if (resourceData.point.y > 0)
                GameManager.Instance.AddPoint((int)Random.Range(resourceData.point.x, resourceData.point.y+1));
            
            if (resourceData.itemDrop1.Count > 0)
            {
                GatheringItemDrop gatheringItemDrop = ProjectExtensions.RandomPickOne(resourceData.itemDrop1).obj;
                int quantity = (int)Random.Range(gatheringItemDrop.quantityDrop.x, gatheringItemDrop.quantityDrop.y+1);
                PlayerInventoryController.Instance.InventoryData.AddItem(gatheringItemDrop.item, quantity);
            }
            
            if (resourceData.itemDrop2.Count > 0)
            {
                GatheringItemDrop gatheringItemDrop = ProjectExtensions.RandomPickOne(resourceData.itemDrop2).obj;
                int quantity = (int)Random.Range(gatheringItemDrop.quantityDrop.x, gatheringItemDrop.quantityDrop.y+1);
                PlayerInventoryController.Instance.InventoryData.AddItem(gatheringItemDrop.item, quantity);
            }
            
            if (resourceData.itemDrop3.Count > 0)
            {
                GatheringItemDrop gatheringItemDrop = ProjectExtensions.RandomPickOne(resourceData.itemDrop3).obj;
                int quantity = (int)Random.Range(gatheringItemDrop.quantityDrop.x, gatheringItemDrop.quantityDrop.y+1);
                PlayerInventoryController.Instance.InventoryData.AddItem(gatheringItemDrop.item, quantity);
            }
            
            if (resourceData.itemDrop4.Count > 0)
            {
                GatheringItemDrop gatheringItemDrop = ProjectExtensions.RandomPickOne(resourceData.itemDrop4).obj;
                int quantity = (int)Random.Range(gatheringItemDrop.quantityDrop.x, gatheringItemDrop.quantityDrop.y+1);
                PlayerInventoryController.Instance.InventoryData.AddItem(gatheringItemDrop.item, quantity);
            }
            
            if (resourceData.itemDrop5.Count > 0)
            {
                GatheringItemDrop gatheringItemDrop = ProjectExtensions.RandomPickOne(resourceData.itemDrop5).obj;
                int quantity = (int)Random.Range(gatheringItemDrop.quantityDrop.x, gatheringItemDrop.quantityDrop.y+1);
                PlayerInventoryController.Instance.InventoryData.AddItem(gatheringItemDrop.item, quantity);
            }

            base.InteractAction();
        }
    }
}
