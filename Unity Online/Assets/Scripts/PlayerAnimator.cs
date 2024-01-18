using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    [SerializeField] private Player _player;
    
    private Animator _animator;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    private void Awake() => _animator = GetComponent<Animator>();

    private void Update()
    {
        if (!IsOwner) return;
        
        _animator.SetBool(IsWalking, _player.IsWalking());
    }
}
