using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyPlayerSpawned;

    public static event EventHandler OnAnyPickedSomething;
    public static void ResetStaticData()
    {
        OnAnyPlayerSpawned = null;
        OnAnyPickedSomething = null;
    }
    
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    
    public static Player LocalInstance => _localInstance;
  
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private LayerMask _countersLayerMask;
    [SerializeField] private LayerMask _collisionsLayerMask;
    [SerializeField] private Transform _handPosition;
    [SerializeField] private List<Vector3> _spawnPositionList;

    private static Player _localInstance;
    private bool _isWalking;
    private Vector3 _lastInteractionDirection;
    private BaseCounter _selectedCounter;
    private KitchenObject _kitchenObject;
    private GameInput _gameInput;

    private void Start()
    {
        _gameInput = GameInput.Instance;
        _gameInput.OnInteractAction += HandleInteractAction;
        _gameInput.OnInteractAlternateAction += HandleInteractAlternateAction;
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            if (_localInstance == null)
            {
                _localInstance = this;
            }
        }

        transform.position = _spawnPositionList[(int)OwnerClientId];
        
        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        HandleMovement();
        HandleInteractions();
    }


    public bool IsWalking()
    {
        return _isWalking;
    }

    private void HandleMovement()
    {
        var inputVector = _gameInput.GetMovementVectorNormalized();
        var moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = _moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDirection, Quaternion.identity, moveDistance, _collisionsLayerMask);

        if (!canMove)
        {
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;
            canMove = (moveDirection.x  < -.2f || moveDirection.x > .2f) &&  !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDirectionX, Quaternion.identity, moveDistance, _collisionsLayerMask);

            if (canMove)
            {
                moveDirection = moveDirectionX;
            }
            else
            {
                Vector3 moveDirectionZ = new Vector3(0, 0, moveDirection.z).normalized;
                canMove = (moveDirection.z  < -.2f || moveDirection.z > .2f) && !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDirectionZ, Quaternion.identity, moveDistance, _collisionsLayerMask);

                if (canMove)
                    moveDirection = moveDirectionZ;
            }
        }

        if(canMove)
            transform.position += moveDirection * (moveDistance);

        _isWalking = moveDirection != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }
    
    private void HandleInteractions()
    {
        var inputVector = _gameInput.GetMovementVectorNormalized();
        var moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDirection != Vector3.zero)
            _lastInteractionDirection = moveDirection;

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, _lastInteractionDirection, out RaycastHit raycastHit, interactDistance, _countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != _selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }                 
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }
    
    private void HandleInteractAction(object sender, EventArgs eventArgs)
    {
        if(!GameManager.Instance.IsGamePlaying())
            return;
        
        if (_selectedCounter != null)
        {
            _selectedCounter.Interact(this);
        }
    }
    
    private void HandleInteractAlternateAction(object sender, EventArgs e)
    {
        if(!GameManager.Instance.IsGamePlaying())
            return;
        
        if (_selectedCounter != null)
        {
            _selectedCounter.InteractAlternate(this);
        }
    }


    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        _selectedCounter = selectedCounter;
        
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = _selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform() => _handPosition;

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnAnyPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject() => _kitchenObject;

    public void ClearKitchenObject() => _kitchenObject = null;

    public bool HasKitchenObject() => _kitchenObject != null;
    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}
