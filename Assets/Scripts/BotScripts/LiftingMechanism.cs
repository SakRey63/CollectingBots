using System;
using System.Collections;
using UnityEngine;

public class LiftingMechanism : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _delay;

    private bool _isUploaded;
    private Resource _resource;

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

    public void ChangeElevator(Resource resource, bool isUploaded)
    {
        if (_resource == null)
        {
            _resource = resource;
        }

        _isUploaded = isUploaded;
        
        StartCoroutine(WorkingElevator());
    }
}