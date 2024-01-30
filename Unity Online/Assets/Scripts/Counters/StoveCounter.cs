using System;
using Unity.Netcode;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<OnStoveStateChangedEventArgs> OnStoveStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStoveStateChangedEventArgs : EventArgs
    {
        public bool stoveOn;
    }

    [SerializeField] private FryingRecipeSO[] _recipes;
    private NetworkVariable<float> _fryingTimer = new NetworkVariable<float>(0f);
    private bool _burned;
    private FryingRecipeSO _currentRecipe;

    public override void OnNetworkSpawn()
    {
        _fryingTimer.OnValueChanged += OnValueChanged;
        base.OnNetworkSpawn();
    }

    private void OnValueChanged(float previousValue, float newValue)
    {
        float fryingTimerMax = _currentRecipe != null ? _currentRecipe.timerMax : 1f;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
        {
            progressNormalized = _fryingTimer.Value / fryingTimerMax
        });
    }

    private void Update()
    {
        if (!IsServer) return;
        
        if (HasKitchenObject() && !_burned)
        {
            _fryingTimer.Value += Time.deltaTime;
            
            if (_fryingTimer.Value >= _currentRecipe.timerMax)
            {
                _fryingTimer.Value = 0f;
                KitchenGameMultiplayer.Instance.DestroyKitchenObject(GetKitchenObject());
                KitchenObject.SpawnKitchenObject(_currentRecipe.output, this);
                var kitchenObject = GetKitchenObject();
                SetCurrentRecipeServerRpc();
                if (!HasRecipeWithInput(kitchenObject.KitcheObjectSO))
                {
                    _burned = true;
                }
            }
        }
    }

    [ServerRpc]
    private void SetCurrentRecipeServerRpc()
    {
        SetCurrentRecipeClientRpc();
    }
    
    [ClientRpc]
    private void SetCurrentRecipeClientRpc()
    {
        _currentRecipe = GetFryingRecipeWithInput(GetKitchenObject().KitcheObjectSO);
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().KitcheObjectSO))
                {
                    var kitchenObject = player.GetKitchenObject();
                    kitchenObject.SetKitchenObjectParent(this);
                    InteractLogicPlaceObjectOnCounterServerRpc();
                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                var kitchenObject = GetKitchenObject();
                kitchenObject.SetKitchenObjectParent(player);
                InteractLogicTakeObjectFromCounterServerRpc();
            }
            else
            {
                if (player.GetKitchenObject().TryGetPlate(out var plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().KitcheObjectSO))
                    {
                        KitchenGameMultiplayer.Instance.DestroyKitchenObject(GetKitchenObject());
                        InteractLogicTakeObjectFromCounterWithPlateServerRpc();
                    }
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicPlaceObjectOnCounterServerRpc()
    {
        InteractLogicPlaceObjectOnCounterClientRpc();
    }
    
    [ClientRpc]
    private void InteractLogicPlaceObjectOnCounterClientRpc()
    {
        _currentRecipe = GetFryingRecipeWithInput(GetKitchenObject().KitcheObjectSO);
        OnStoveStateChanged?.Invoke(this, new OnStoveStateChangedEventArgs()
        {
            stoveOn = true
        });
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicTakeObjectFromCounterWithPlateServerRpc()
    {
        _fryingTimer.Value = 0;
        InteractLogicTakeObjectFromCounterWithPlateClientRpc();
    }
    
    
    [ClientRpc]
    private void InteractLogicTakeObjectFromCounterWithPlateClientRpc()
    {
        OnStoveStateChanged?.Invoke(this, new OnStoveStateChangedEventArgs()
        {
            stoveOn = false
        });
        _burned = false;
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicTakeObjectFromCounterServerRpc()
    {
        _fryingTimer.Value = 0;
        InteractLogicTakeObjectFromCounterClientRpc();
    }
    
    
    [ClientRpc]
    private void InteractLogicTakeObjectFromCounterClientRpc()
    {
        _burned = false;
        OnStoveStateChanged?.Invoke(this, new OnStoveStateChangedEventArgs()
        {
            stoveOn = false
        });
    }

    private bool HasRecipeWithInput(KitchenObjectSO input) => GetFryingRecipeWithInput(input) != null;

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO input) => GetFryingRecipeWithInput(input)?.output;

    private FryingRecipeSO GetFryingRecipeWithInput(KitchenObjectSO input)
    {
        foreach (var recipe in _recipes)
        {
            if (recipe.input == input)
            {
                return recipe;
            }
        }

        return null;
    }
}