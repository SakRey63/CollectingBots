using System;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private BotScanner _botScanner;
    [SerializeField] private TargetDirection _targetDirection;
    [SerializeField] private Mover _mover;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private LiftingMechanism _liftingMechanism;
    [SerializeField] private ResourceTransfer _resourceTransfer;
    [SerializeField] private FlashingLight _flashingLight;
    
    private bool _isReleased = true;
    private bool _isUploaded;
    private bool _isMoving;
    private bool _isRotating;
    private Resource _resource;
    
    public bool IsReleased => _isReleased;

    public event Action<Bot> Released;

    private void OnEnable()
    {
        _botScanner.AchievedCheckPoint += SetDirection;
        _botScanner.AchievedResource += RaisingResource;
        _botScanner.AchievedStockroom += UnloadingResource;
        _botScanner.AchievedWaitingPoint += ChangeStatus;
        _liftingMechanism.AscentFinished += DeliveringResource;
        _liftingMechanism.Unloaded += DriveBack;
        _mover.DeliveredResource += ReturnToParking;
    }

    private void OnDisable()
    {
        _botScanner.AchievedCheckPoint -= SetDirection;
        _botScanner.AchievedResource -= RaisingResource;
        _botScanner.AchievedStockroom -= UnloadingResource;
        _botScanner.AchievedWaitingPoint -= ChangeStatus;
        _liftingMechanism.AscentFinished -= DeliveringResource;
        _liftingMechanism.Unloaded -= DriveBack;
        _mover.DeliveredResource -= ReturnToParking;
    }

    private void Start()
    {
        _flashingLight.ChangeEffectWaiting();
    }

    public void SetTarget(Resource resource)
    {
        _resource = resource;
        
        _isReleased = false;

        _targetDirection.ChangeDirectionParkingExit();
        
        ChangeMovementRotation();
        
        _flashingLight.ChangeEffectFollowing();
    }

    private void ChangeStatus()
    {
        if (_isReleased == false)
        {
            _isReleased = true;

            ChangeMovementRotation();
                    
            Released?.Invoke(this);
            
            _flashingLight.ChangeEffectWaiting();
        }
    }

    private void ChangeMovementRotation()
    {
        if (_isRotating && _isMoving)
        {
            _isRotating = false;
            _isMoving = false;
            
            _rotator.ChangeRotation(_isRotating);
            _mover.ChangeMove(_isMoving);
        }
        else
        {
            _isRotating = true;
            _isMoving = true;
            
            _rotator.ChangeRotation(_isRotating);
            _mover.ChangeMove(_isMoving);
        }
    }

    private void ReturnToParking()
    {
        _targetDirection.ChangeDirectionParking();

        ChangeMovementRotation();
    }

    private void SetDirection()
    {
        if (_isUploaded == false)
        {
            _targetDirection.ChangeDirectionResourcePosition(_resource.transform.position);
        }
        else
        {
            _targetDirection.ChangeDirectionStockroom();
        }
    }

    private void DriveBack()
    {
        _isUploaded = false;
        
        _mover.SetBotMoveBack();
        
        _flashingLight.ChangeEffectFollowing();
    }

    private void UnloadingResource(Stockroom stockroom)
    {
        bool isUploaded = false;
        
        ChangeMovementRotation();
        
        _liftingMechanism.ChangeElevator(_resource, isUploaded);
        
        _resourceTransfer.TransferResource(stockroom, _resource);

        _resource = null;
    }

    private void DeliveringResource()
    {
        _isUploaded = true;
        
        _targetDirection.ChangeDirectionStockroomExit();
        
        ChangeMovementRotation();
    }

    private void RaisingResource(Resource resource)
    {
        if (_resource != null && _resource.Index == resource.Index)
        {
            bool isUploaded = true;
            
            ChangeMovementRotation();
            
            _liftingMechanism.ChangeElevator(resource, isUploaded);
            
            _flashingLight.ChangeEffectWork();
        }
    }
}