using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private BaseScanner _scanner;
    [SerializeField] private Bot[] _bots;
    [SerializeField] private Stockroom[] _stockrooms;
    [SerializeField] private BaseView _baseView;
    
    private bool _isDefined;
    private int _indexRemove;
    private int _countResource;

    private List<Resource> _waitingList;

    private void OnEnable()
    {
        _scanner.Detected += AssignBot;
        _scanner.Disappeared += DetermineWhoseResource;

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
        _scanner.Detected -= AssignBot;
        _scanner.Disappeared -= DetermineWhoseResource;
        
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
        _baseView.SetText(_countResource);
        
        _waitingList = new List<Resource>();
    }

    private void ChangeCountResource()
    {
        _countResource++;
        
        _baseView.SetText(_countResource);
    }

    private void DetermineWhoseResource(Resource resource)
    {
        _isDefined = false;
        
        foreach (Resource waitingResource in _waitingList)
        {
            if (waitingResource.Index == resource.Index)
            {
                _waitingList.Remove(waitingResource);

                _isDefined = true;
                
                break;
            }
        }
        
        if(_isDefined == false)
        {
            foreach (Bot bot in _bots)
            {
                if (bot.Resource != null)
                {
                    if (bot.Resource.Index == resource.Index && bot.Uploaded)
                    { 
                        bot.ReturnToBase();
                    }
                }
            }
        }
    }

    private void ViewWaitingList(Bot bot)
    {
        if (_waitingList.Count > 0)
        {
            bot.SetTarget(_waitingList[0]);

            _waitingList.Remove(_waitingList[0]);
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

        if (isAppointed == false)
        {
            AddWaitingListResources(resource);
        }
    }

    private void AddWaitingListResources(Resource resource)
    {
        _waitingList.Add(resource);
    }
}