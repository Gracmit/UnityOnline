using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> _validKitchenObjects;

    private List<KitchenObjectSO> _kitchenObjectSos = new List<KitchenObjectSO>();

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSo)
    {
        if (!_validKitchenObjects.Contains(kitchenObjectSo))
        {
            return false;
        }
        if (_kitchenObjectSos.Contains(kitchenObjectSo))
        {
            return false;
        }
        _kitchenObjectSos.Add(kitchenObjectSo);
        return true;
    }
}
