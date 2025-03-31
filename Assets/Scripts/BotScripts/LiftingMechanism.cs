using System;
using System.Collections;
using UnityEngine;

public class LiftingMechanism : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _delay;

    private bool _isUploaded;
    private Resource _resource;

    public bool IsUploaded => _isUploaded;

    public event Action AscentFinished;
    public event Action Unloaded;

    private IEnumerator WorkingElevator()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);
        
        _animator.SetBool(AnimatorData.Params.Raise, _isUploaded);

        yield return delay;

        if (_isUploaded)
        {
            _resource.transform.parent = transform;
            
            AscentFinished?.Invoke();
        }
        else
        {
            _resource.transform.parent = null;

            _resource = null;
            
            Unloaded?.Invoke();
        }
    }

    public void TransferResource(Stockroom stockroom)
    {
        stockroom.TransferResource(_resource);
    }

    public void ChangeElevator(Resource resource)
    {
        if (_resource == null)
        {
            _resource = resource;
        }

        _isUploaded = !_isUploaded;
        
        StartCoroutine(WorkingElevator());
    }
}