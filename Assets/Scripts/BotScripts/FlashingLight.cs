using UnityEngine;

public class FlashingLight : MonoBehaviour
{
    [SerializeField] private Effect _effectFollowing;
    [SerializeField] private Effect _effectWaiting;
    [SerializeField] private Effect _effectWork;

    private Effect _effect;

    public void ChangeEffectWaiting()
    {
        InstallEffect(_effectWaiting);
    }
    
    public void ChangeEffectFollowing()
    {
        InstallEffect(_effectFollowing);
    }
    
    public void ChangeEffectWork()
    {
        InstallEffect(_effectWork);
    }

    private void InstallEffect(Effect effect)
    {
        if (_effect != null)
        {
            _effect.ResetEffect();
        }
        
        _effect = effect;

        _effect.PlayEffect();
    }
}