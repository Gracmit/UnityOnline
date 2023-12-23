using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeAdded;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManager Instance => _instance;
    
    [SerializeField] private List<RecipeSO> _menu;
    
    private const float SpawnRecipeTimerMax = 4f;
    private const int OrdersMax = 5;
    
    private static DeliveryManager _instance;
    private List<RecipeSO> _orders = new List<RecipeSO>();
    private float _spawnRecipeTimer;
    private int _ordersDone;

    public List<RecipeSO> Orders => _orders;

    public int OrdersDone => _ordersDone;

    private void Awake()
    {
        _instance = this;
        _spawnRecipeTimer = SpawnRecipeTimerMax;
        _ordersDone = 0;
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
                
                OnRecipeAdded?.Invoke(this, EventArgs.Empty);
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
                    _orders.Remove(order);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    _ordersDone++;
                    return;
                }
            }
        }
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }
    
}