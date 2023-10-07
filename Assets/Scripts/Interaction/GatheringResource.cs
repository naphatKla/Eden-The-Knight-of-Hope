using UnityEngine;

namespace Interaction
{
    public class GatheringResource : InteractableObject
    {
        [SerializeField] private Sprite[] sprites;

        protected override void Start()
        {
            base.Start();
            GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
            gameObject.AddComponent<PolygonCollider2D>();
        }

        protected override void InteractHandler()
        {
            if(!Input.GetKeyDown(key)) return;
            base.InteractHandler();
        }
    }
}
