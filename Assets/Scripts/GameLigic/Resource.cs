using System;
using System.Collections;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private float _delay;
    
    private int _index;

    public int Index => _index;
    
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

    public void ReturnToPool()
    {
        _index = 0;
        
        StartCoroutine(WaitingСall());
    }
}