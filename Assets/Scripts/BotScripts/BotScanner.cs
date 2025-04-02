using System;
using UnityEngine;

public class BotScanner : MonoBehaviour
{
    public event Action AchievedCheckPoint;
    public event Action<Resource> AchievedResource;
    public event Action AchievedWaitingPoint;
    public event Action<Stockroom> AchievedStockroom;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ParkingExit>(out _))
        {
            AchievedCheckPoint?.Invoke();
        }
        else if(other.TryGetComponent(out Resource resource))
        {
            AchievedResource?.Invoke(resource);
        }
        else if(other.TryGetComponent<StockroomExit>(out _))
        {
            AchievedCheckPoint?.Invoke();
        }
        else if (other.TryGetComponent(out Stockroom stockroom))
        {
             AchievedStockroom?.Invoke(stockroom);
        }
        else if (other.TryGetComponent<Parking>(out _))
        {
            AchievedWaitingPoint?.Invoke();
        }
    }
}