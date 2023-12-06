using System.Collections;
using System.Linq;
using Inventory;
using PlayerBehavior;
using UnityEngine;

namespace Interaction
{
    public class GatheringResource : InteractableObject
    {
        public GatheringResourceSO resourceData;
        private PolygonCollider2D _collider2D;
        private bool _isDestroying;
        
        public void SetData(GatheringResourceSO data)
        {
            resourceData = data;
            countdownTime = resourceData.gatheringTime;
            GetComponent<SpriteRenderer>().sprite = resourceData.sprite[Random.Range(0, resourceData.sprite.Length)];
        }
        protected override void Start()
        {
            base.Start();
            _collider2D = gameObject.AddComponent<PolygonCollider2D>();
            prompt = $"<b>[ E ] {resourceData.name}";
            interactionTextUI.text = prompt;
            SpriteRenderer = GetComponent<SpriteRenderer>();
            if (resourceData)
                SetData(resourceData);
        }

        protected override void InteractHandler()
        {
            if (_isDestroying) return;
            base.InteractHandler();
        }

        public override void OnTarget(bool isTarget)
        {
            if (_isDestroying)
            {
                foreach (var interactionIndicator in interactionIndicators)
                        interactionIndicator.SetActive(false);
                return;
            }
            foreach (var interactionIndicator in interactionIndicators)
                interactionIndicator.SetActive(isTarget);
        
            if (!isTarget) return;
            InteractHandler();
        }

        protected override void InteractAction()
        {
            SoundManager.Instance.RandomPlaySound(resourceData.destroySounds);
            
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

            StartCoroutine(FadeAnimWhenDestroy());
        }

        protected override IEnumerator CountDownAndInteract(float time)
        {
            SoundManager.Instance.RandomPlaySound(resourceData.gatheringSounds);
            return base.CountDownAndInteract(time);
        }

        IEnumerator FadeAnimWhenDestroy()
        {
            PlayerInteractSystem.Instance.isStopMove = false;
            progressBar.gameObject.SetActive(false);
            _isDestroying = true;
            _collider2D.isTrigger = true;
            float timeCount = 0;
            while (timeCount < 0.5f)
            {
                Color color = SpriteRenderer.color;
                color.a = 1 - timeCount / 0.5f;
                SpriteRenderer.color = color;
                timeCount += Time.deltaTime;
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}
