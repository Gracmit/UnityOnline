using System;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countdownText;

    private void Start()
    {
        GameManager.Instance.OnStateChanged += HandleOnStateChanged;
        Hide();
    }

    private void Update()
    {
        _countdownText.text = Mathf.Ceil(GameManager.Instance.GetStateTimer()).ToString();
    }

    private void HandleOnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActive())
        {
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
