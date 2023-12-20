using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CuttingCounterVisual : MonoBehaviour
{
    [FormerlySerializedAs("_containerCounter")] [SerializeField] private CuttingCounter _cuttingCounter;
    
    private Animator _animator;
    private static readonly int Cut = Animator.StringToHash("Cut");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _cuttingCounter.OnCut += CuttingCounterOnOnCut;
    }

    private void CuttingCounterOnOnCut(object sender, EventArgs e)
    {
        _animator.SetTrigger(Cut);
    }
}
