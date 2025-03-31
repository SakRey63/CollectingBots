using System;
using UnityEngine;

public class BotScanner : MonoBehaviour
{
    private Resource _resource;
    
    public event Action AchievedCheckPoint;
    public event Action<Resource> AchievedResource;
    public event Action AchievedWaitingPoint;
    public event Action<Stockroom> AchievedStockroom;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CheckPointWaiting>(out _))
        {
            AchievedCheckPoint?.Invoke();
        }
        else if(other.TryGetComponent(out Resource resource))
        {
            AchievedResource?.Invoke(resource);
        }
        else if(other.TryGetComponent<CheckPointStockroom>(out _))
        {
            AchievedCheckPoint?.Invoke();
        }
        else if (other.TryGetComponent(out Stockroom stockroom))
        {
             AchievedStockroom?.Invoke(stockroom);
            
        }
        else if (other.TryGetComponent<WaitingPoint>(out _))
        {
            AchievedWaitingPoint?.Invoke();
        }
    }
}