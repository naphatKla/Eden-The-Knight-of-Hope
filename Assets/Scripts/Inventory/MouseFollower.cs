using UnityEngine;

namespace Inventory
{
    public class MouseFollower : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Camera mainCam;
        [SerializeField] private BaseUIInventoryItem item;

        public void Awake()
        {
            canvas = transform.parent.GetComponent<Canvas>();
            mainCam = Camera.main;
            item = GetComponentInChildren<BaseUIInventoryItem>();
        }

        private void Update()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform,
                Input.mousePosition, canvas.worldCamera, out Vector2 position);
            transform.position = canvas.transform.TransformPoint(position);
        }

        /// <summary>
        /// Set the data of the item.
        /// </summary>
        /// <param name="sprite">Item sprite.</param>
        /// <param name="quantity">Item amount.</param>
        public void SetData(Sprite sprite, int quantity)
        {
            item.SetData(sprite, quantity);
        }

        /// <summary>
        /// Toggle active of the mouse follower.
        /// </summary>
        /// <param name="val">Is active or not.</param>
        public void Toggle(bool val)
        {
            gameObject.SetActive(val);
        }
    }
}