using System;
using UnityEngine;
using UnityEngine.Serialization;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter _baseCounter;
    [SerializeField] private GameObject[] _visualGameObjects;
    private void Start()
    {
        if(Player.LocalInstance != null) Player.LocalInstance.OnSelectedCounterChanged += HandleSelectedCounterChanged;
        else
        {
            Player.OnAnyPlayerSpawned += OnAnyPlayerSpawned;
        }
    }

    private void OnAnyPlayerSpawned(object sender, EventArgs e)
    {
        if (Player.LocalInstance != null)
        {
            Player.LocalInstance.OnSelectedCounterChanged -= HandleSelectedCounterChanged;
            Player.LocalInstance.OnSelectedCounterChanged += HandleSelectedCounterChanged;
        }
    }

    private void HandleSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if(e.selectedCounter == _baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (var counter in _visualGameObjects)
        {
            counter.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (var counter in _visualGameObjects)
        {
            counter.SetActive(false);
        }
    }
}
