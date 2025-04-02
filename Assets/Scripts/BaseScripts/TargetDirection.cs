using UnityEngine;

public class TargetDirection : MonoBehaviour
{
    [SerializeField] private Rotator _rotator;
    [SerializeField] private Transform _parking; 
    [SerializeField] private Transform _parkingExit;
    [SerializeField] private Transform _stockroom;
    [SerializeField] private Transform _stockroomExit;

    public void ChangeDirectionParkingExit()
    {
        _rotator.ChangeDirection(_parkingExit.position);
    }

    public void ChangeDirectionResourcePosition(Vector3 position)
    {
        _rotator.ChangeDirection(position);
    }
    
    public void ChangeDirectionStockroomExit()
    {
        _rotator.ChangeDirection(_stockroomExit.position);
    }
    
    public void ChangeDirectionStockroom()
    {
        _rotator.ChangeDirection(_stockroom.position);
    }
    
    public void ChangeDirectionParking()
    {
        _rotator.ChangeDirection(_parking.position);
    }
}