using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance => _instance;
    private static OptionsUI _instance;
    
    
    [SerializeField] private Button _sfxButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _closeButton;

    private TextMeshProUGUI _sfxText;
    private TextMeshProUGUI _musicText;

    private void Awake()
    {
        _instance = this;
        _sfxText = _sfxButton.GetComponentInChildren<TextMeshProUGUI>();
        _musicText = _musicButton.GetComponentInChildren<TextMeshProUGUI>();
        _sfxButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisuals();
        });
        
        _musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisuals();
        });
        _closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
        
        Hide();
    }

    private void Start()
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        GameManager.Instance.OnGameUnpaused += HandleOnGameUnpaused;
        _sfxText.text = $"Sound Effects : {Mathf.Round(SoundManager.Instance.Volume * 10f)}";
        _musicText.text = $"Music : {Mathf.Round(MusicManager.Instance.Volume * 10f)}";
    }

    private void HandleOnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
