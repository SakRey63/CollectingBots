using System;
using UnityEngine;

public class Stockroom : MonoBehaviour
{
    public event Action AcceptedResource;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            AcceptedResource?.Invoke();
            
            resource.ReturnToPool();
        }
    }
}