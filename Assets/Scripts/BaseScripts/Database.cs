using System;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    [SerializeField] private BaseScanner _scanner;
    
    private Dictionary<int, Resource> _freeResource;
    private HashSet<int> _assignedResource;
    private List<int> _indexFreeResource;
    
    public int CountFreeResource => _freeResource.Count;

    public event Action FoundedNewResource;

    private void Awake()
    {
        _indexFreeResource = new List<int>();
        _freeResource = new Dictionary<int, Resource>();
        _assignedResource = new HashSet<int>();
    }

    public void DetermineStatusResource(Dictionary<int, Resource> resources)
    {
        foreach(var resource in resources)
        {
            if (_assignedResource.Contains(resource.Key) == false && _freeResource.ContainsKey(resource.Key) == false)
            {
                _freeResource.Add(resource.Key, resource.Value);
                
                _indexFreeResource.Add(resource.Key);
                    
                FoundedNewResource?.Invoke();
            }
        }
    }
    
    public void RemoveToAssignedResource(int index)
    {
        _assignedResource.Remove(index);
    }

    public Resource GetFreeResource()
    {
        int number = _indexFreeResource[0];
        
        Resource resource = _freeResource[number];
        
        _indexFreeResource.Remove(number);
        
        ChangeStatusResource(_freeResource[number]);

        return resource;
    }

    private void ChangeStatusResource(Resource resource)
    {
        _freeResource.Remove(resource.Index);
        
        _assignedResource.Add(resource.Index);
    }
}