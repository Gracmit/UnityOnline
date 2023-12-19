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
        if (_kitchenObjectParent.HasKitchenObject())
            return;
        
        _kitchenObjectParent.SetKitchenObject(this);
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return _kitchenObjectParent;
    }
}
