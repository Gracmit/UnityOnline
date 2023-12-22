using System;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter _stoveCouner;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _stoveCouner.OnStoveStateChanged += HandleOnStoveStateChanged;
    }

    private void HandleOnStoveStateChanged(object sender, StoveCounter.OnStoveStateChangedEventArgs e)
    {
        if (e.stoveOn)
        {
            _audioSource.Play();
        }
        else
        {
            _audioSource.Pause();
        }
    }
}
