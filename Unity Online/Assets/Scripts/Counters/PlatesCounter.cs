using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    
    [SerializeField] private KitchenObjectSO _plateKitchenObjectSo;
    
    private float _spawnPlateTimer;
    private float _spawnPlateTimerMax = 4f;
    private int _platesAmount;
    private int _platesAmountMax = 4;

    private void Update()
    {
        _spawnPlateTimer += Time.deltaTime;
        if (_spawnPlateTimer > _spawnPlateTimerMax)
        {
            _spawnPlateTimer = 0f;

            if (_platesAmount < _platesAmountMax)
            {
                _platesAmount++;
                
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
            return;
        
        if(_platesAmount > 0)
        {
            _platesAmount--;
            KitchenObject.SpawnKitchenObject(_plateKitchenObjectSo, player);
            OnPlateRemoved?.Invoke(this, EventArgs.Empty);
        }
    }
}
