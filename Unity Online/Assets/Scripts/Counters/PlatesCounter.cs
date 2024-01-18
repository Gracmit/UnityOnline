using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
        if(!IsServer) return;
        
        _spawnPlateTimer += Time.deltaTime;
        if (_spawnPlateTimer > _spawnPlateTimerMax)
        {
            _spawnPlateTimer = 0f;

            if (_platesAmount < _platesAmountMax)
            {
                SpawnPlateClientRpc();
            }
        }
    }

    [ClientRpc]
    private void SpawnPlateClientRpc()
    {
        _platesAmount++;
                
        OnPlateSpawned?.Invoke(this, EventArgs.Empty);
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
            return;
        
        if(_platesAmount > 0)
        {
            KitchenObject.SpawnKitchenObject(_plateKitchenObjectSo, player);
            InteractLogicServerRpc();
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }
    
    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        _platesAmount--;
        OnPlateRemoved?.Invoke(this, EventArgs.Empty);
    }
}
