using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScanner : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private float _maxX;
    [SerializeField] private float _maxY;
    [SerializeField] private float _maxZ;
    [SerializeField] private float _halfFromCenter;
    
    private Transform _transformScan;
    private Vector3 _boxSize;

    private Dictionary<int, Resource> _allResourcesFound;
    
    public event Action<Dictionary<int, Resource>> Detected;

    private void Awake()
    {
        _boxSize = new Vector3(_maxX, _maxY, _maxZ);
        
        _allResourcesFound = new Dictionary<int, Resource>();
    }
    
    private IEnumerator RepeatedScanningTerritory()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);

        while (enabled)
        {
            ScanningTerritory();
            
            yield return delay;
        }
    }
    
    public void SetAreaScan(Transform transform)
    {
        _transformScan = transform;

        StartCoroutine(RepeatedScanningTerritory());
    }

    private void ScanningTerritory()
    {
        Collider[] hitColliders = Physics.OverlapBox(_transformScan.position, _boxSize / _halfFromCenter, Quaternion.identity);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out Resource resource))
            {
                _allResourcesFound.Add(resource.Index, resource);
            }
        }

        if (_allResourcesFound.Count > 0)
        {
            Detected?.Invoke(_allResourcesFound);
        }
        
        _allResourcesFound.Clear();
    }
}