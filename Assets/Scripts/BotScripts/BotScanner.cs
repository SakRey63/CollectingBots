using System;
using UnityEngine;

public class BotScanner : MonoBehaviour
{
    public event Action AchievedCheckPoint;
    public event Action AchievedResource;
    public event Action AchievedWaitingPoint;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CheckPointWaiting>(out _))
        {
            AchievedCheckPoint?.Invoke();
        }
        else if(other.TryGetComponent(out Resource resource))
        {
            if (resource.IsTransported == false)
            {
                resource.ChangeStatus();
                
                AchievedResource?.Invoke();
            }
        }
        else if(other.TryGetComponent<CheckPointStockroom>(out _))
        {
            AchievedCheckPoint?.Invoke();
        }
        else if (other.TryGetComponent<Stockroom>(out _))
        {
            AchievedResource?.Invoke();
        }
        else if (other.TryGetComponent<WaitingPoint>(out _))
        {
            AchievedWaitingPoint?.Invoke();
        }
    }
}