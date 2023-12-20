using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;
    
    [SerializeField] private KitchenObjectSO _kichenObjectSO;

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
            return;
        KitchenObject.SpawnKitchenObject(_kichenObjectSO, player);
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }
}


