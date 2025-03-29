using System;
using System.Collections;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private float _delay;
    
    private bool _isTransported;
    private int _index;

    public int Index => _index;
    public bool IsTransported => _isTransported;
    
    public event Action<Resource> Delivered;

    private IEnumerator WaitingСall()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);

        yield return delay;
        
        Delivered?.Invoke(this);
    }

    public void SetIndex(int index)
    {
        _index = index;
    }
    public void ChangeStatus()
    {
        _isTransported = true;
    }

    public void ReturnToPool()
    {
        _isTransported = false;
        
        StartCoroutine(WaitingСall());
    }
}