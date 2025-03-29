using System;
using UnityEngine;

public class BaseScanner : MonoBehaviour
{
    public event Action<Resource> Detected;
    public event Action<Resource> Disappeared;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource  resource))
        {
            Detected?.Invoke(resource);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            Disappeared?.Invoke(resource);
        }
    }
}