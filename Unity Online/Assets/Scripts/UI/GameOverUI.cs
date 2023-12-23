using System;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _recipeDeliveredText;

    private void Start()
    {
        GameManager.Instance.OnStateChanged += HandleOnStateChanged;
        Hide();
    }

    private void HandleOnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            _recipeDeliveredText.text = DeliveryManager.Instance.OrdersDone.ToString();
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show() => gameObject.SetActive(true);

    private void Hide() => gameObject.SetActive(false);
}