using System;
using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObject;

    public KitchenObjectSO KitcheObjectSO => _kitchenObject;
    public IKitchenObjectParent KitchenObjectParent => _kitchenObjectParent;

    private IKitchenObjectParent _kitchenObjectParent;
    private FollowTransform _followTransform;

    private void Awake()
    {
        _followTransform = GetComponent<FollowTransform>();
    }


    public void SetKitchenObjectParent(IKitchenObjectParent parent)
    {
        SetKitchenObjectServerRpc(parent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetKitchenObjectServerRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        SetKitchenObjectClientRpc(kitchenObjectParentNetworkObjectReference);
    }

    [ClientRpc]
    private void SetKitchenObjectClientRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        var parent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();
        
        if (_kitchenObjectParent != null)
        {
            _kitchenObjectParent.ClearKitchenObject();
        }
        _kitchenObjectParent = parent;
        _followTransform.SetTargetTransform(parent.GetKitchenObjectFollowTransform());
        if (_kitchenObjectParent.HasKitchenObject())
            return;
        
        _kitchenObjectParent.SetKitchenObject(this);
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return _kitchenObjectParent;
    }

    public void DestroySelf()
    {
        _kitchenObjectParent.ClearKitchenObject();
        
        Destroy(gameObject);
    }


    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSo, IKitchenObjectParent kitchenObjectParent)
    {
        KitchenGameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSo, kitchenObjectParent);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }

        plateKitchenObject = null;
        return false;
    }
}
