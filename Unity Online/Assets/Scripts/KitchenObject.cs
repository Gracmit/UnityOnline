using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObject;

    private IKitchenObjectParent _kitchenObjectParent;

    public KitchenObjectSO KitcheObjectSO => _kitchenObject;

    public IKitchenObjectParent KitchenObjectParent => _kitchenObjectParent;

    public void SetKitchenObjectParent(IKitchenObjectParent parent)
    {
        if (_kitchenObjectParent != null)
        {
            _kitchenObjectParent.ClearKitchenObject();
        }
        _kitchenObjectParent = parent;
        //transform.parent = parent.GetKitchenObjectFollowTransform();
        //  transform.localPosition = Vector3.zero;
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
