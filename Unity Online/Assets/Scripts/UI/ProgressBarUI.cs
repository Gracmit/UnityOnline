using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject _hasProgressGameObject;
    [SerializeField] private Image _barImage;

    private IHasProgress _hasProgress;
    
    
    private void Start()
    {
        _hasProgress = _hasProgressGameObject.GetComponent<IHasProgress>();
        
        _hasProgress.OnProgressChanged += HandleProgressChanged;
        _barImage.fillAmount = 0;
        Hide();
    }

    private void HandleProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
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
