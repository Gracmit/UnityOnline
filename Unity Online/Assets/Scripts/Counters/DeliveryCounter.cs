using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    private static DeliveryCounter _instance;
    
    public static DeliveryCounter Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            if (player.GetKitchenObject().TryGetPlate(out var plate))
            {
                DeliveryManager.Instance.DeliverRecipe(plate);

                player.GetKitchenObject().DestroySelf();
            }
        }
    }
}