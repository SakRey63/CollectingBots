using UnityEngine;

public class ResourceTransfer : MonoBehaviour
{
    public void TransferResource(Stockroom stockroom, Resource resource)
    {
        stockroom.TransferResource(resource);
    }
}