using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;

    public void PlayEffect()
    {
        _effect.Play();
    }

    public void ResetEffect()
    {
        _effect.Stop();
    }
}