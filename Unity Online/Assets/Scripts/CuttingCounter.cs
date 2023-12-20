using UnityEngine;
using UnityEngine.Serialization;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] _cuttingRecipes;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if(HasRecipeWithInput(player.GetKitchenObject().KitcheObjectSO))
                    player.GetKitchenObject().SetKitchenObjectParent(this);
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
            var output = GetOutputForInput(GetKitchenObject().KitcheObjectSO);
            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(output, this);
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO input)
    {
        foreach (var cuttingRecipe in _cuttingRecipes)
        {
            if (cuttingRecipe.input == input)
            {
                return true;
            }
        }

        return false;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO input)
    {
        foreach (var cuttingRecipe in _cuttingRecipes)
        {
            if (cuttingRecipe.input == input)
            {
                return cuttingRecipe.output;
            }
        }

        return null;
    }
}
