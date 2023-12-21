using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public event EventHandler OnCut;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    
    
    [SerializeField] private CuttingRecipeSO[] _cuttingRecipes;

    private int _cuttingProcess;
    
    
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().KitcheObjectSO))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    _cuttingProcess = 0;

                    var cuttingRecipe = GetCuttingRecipeWithInput(GetKitchenObject().KitcheObjectSO);

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)_cuttingProcess / cuttingRecipe.cuttingProgressMax
                    });
                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().KitcheObjectSO))
        {
            var cuttingRecipe = GetCuttingRecipeWithInput(GetKitchenObject().KitcheObjectSO);
            
            _cuttingProcess++;
            OnCut?.Invoke(this, EventArgs.Empty);
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)_cuttingProcess / cuttingRecipe.cuttingProgressMax
            });
            
            if (_cuttingProcess >= cuttingRecipe.cuttingProgressMax)
            {
                var output = GetOutputForInput(GetKitchenObject().KitcheObjectSO);
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(output, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO input)
    {
        return GetCuttingRecipeWithInput(input) != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO input)
    {
        return GetCuttingRecipeWithInput(input)?.output;
    }

    private CuttingRecipeSO GetCuttingRecipeWithInput(KitchenObjectSO input)
    {
        
        foreach (var cuttingRecipe in _cuttingRecipes)
        {
            if (cuttingRecipe.input == input)
            {
                return cuttingRecipe;
            }
        }

        return null;
    }
}
