using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance => _instance;
    
    [SerializeField] private List<RecipeSO> _menu;
    
    private static DeliveryManager _instance;
    private List<RecipeSO> _orders = new List<RecipeSO>();
    private float _spawnRecipeTimer;
    private const float SpawnRecipeTimerMax = 4f;
    private const int OrdersMax = 5;

    private void Awake()
    {
        _instance = this;
        _spawnRecipeTimer = SpawnRecipeTimerMax;
    }

    private void Update()
    {
        _spawnRecipeTimer += Time.deltaTime;
        if (_spawnRecipeTimer >= SpawnRecipeTimerMax)
        {
            _spawnRecipeTimer = 0f;
            if (_orders.Count < OrdersMax)
            {
                var order = _menu[Random.Range(0, _menu.Count)];
                Debug.Log(order.recipeName);
                _orders.Add(order);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        foreach (var order in _orders)
        {
            if (order.ingredients.Count == plateKitchenObject.KitchenObjectSos.Count)
            {
                bool plateMatchesOrder = true;
                foreach (var ingredient in order.ingredients)
                {
                    bool ingredientFound = false;
                    foreach (var kitchenObjectSo in plateKitchenObject.KitchenObjectSos)
                    {
                        if (kitchenObjectSo == ingredient)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound)
                    {
                        plateMatchesOrder = false;
                        break;
                    }
                }

                if (plateMatchesOrder)
                {
                    Debug.Log("Correct Recipe!");
                    _orders.Remove(order);
                    return;
                }
            }
        }
        Debug.Log("Wrong Recipe");
    }
}