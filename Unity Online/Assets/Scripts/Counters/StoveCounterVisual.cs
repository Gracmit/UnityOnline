using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter _stoveCounter;
    [SerializeField] private GameObject _stove;
    [SerializeField] private GameObject _particles;

    private void Start()
    {
        _stoveCounter.OnStoveStateChanged += StoveCounterOnOnStoveStateChanged;
    }

    private void StoveCounterOnOnStoveStateChanged(object sender, StoveCounter.OnStoveStateChangedEventArgs e)
    {
        _stove.SetActive(e.stoveOn);
        _particles.SetActive(e.stoveOn);
    }
}
