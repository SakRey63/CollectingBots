using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private BaseScanner _scanner;
    [SerializeField] private Bot[] _bots;
    [SerializeField] private Stockroom[] _stockrooms;
    [SerializeField] private BaseView _baseView;
    [SerializeField] private Transform _areaScanningBase;
    [SerializeField] private Storage _storage;
    
    private bool _isDefined;
    private int _indexRemove;
    private int _countResource;

    public Transform AreaScanningBase => _areaScanningBase;

    private void OnEnable()
    {
        _storage.FoundedNewResource += AssignBot;
        
        foreach (Stockroom stockroom in _stockrooms)
        {
            stockroom.AcceptedResource += ChangeCountResource;
        }
        
        foreach (Bot bot in _bots)
        {
            bot.Released += ViewWaitingList;
        }
    }

    private void OnDisable()
    {
        _storage.FoundedNewResource -= AssignBot;
        
        foreach (Stockroom stockroom in _stockrooms)
        {
            stockroom.AcceptedResource -= ChangeCountResource;
        }
        
        foreach (Bot bot in _bots)
        {
            bot.Released -= ViewWaitingList;
        }
    }

    private void Start()
    {
        _scanner.SetAreaScan(_areaScanningBase);
        
        _baseView.SetText(_countResource);
    }

    private void ChangeCountResource(Resource resource)
    {
        _countResource++;
        
        _baseView.SetText(_countResource);

        _storage.RemoveToAssignedResource(resource);
    }

    private void ViewWaitingList(Bot bot)
    {
        if (_storage.CountFreeResource > 0)
        {
            bot.SetTarget(_storage.GetFreeResource());
        }
    }

    private void AssignBot(Resource resource)
    {
        bool isAppointed = false;
        
        foreach (Bot bot in _bots)
        {
            if (bot.IsReleased && isAppointed == false)
            {
                bot.SetTarget(resource);

                isAppointed = true;
            }
        }

        if (isAppointed)
        {
            _storage.ChangeStatusResource(resource);
        }
    }
}