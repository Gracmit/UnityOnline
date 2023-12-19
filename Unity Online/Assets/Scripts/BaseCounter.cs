using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform _counterTopPoint;
    
    private KitchenObject _kitchenObject;

    
    public virtual void Interact(Player player)
    {
        
    }
    
    public Transform GetKitchenObjectFollowTransform() => _counterTopPoint;

    public void SetKitchenObject(KitchenObject kitchenObject) => _kitchenObject = kitchenObject;
    
    public KitchenObject GetKitchenObject() => _kitchenObject;

    public void ClearKitchenObject() => _kitchenObject = null;

    public bool HasKitchenObject() => _kitchenObject != null;
}
