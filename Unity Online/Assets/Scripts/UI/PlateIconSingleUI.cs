using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateIconSingleUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    
    public void SetKitchenObjectSO(KitchenObjectSO kitchenObjectSo)
    {
        _image.sprite = kitchenObjectSo.sprite;
    }
}
