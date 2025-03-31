using System;
using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _delayDriveBack;
    
    private Vector3 _lockAtTarget;
    private Vector3 _direction;
    
    private bool _isMovingForward;
    private bool _isMovingBack;

    public event Action DeliveredResource;

    private void Update()
    {
        Move();
        RotationBot();
    }

    private IEnumerator DriveBack()
    {
        WaitForSeconds delay = new WaitForSeconds(_delayDriveBack);

        _isMovingBack = true;

        yield return delay;

        _isMovingBack = false;

        _isMovingForward = true; 
        
        DeliveredResource?.Invoke();
    }

    public void ChangeDirection(Vector3 target)
    {
        _lockAtTarget = target;
    }
    
    public void ChangeSpeed(bool isMoving)
    {
        _isMovingForward = isMoving;
    }

    public void ChangeMoving()
    {
        StartCoroutine(DriveBack());
    }
    
    private void RotationBot()
    {
        if (_lockAtTarget != null && _isMovingForward)
        {
            _direction = _lockAtTarget - transform.position;
            
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_direction), _rotationSpeed * Time.deltaTime);
        }
    }
    
    private void Move()
    {
        if (_isMovingForward)
        {
            transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime, Space.Self);
        }

        if (_isMovingBack)
        {
            transform.Translate(-Vector3.forward * _moveSpeed * Time.deltaTime, Space.Self);
        }
    }
}