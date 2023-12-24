using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance => _instance;
    private static MusicManager _instance;
    
    private float _volume = .3f;
    private AudioSource _audioSource;

    public float Volume => _volume;

    private void Awake()
    {
        _instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    public void ChangeVolume()
    {
        _volume += .1f;
        if (_volume > 1f)
        {
            _volume = 0f;
        }

        _audioSource.volume = _volume;
    }
}
