using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject _plateKitchenObject;
    [SerializeField] private GameObject _iconTemplate;

    private void Awake()
    {
        _iconTemplate.SetActive(false);
    }

    private void Start()
    {
        _plateKitchenObject.OnIngredientAdded += PlateKitchenObjectOnOnIngredientAdded;
    }

    private void PlateKitchenObjectOnOnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        foreach (Transform child in transform)
        {
            if(child.gameObject == _iconTemplate)
                continue;
            Destroy(child.gameObject);
        }
        foreach (var kitchenObjectSo in _plateKitchenObject.KitchenObjectSos)
        {
            var icon = Instantiate(_iconTemplate, transform);
            icon.SetActive(true);
            icon.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(kitchenObjectSo);
        }
    }
}
