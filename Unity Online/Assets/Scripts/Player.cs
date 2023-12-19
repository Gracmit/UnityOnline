using UnityEngine;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private LayerMask _countersLayerMask;

    private bool _isWalking;
    private Vector3 lastInteractionDirection;
    
    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }


    public bool IsWalking()
    {
        return _isWalking;
    }

    public void HandleMovement()
    {
        var inputVector = _gameInput.GetMovementVectorNormalized();
        var moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = _moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance);

        if (!canMove)
        {
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, moveDistance);

            if (canMove)
            {
                moveDirection = moveDirectionX;
            }
            else
            {
                Vector3 moveDirectionZ = new Vector3(0, 0, moveDirection.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionZ, moveDistance);

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
            lastInteractionDirection = moveDirection;

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractionDirection, out RaycastHit raycastHit, interactDistance, _countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                clearCounter.Interact();
            }
        }
    }
}
