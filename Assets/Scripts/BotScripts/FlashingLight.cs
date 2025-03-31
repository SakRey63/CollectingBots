using UnityEngine;

public class FlashingLight : MonoBehaviour
{
    [SerializeField] private Effect _effectFollowing;
    [SerializeField] private Effect _effectWaiting;
    [SerializeField] private Effect _effectWork;

    private Effect _effect;

    public void ChangeEffect(bool isReleased, bool uploaded)
    {
        if (isReleased)
        {
            InstallEffect(_effectWaiting);
        }
        else
        {
            if (uploaded == false)
            {
                InstallEffect(_effectFollowing);
            }
            else
            {
                InstallEffect(_effectWork);
            }
        }
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