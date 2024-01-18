using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }
    
    [SerializeField] private List<KitchenObjectSO> _validKitchenObjects;

    private List<KitchenObjectSO> _kitchenObjectSos = new List<KitchenObjectSO>();

    public List<KitchenObjectSO> KitchenObjectSos => _kitchenObjectSos;

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
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs()
        {
            kitchenObjectSO = kitchenObjectSo
        });
        return true;
    }
}
