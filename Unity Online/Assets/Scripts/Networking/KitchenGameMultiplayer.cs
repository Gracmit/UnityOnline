using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class KitchenGameMultiplayer : NetworkBehaviour
{
    public static KitchenGameMultiplayer Instance => _instance;

    [SerializeField] private List<KitchenObjectSO> _kitchenObjectSos;

    private static KitchenGameMultiplayer _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
    
    
    public void SpawnKitchenObject(KitchenObjectSO kitchenObjectSo, IKitchenObjectParent kitchenObjectParent)
    {
        SpawnKitchenObjectServerRpc(_kitchenObjectSos.IndexOf(kitchenObjectSo), kitchenObjectParent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnKitchenObjectServerRpc(int kitchenObjectSoIndex, NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        var kitchenObjectSo = _kitchenObjectSos[kitchenObjectSoIndex];
        var kitchenObjectInstance = Instantiate(kitchenObjectSo.prefab);
        var networkObject = kitchenObjectInstance.GetComponent<NetworkObject>();
        networkObject.Spawn(true);
        var kitchenObject = kitchenObjectInstance.GetComponent<KitchenObject>();
        kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        var kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
    }

    public void DestroyKitchenObject(KitchenObject kitchenObject)
    { 
        DestroyKitchenObjectServerRpc(kitchenObject.NetworkObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyKitchenObjectServerRpc(NetworkObjectReference kitchenObjectReference)
    {
        kitchenObjectReference.TryGet(out NetworkObject kitchenNetworkObject);
        var kitchenObject = kitchenNetworkObject.GetComponent<KitchenObject>();
        
        ClearKitchenObjectParentClientRpc(kitchenObjectReference);
        
        kitchenObject.DestroySelf();
    }

    [ClientRpc]
    private void ClearKitchenObjectParentClientRpc(NetworkObjectReference kitchenObjectReference)
    {
        kitchenObjectReference.TryGet(out NetworkObject kitchenNetworkObject);
        var kitchenObject = kitchenNetworkObject.GetComponent<KitchenObject>();
        
        kitchenObject.ClearKitchenObjectParent();
    }
}
