using System;
using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _delayDriveBack;
    
    private bool _isMovingForward;
    private bool _isMovingBack;

    public event Action DeliveredResource;

    private void Update()
    {
        Move();
    }

    private IEnumerator DriveBack()
    {
        WaitForSeconds delay = new WaitForSeconds(_delayDriveBack);

        _isMovingBack = true;

        yield return delay;

        _isMovingBack = false;
        
        DeliveredResource?.Invoke();
    }
    
    public void ChangeMove(bool isMoving)
    {
        _isMovingForward = isMoving;
    }

    public void SetBotMoveBack()
    {
        StartCoroutine(DriveBack());
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