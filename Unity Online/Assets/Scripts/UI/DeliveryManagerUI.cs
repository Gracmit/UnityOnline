using System;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] private GameObject _template;

    private void Awake()
    {
        _template.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeAdded += HandleOnRecipeAdded;
        
        DeliveryManager.Instance.OnRecipeCompleted += HandleOnRecipeCompleted;
        
        UpdateVisuals();
    }

    private void HandleOnRecipeCompleted(object sender, EventArgs e)
    {
        UpdateVisuals();
    }

    private void HandleOnRecipeAdded(object sender, EventArgs e)
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        foreach (Transform child in _container.transform)
        {
            if(child.gameObject == _template)
                continue;
            Destroy(child.gameObject);
        }
        foreach (var order in DeliveryManager.Instance.Orders)
        {
            var orderInstance = Instantiate(_template, _container.transform);
            orderInstance.SetActive(true);
            orderInstance.GetComponent<DeliveryManagerSingelUI>().SetRecipe(order);
        }
    }
}
