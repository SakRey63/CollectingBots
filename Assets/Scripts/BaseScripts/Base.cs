using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private BaseScanner _scanner;
    [SerializeField] private Garage _garage;
    [SerializeField] private Stockroom _stockroom;
    [SerializeField] private BaseView _baseView;
    [SerializeField] private Transform _areaScanningBase;
    [SerializeField] private Database _database;
    
    private int _countResource;
    
    public Transform AreaScanningBase => _areaScanningBase;

    private void OnEnable()
    {
        _scanner.Detected += SetAllResource;
        _database.FoundedNewResource += AssignBot;
        _garage.FreeBot += AssignBot;
        _stockroom.AcceptedResource += ChangeCountResource;
    }

    private void OnDisable()
    {
        _scanner.Detected -= SetAllResource;
        _database.FoundedNewResource -= AssignBot;
        _garage.FreeBot -= AssignBot;
        _stockroom.AcceptedResource -= ChangeCountResource;
    }

    private void Awake()
    {
        _scanner.SetAreaScan(_areaScanningBase);
        
        _baseView.SetText(_countResource);
    }

    private void SetAllResource(Dictionary<int, Resource> resources)
    {
        _database.DetermineStatusResource(resources);
    }

    private void ChangeCountResource(Resource resource)
    {
        _countResource++;
        
        _baseView.SetText(_countResource);

        _database.RemoveToAssignedResource(resource.Index);
    }

    private void AssignBot()
    {
        if (_database.CountFreeResource > 0 && _garage.CountFreeBots > 0)
        {
            Bot bot = _garage.GetBot();
                
            bot.SetTarget(_database.GetFreeResource());
        }
    }
}