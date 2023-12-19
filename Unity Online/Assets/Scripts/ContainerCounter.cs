using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;
    
    [SerializeField] private KitchenObjectSO _kichenObjectSO;

    public override void Interact(Player player)
    {
        var kitchenObjectInstance = Instantiate(_kichenObjectSO.prefab);
        kitchenObjectInstance.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }
}


