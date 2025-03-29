using System;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private BotScanner _botScanner;
    [SerializeField] private Mover _mover;
    [SerializeField] private LiftingMechanism _liftingMechanism;
    [SerializeField] private FlashingLight _flashingLight;
    [SerializeField] private Transform _waitingPoint;
    [SerializeField] private Transform _waitingCheackPoint;
    [SerializeField] private Transform _stockroom;
    [SerializeField] private Transform _stockroomCheackPoint;
    
    private bool _isReleased = true;
    private bool _uploaded = true;
    private bool _isReturningToBase;
    private bool _isMovingToWarehouse;
    private Resource _resource;

    public Resource Resource => _resource;
    public bool IsReleased => _isReleased;
    public bool Uploaded => _uploaded;

    public event Action<Bot> Released;

    private void OnEnable()
    {
        _botScanner.AchievedCheckPoint += ChooseDirection;
        _botScanner.AchievedResource += RaisingResource;
        _botScanner.AchievedWaitingPoint += ChangeStatus;
        _liftingMechanism.AscentFinished += ChooseDirection;
        _liftingMechanism.Unloaded += GoBack;
        _mover.DeliveredResource += ChooseDirection;
    }

    private void OnDisable()
    {
        _botScanner.AchievedCheckPoint -= ChooseDirection;
        _botScanner.AchievedResource -= RaisingResource;
        _botScanner.AchievedWaitingPoint -= ChangeStatus;
        _liftingMechanism.AscentFinished -= ChooseDirection;
        _liftingMechanism.Unloaded -= GoBack;
        _mover.DeliveredResource -= ChooseDirection;
    }

    private void Start()
    {
        _flashingLight.ChangeEffect(_isReleased, _uploaded);
    }

    public void ReturnToBase()
    {
        _resource = null;
        
        _isReturningToBase = true;
        
        _mover.ChangeDirection(_waitingPoint);
        
        _flashingLight.ChangeEffect(_isReleased, _uploaded);
    }

    public void SetTarget(Resource resource)
    {
        _isReleased = false;

        _resource = resource;

        _isReturningToBase = false;
        
        _mover.ChangeDirection(_waitingCheackPoint);
        _mover.ChangeSpeed();
        
        _flashingLight.ChangeEffect(_isReleased, _uploaded);
    }

    private void ChangeStatus()
    {
        if (_isReleased == false)
        {
            _mover.ChangeSpeed();
                    
            _isReleased = true;
                    
            Released?.Invoke(this);
            
            _flashingLight.ChangeEffect(_isReleased, _uploaded);
        }
    }

    private void RaisingResource()
    {
        _mover.ChangeSpeed();
        
        if (_isMovingToWarehouse == false)
        {
            _liftingMechanism.ChangeElevator(_uploaded);
                
            _resource.transform.parent = null;
                
            _uploaded = true;
        }
        else
        {
            _liftingMechanism.ChangeElevator(_uploaded);
            
            _resource.transform.parent = transform;
            
            _uploaded = false;
            
            _flashingLight.ChangeEffect(_isReleased, _uploaded);
        }
    }

    private void ChooseDirection()
    {
        if (_resource != null && _uploaded)
        {
            _mover.ChangeDirection(_resource.transform);
            
            _isMovingToWarehouse = true;
        }
        else if (_resource != null && _uploaded == false)
        {
            if (_isMovingToWarehouse)
            {
                _mover.ChangeSpeed();
                _mover.ChangeDirection(_stockroomCheackPoint);
                
                _isMovingToWarehouse = false;
            }
            else
            {
                _mover.ChangeDirection(_stockroom);
            }
        }
        else 
        {
            if (_isReturningToBase == false)
            {
                _mover.ChangeDirection(_waitingPoint);
                _mover.ChangeSpeed();
            }
        }
    }
    
    private void GoBack()
    {
        _flashingLight.ChangeEffect(_isReleased, _uploaded);
        
        _resource = null;
        
        _mover.ChangeMoving();
    }
}