using System;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private BaseScanner _scanner;
    
    private List<Resource> _freeResource;
    private List<Resource> _assignedResource;
    private List<Resource> _allResources;

    public int CountFreeResource => _freeResource.Count;

    public event Action<Resource> FoundedNewResource;

    private void OnEnable()
    {
        _scanner.Detected += DetermineStatusResource;
    }

    private void OnDisable()
    {
        _scanner.Detected -= DetermineStatusResource;
    }

    private void Awake()
    {
        _freeResource = new List<Resource>();
        _assignedResource = new List<Resource>();
        _allResources = new List<Resource>();
    }

    public void RemoveToAssignedResource(Resource resource)
    {
        int number = GetIndexResource(_assignedResource, resource);

        _assignedResource.Remove(_assignedResource[number]);
    }

    public Resource GetFreeResource()
    {
        Resource resource;
        
        resource = _freeResource[0]; 
        
        ChangeStatusResource(_freeResource[0]);

        return resource;
    }

    public void ChangeStatusResource(Resource resource)
    {
        int number = GetIndexResource(_freeResource, resource);

        _freeResource.Remove(_freeResource[number]);
        
        _assignedResource.Add(resource);
    }

    private int GetIndexResource(List<Resource> resources, Resource resource)
    {
        int number = 0;

        for (int i = 0; i < resources.Count; i++)
        {
            if (resources[i].Index == resource.Index)
            {
                number = i;
                
                break;
            }
        }

        return number;
    }

    private void DetermineStatusResource(List<Resource> resources)
    {
        _allResources = resources;
        
        bool isFoundMatch = false;
        
        foreach (Resource resource in _allResources)
        {
            if (_assignedResource.Count > 0)
            {
                isFoundMatch = DetectMatches(_assignedResource, resource);
            }
            
            if (isFoundMatch == false)
            {
                if (_freeResource.Count > 0)
                {
                    isFoundMatch = DetectMatches(_freeResource, resource);
                }
                
                if (isFoundMatch == false)
                {
                    _freeResource.Add(resource);
                    
                    FoundedNewResource?.Invoke(resource);
                }
            }
        }
        
        _allResources.Clear();
    }

    private bool DetectMatches(List<Resource> resources, Resource resource)
    {
        bool isFoundMatch = false;
        
        foreach (Resource foundedResource in resources)
        {
            if (foundedResource.Index == resource.Index)
            {
                isFoundMatch = true;
                
                break;
            }
        }
        
        return isFoundMatch;
    }
}