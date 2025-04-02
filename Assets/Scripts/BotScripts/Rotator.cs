using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;

    private bool _isRotation;
    private Vector3 _lockAtTarget;
    private Vector3 _direction;

    private void Update()
    {
        Rotation();
    }

    public void ChangeRotation(bool isRotation)
    {
        _isRotation = isRotation;
    }

    public void ChangeDirection(Vector3 target)
    {
        _lockAtTarget = target;
    }
    
    private void Rotation()
    {
        if (_lockAtTarget != null && _isRotation)
        {
            _direction = _lockAtTarget - transform.position;
            
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_direction), _rotationSpeed * Time.deltaTime);
        }
    }
}