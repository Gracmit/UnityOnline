using System;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    [SerializeField] private ContainerCounter _containerCounter;
    
    private Animator _animator;
    private static readonly int OpenClose = Animator.StringToHash("OpenClose");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _containerCounter.OnPlayerGrabbedObject += ContainerCounterOnOnPlayerGrabbedObject;
    }

    private void ContainerCounterOnOnPlayerGrabbedObject(object sender, EventArgs e)
    {
        _animator.SetTrigger(OpenClose);
    }
}
