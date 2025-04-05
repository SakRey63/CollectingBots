using System;
using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _delayDriveBack;
    
    private bool _isMoving;
    private Vector3 _directionMovementForward = Vector3.forward;
    private Vector3 _directionMovementBack = -Vector3.forward;
    private Vector3 _directionMovement;
    public event Action DeliveredResource;

    private void Awake()
    {
        _directionMovement = _directionMovementForward;
    }

    private void Update()
    {
        Move();
    }

    private IEnumerator DriveBack()
    {
        WaitForSeconds delay = new WaitForSeconds(_delayDriveBack);

        _isMoving = true;

        _directionMovement = _directionMovementBack;

        yield return delay;

        _isMoving = false;

        _directionMovement = _directionMovementForward;
        
        DeliveredResource?.Invoke();
    }
    
    public void ChangeMove(bool isMoving)
    {
        _isMoving = isMoving;
    }

    public void SetBotMoveBack()
    {
        StartCoroutine(DriveBack());
    }
    
    private void Move()
    {
        if (_isMoving)
        {
            transform.Translate(_directionMovement * _moveSpeed * Time.deltaTime, Space.Self);
        }
    }
}