using System;
using Unity.Netcode;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;
    public event EventHandler OnCut;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    
    
    [SerializeField] private CuttingRecipeSO[] _cuttingRecipes;

    private int _cuttingProcess;

    public new static void ResetStaticData()
    {
        OnAnyCut = null;
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
                    InteractLogicPlaceObjectServerRpc();

                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            else
            {
                if (player.GetKitchenObject().TryGetPlate(out var plateKitchenObject))
                {
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().KitcheObjectSO))
                        GetKitchenObject().DestroySelf();
                }
            }
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicPlaceObjectServerRpc()
    {
        InteractLogicPlaceObjectClientRpc();
    }
    
    [ClientRpc]
    private void InteractLogicPlaceObjectClientRpc()
    {

        _cuttingProcess = 0;

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = 0f
        });
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().KitcheObjectSO))
        {
            CutObjectServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void CutObjectServerRpc()
    {
        CutObjectClientRpc();
        TestCuttingProcessDoneServerRpc();
    }
    
    [ClientRpc]
    private void CutObjectClientRpc()
    {
        var cuttingRecipe = GetCuttingRecipeWithInput(GetKitchenObject().KitcheObjectSO);
            
        _cuttingProcess++;
        OnCut?.Invoke(this, EventArgs.Empty);
        OnAnyCut?.Invoke(this, EventArgs.Empty);
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = (float)_cuttingProcess / cuttingRecipe.cuttingProgressMax
        });
            

    }

    [ServerRpc]
    private void TestCuttingProcessDoneServerRpc()
    {
        var cuttingRecipe = GetCuttingRecipeWithInput(GetKitchenObject().KitcheObjectSO);
        if (_cuttingProcess >= cuttingRecipe.cuttingProgressMax)
        {
            var output = GetOutputForInput(GetKitchenObject().KitcheObjectSO);
            KitchenObject.DestroyKitchenObject(GetKitchenObject());
            KitchenObject.SpawnKitchenObject(output, this);
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
