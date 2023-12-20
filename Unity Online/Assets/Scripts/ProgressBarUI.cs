using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private CuttingCounter _cuttingCounter;
    [SerializeField] private Image _barImage;

    private void Start()
    {
        _cuttingCounter.OnProgressChanged += HandleProgressChanged;
        _barImage.fillAmount = 0;
        Hide();
    }

    private void HandleProgressChanged(object sender, CuttingCounter.OnProgressChangedEventArgs e)
    {
        _barImage.fillAmount = e.progressNormalized;

        if (e.progressNormalized is 0f or >= 1f)
            Hide();
        else
            Show();
    }

    private void Show() => gameObject.SetActive(true);
    
    private void Hide() => gameObject.SetActive(false);
}
