using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
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