using System;
using UnityEngine;

public class Stockroom : MonoBehaviour
{
    public event Action<Resource> AcceptedResource;

    public void TransferResource(Resource resource)
    {
        AcceptedResource?.Invoke(resource);
        
        resource.ReturnToPool();
    }
}