using System;
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
    private float _fryingTimer;
    private bool _burned;

    private void Update()
    {
        if (HasKitchenObject() && !_burned)
        {
            _fryingTimer += Time.deltaTime;
            
 
            var recipe = GetFryingRecipeWithInput(GetKitchenObject().KitcheObjectSO);

            if (_fryingTimer >= recipe.timerMax)
            {
                _fryingTimer = 0f;
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(recipe.output, this);
                if (!HasRecipeWithInput(GetKitchenObject().KitcheObjectSO))
                {
                    _burned = true;
                }
            }
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
            {
                progressNormalized = _fryingTimer / recipe.timerMax
            });
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().KitcheObjectSO))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    OnStoveStateChanged?.Invoke(this, new OnStoveStateChangedEventArgs()
                    {
                        stoveOn = true
                    });
                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                _burned = false;
                OnStoveStateChanged?.Invoke(this, new OnStoveStateChangedEventArgs()
                {
                    stoveOn = false
                });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                {
                    progressNormalized = 0
                });
                _fryingTimer = 0;
            }
        }
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
