using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO _kichenObjectSO;

    public override void Interact(Player player)
    {
    }
}
