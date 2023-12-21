using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSo;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject _plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> _kitchenObjectSoGameObjects;

    private void Start()
    {
        _plateKitchenObject.OnIngredientAdded += PlateKitchenObjectOnOnIngredientAdded;
        foreach (var kitchenObject in _kitchenObjectSoGameObjects)
        {
            kitchenObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObjectOnOnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (var kitchenObject in _kitchenObjectSoGameObjects)
        {
            if (kitchenObject.kitchenObjectSo == e.kitchenObjectSO)
            {
                kitchenObject.gameObject.SetActive(true);
            }
        }
    }
}