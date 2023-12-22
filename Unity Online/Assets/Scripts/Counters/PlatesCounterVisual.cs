using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter _platesCounter;
    [SerializeField] private Transform _counterTopPoint;
    [SerializeField] private GameObject _plateVisualPrefab;

    private List<GameObject> _plates;

    private void Awake()
    {
        _plates = new List<GameObject>();
    }

    private void Start()
    {
        _platesCounter.OnPlateSpawned += PlatesCounterOnOnPlateSpawned;
        _platesCounter.OnPlateRemoved += PlatesCounterOnOnPlateRemoved;
    }

    private void PlatesCounterOnOnPlateRemoved(object sender, EventArgs e)
    {
        var plate = _plates[^1];
        _plates.Remove(plate);
        Destroy(plate);
    }

    private void PlatesCounterOnOnPlateSpawned(object sender, EventArgs e)
    {
        var plateVisual = Instantiate(_plateVisualPrefab, _counterTopPoint);
        float plateOffsetY = .1f;

        plateVisual.transform.localPosition = new Vector3(0, plateOffsetY * _plates.Count, 0);
        _plates.Add(plateVisual);
    }
}
