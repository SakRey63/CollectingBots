using System;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private BotScanner _botScanner;
    [SerializeField] private DirectionMovingBot _directionMoving;
    [SerializeField] private Mover _mover;
    [SerializeField] private LiftingMechanism _liftingMechanism;
    [SerializeField] private FlashingLight _flashingLight;
    
    private bool _isReleased = true;
    private bool _isMoving;
    private Resource _resource;
    
    public bool IsReleased => _isReleased;

    public event Action<Bot> Released;

    private void OnEnable()
    {
        _botScanner.AchievedCheckPoint += ChooseDirection;
        _botScanner.AchievedResource += RaisingResource;
        _botScanner.AchievedStockroom += UnloadingResource;
        _botScanner.AchievedWaitingPoint += ChangeStatus;
        _liftingMechanism.AscentFinished += DeliveringResource;
        _liftingMechanism.Unloaded += DriveBack;
    }

    private void OnDisable()
    {
        _botScanner.AchievedCheckPoint -= ChooseDirection;
        _botScanner.AchievedResource -= RaisingResource;
        _botScanner.AchievedStockroom -= UnloadingResource;
        _botScanner.AchievedWaitingPoint -= ChangeStatus;
        _liftingMechanism.AscentFinished -= DeliveringResource;
        _liftingMechanism.Unloaded -= DriveBack;
    }

    private void Start()
    {
        _flashingLight.ChangeEffect(_isReleased, _liftingMechanism.IsUploaded);
    }

    public void SetTarget(Resource resource)
    {
        _resource = resource;
        
        _isReleased = false;

        _isMoving = true;
        
        _directionMoving.SetDirectionTarget(_resource.transform, _liftingMechanism.IsUploaded);
        
        _mover.ChangeSpeed(_isMoving);
        
        _flashingLight.ChangeEffect(_isReleased, _liftingMechanism.IsUploaded);
    }

    private void ChangeStatus()
    {
        if (_isReleased == false)
        {
            _resource = null;
            
            _isReleased = true;

            _isMoving = false;
            
            _mover.ChangeSpeed(_isMoving);
                    
            Released?.Invoke(this);
            
            _flashingLight.ChangeEffect(_isReleased, _liftingMechanism.IsUploaded);
        }
    }

    private void DriveBack()
    {
        _directionMoving.GoBack();
    }

    private void UnloadingResource(Stockroom stockroom)
    {
        _isMoving = false;
        
        _mover.ChangeSpeed(_isMoving);
        
        _liftingMechanism.ChangeElevator(_resource);
        _liftingMechanism.TransferResource(stockroom);
        
        ChooseDirection();
        
        _flashingLight.ChangeEffect(_isReleased, _liftingMechanism.IsUploaded);
    }

    private void DeliveringResource()
    {
        _isMoving = true;
        
        _mover.ChangeSpeed(_isMoving);
    }

    private void RaisingResource(Resource resource)
    {
        _isMoving = false;
        
        _mover.ChangeSpeed(_isMoving);

        if (_resource.Index == resource.Index)
        {
            _liftingMechanism.ChangeElevator(resource);
        }
        
        ChooseDirection();
        
        _flashingLight.ChangeEffect(_isReleased, _liftingMechanism.IsUploaded);
    }

    private void ChooseDirection()
    {
        _directionMoving.ChangeDirection(_liftingMechanism.IsUploaded);
    }
}