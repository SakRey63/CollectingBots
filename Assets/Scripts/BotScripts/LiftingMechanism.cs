using System;
using System.Collections;
using UnityEngine;

public class LiftingMechanism : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _delay;

    private bool _isRaise;

    public event Action AscentFinished;
    public event Action Unloaded;

    private IEnumerator WorkingElevator()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);
        
        _animator.SetBool(AnimatorData.Params.Raise, _isRaise);

        yield return delay;

        if (_isRaise)
        {
            AscentFinished?.Invoke();
        }
        else
        {
            Unloaded?.Invoke();
        }
    }

    public void ChangeElevator(bool isRaise)
    {
        _isRaise = isRaise;
        
        StartCoroutine(WorkingElevator());
    }
}