using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Base[] _bases;
    [SerializeField] private SpawnerResources _spawnerResources;
    [SerializeField] private float _delay;
    
    private void Start()
    {
        StartCoroutine(RepeatResource());
    }
    
    private IEnumerator RepeatResource()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);

        while (enabled)
        {
            SetAreaSpawnResources();

            yield return delay;
        }
    }

    private void SetAreaSpawnResources()
    {
        foreach (Base playerBase in _bases)
        {
            _spawnerResources.SetAreaResource(playerBase.AreaScanningBase);
        }
    }
}