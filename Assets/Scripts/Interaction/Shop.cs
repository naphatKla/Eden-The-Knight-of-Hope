using Interaction;
using Inventory;
using UnityEngine;

public class Shop : InteractableObject
{
    [SerializeField] private PlayerUIInventoryPage playerUIInventoryPage;
    [SerializeField] private LayerMask playerLayer;
    private float _lastOpenTime;
    
    protected void Update()
    {
        if(_lastOpenTime + 1.5f > Time.time) return;
        if (Physics2D.OverlapBoxNonAlloc(transform.position, new Vector2(10, 10), 0, new Collider2D[1], playerLayer) ==
            0)
        {
            if(playerUIInventoryPage.isSellPageOn)
                playerUIInventoryPage.Hide();
        }
    }

    protected override void InteractAction()
    {
        playerUIInventoryPage.ShowSellButton();
        playerUIInventoryPage.Show();
        _lastOpenTime = Time.time;
    }
}
