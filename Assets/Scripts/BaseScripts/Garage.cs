using System;
using System.Collections.Generic;
using UnityEngine;

public class Garage : MonoBehaviour
{
    [SerializeField] private List<Bot> _allBots;
    
    private Queue<Bot> _freeBots;

    public int CountFreeBots => _freeBots.Count;

    public event Action FreeBot;

    private void OnEnable()
    {
        foreach (Bot bot in _allBots)
        {
            bot.Released += ChangeStatus;
        }
    }

    private void OnDisable()
    {
        foreach (Bot bot in _allBots)
        {
            bot.Released -= ChangeStatus;
        }
    }

    private void Awake()
    {
        _freeBots = new Queue<Bot>();
    }

    private void Start()
    {
        SetFreeBot();
    }

    public Bot GetBot()
    {
       return _freeBots.Dequeue();
    }

    private void ChangeStatus(Bot bot)
    {
        _freeBots.Enqueue(bot);
        
        FreeBot?.Invoke();
    }

    private void SetFreeBot()
    {
        foreach (Bot bot in _allBots)
        {
            if (bot.IsReleased)
            {
                _freeBots.Enqueue(bot);
            }
        }
    }
}