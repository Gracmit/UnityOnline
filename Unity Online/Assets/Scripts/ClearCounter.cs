using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO _kichenObjectSO;
    [SerializeField] private Transform _counterTopPoint;

    private KitchenObject _kitchenObject;

    
    public void Interact(Player player)
    {
        if (_kitchenObject == null)
        {
            var kitchenObjectInstance = Instantiate(_kichenObjectSO.prefab, _counterTopPoint);
            kitchenObjectInstance.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
        }
        else
        {
            _kitchenObject.SetKitchenObjectParent(player);
        }
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return _counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) => _kitchenObject = kitchenObject;
    
    public KitchenObject GetKitchenObject() => _kitchenObject;

    public void ClearKitchenObject() => _kitchenObject = null;

    public bool HasKitchenObject() => _kitchenObject != null;
}
