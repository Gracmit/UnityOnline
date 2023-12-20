using UnityEngine;

public class KitchenObject : MonoBehaviour
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
        transform.parent = parent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
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


    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSo, IKitchenObjectParent kitchenObjectParent)
    {
        var kitchenObjectInstance = Instantiate(kitchenObjectSo.prefab);
        var kitchenObject = kitchenObjectInstance.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        return kitchenObject;
    }
}
