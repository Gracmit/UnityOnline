using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _recipeNameText;
    [SerializeField] private GameObject _iconContainer;
    [SerializeField] private GameObject _iconTemplate;

    public void Awake()
    {
        _iconTemplate.SetActive(false);
    }

    public void SetRecipe(RecipeSO recipe)
    {
        _recipeNameText.text = recipe.recipeName;
        
        foreach (Transform child in _iconContainer.transform)
        {
            if(child.gameObject == _iconTemplate)
                continue;
            Destroy(child.gameObject);
        }

        foreach (var ingredient in recipe.ingredients)
        {
            var instance = Instantiate(_iconTemplate, _iconContainer.transform);
            instance.SetActive(true);
            instance.GetComponent<Image>().sprite = ingredient.sprite;
        }
    }
}
