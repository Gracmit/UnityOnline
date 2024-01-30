using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO _kichenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            else
            {
                if (player.GetKitchenObject().TryGetPlate(out var plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().KitcheObjectSO))
                    {
                        var kitchenObject = GetKitchenObject();
                        KitchenGameMultiplayer.Instance.DestroyKitchenObject(kitchenObject);
                    }
                }
                else
                {
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().KitcheObjectSO))
                        {
                            var kitchenObject = GetKitchenObject();
                            KitchenGameMultiplayer.Instance.DestroyKitchenObject(kitchenObject);
                        }
                    }
                }
            }
        }
    }
}
