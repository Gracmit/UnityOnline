using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    
    [SerializeField] private AudioClipRefsSO _audiorefs;
    public static SoundManager Instance => _instance;
    
    private static SoundManager _instance;


    private float _volume = 1f;

    public float Volume => _volume;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeFailed += HandleOnRecipeFailed;
        DeliveryManager.Instance.OnRecipeSuccess += HandleOnRecipeSuccess;
        CuttingCounter.OnAnyCut  += HandleCuttingCounterOnOnAnyCut;
        //Player.Instance.OnPickedSomething += HandleOnPickedSomething;
        BaseCounter.OnAnyObjectPlaced += HandleOnAnyObjectPlaced;
        TrashCounter.OnAnyObjectTrashed += HandleOnAnyObjectTrashed;
    }

    private void HandleOnAnyObjectTrashed(object sender, EventArgs e)
    {
        var counter = sender as BaseCounter;
        PlaySound(_audiorefs.trash, counter.transform.position);
    }

    private void HandleOnAnyObjectPlaced(object sender, EventArgs e)
    {
        var counter = sender as BaseCounter;
        PlaySound(_audiorefs.objectDrop, counter.transform.position);
    }

    private void HandleOnPickedSomething(object sender, EventArgs e)
    {
        //PlaySound(_audiorefs.objectPickup, Player.Instance.transform.position);
    }

    private void HandleCuttingCounterOnOnAnyCut(object sender, EventArgs e)
    {
        var counter = sender as CuttingCounter;
        PlaySound(_audiorefs.chop, counter.transform.position);
    }

    private void HandleOnRecipeSuccess(object sender, EventArgs e)
    {
        var counter = DeliveryCounter.Instance;
        PlaySound(_audiorefs.deliverySuccess, counter.transform.position);
    }

    private void HandleOnRecipeFailed(object sender, EventArgs e)
    {
        var counter = DeliveryCounter.Instance;
        PlaySound(_audiorefs.deliveryFail, counter.transform.position);
    }

    private void PlaySound(AudioClip audio, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audio, position, volumeMultiplier * _volume);
    }    
    
    private void PlaySound(AudioClip[] audio, Vector3 position, float volumeMultiplier = 1f)
    {
        PlaySound(audio[Random.Range(0, audio.Length)], position, volumeMultiplier);
    }

    public void PlayFootstepSound(Vector3 position)
    {
        PlaySound(_audiorefs.footstep, position, 0.8f);
    }

    public void ChangeVolume()
    {
        _volume += .1f;
        if (_volume > 1f)
        {
            _volume = 0f;
        }
    }
}