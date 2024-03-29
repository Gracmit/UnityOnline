using System;
using System.Collections.Generic;
using Unity.Netcode;
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
        AddIngredientServerRpc(KitchenGameMultiplayer.Instance.GetKitchenObjectSoIndex(kitchenObjectSo));

        return true;
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddIngredientServerRpc(int kitchenObjectSOIndex)
    {
        AddIngredientClientRpc(kitchenObjectSOIndex);
    }
    
    [ClientRpc]
    private void AddIngredientClientRpc(int kitchenObjectSOIndex)
    {
        var kitchenObjectSo = KitchenGameMultiplayer.Instance.GetKitchenObjectSoFromIndex(kitchenObjectSOIndex);
        _kitchenObjectSos.Add(kitchenObjectSo);
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs()
        {
            kitchenObjectSO = kitchenObjectSo
        });
    }
}
