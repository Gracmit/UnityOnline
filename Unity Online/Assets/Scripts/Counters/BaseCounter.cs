using System;
using Unity.Netcode;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlaced;
    [SerializeField] private Transform _counterTopPoint;
    
    private KitchenObject _kitchenObject;


    public static void ResetStaticData()
    {
        OnAnyObjectPlaced = null;
    }
    
    public virtual void Interact(Player player)
    {
        
    }
    
    public Transform GetKitchenObjectFollowTransform() => _counterTopPoint;

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnAnyObjectPlaced?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject() => _kitchenObject;

    public void ClearKitchenObject() => _kitchenObject = null;

    public bool HasKitchenObject() => _kitchenObject != null;
    public NetworkObject GetNetworkObject()
    {
        return null;
    }

    public virtual void InteractAlternate(Player player)
    {
        
    }
}
