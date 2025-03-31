using UnityEngine;

public class DirectionMovingBot : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private Transform _waitingPoint;
    [SerializeField] private Transform _waitingCheackPoint;
    [SerializeField] private Transform _stockroom;
    [SerializeField] private Transform _stockroomCheackPoint;

    private Transform _positionTarget;

    private bool _isMovingToWarehouse;
    private bool _uploaded;
    private bool _isExitParking;

    private void OnEnable()
    {
        _mover.DeliveredResource += ChooseDirection;
    }

    private void OnDisable()
    {
        _mover.DeliveredResource -= ChooseDirection;
    }

    public void SetDirectionTarget(Transform positionTarget, bool uploaded)
    {
        _positionTarget = positionTarget;

        ChangeDirection(uploaded);
    }

    public void ChangeDirection(bool uploaded)
    {
        _uploaded = uploaded;

        ChooseDirection();
    }

    public void GoBack()
    {
        _mover.ChangeMoving();
    }
    
    private void ChooseDirection()
    {
        if (_positionTarget != null)
        {
            if (_uploaded == false)
            {
                if (_isExitParking)
                {
                    _mover.ChangeDirection(_positionTarget.position);
                }
                else
                {
                    _mover.ChangeDirection(_waitingCheackPoint.position);

                    _isExitParking = true;
                }
            }
            else
            {
                if (_isMovingToWarehouse)
                {
                    _mover.ChangeDirection(_stockroom.position);
                    
                    _positionTarget = null;
                }
                else
                {
                    _mover.ChangeDirection(_stockroomCheackPoint.position);

                    _isMovingToWarehouse = true;
                }
            }  
        }
        else
        {
            _mover.ChangeDirection(_waitingPoint.position);

            _isMovingToWarehouse = false;

            _isExitParking = false;
        }
    }
}