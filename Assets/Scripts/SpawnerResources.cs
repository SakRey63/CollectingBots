using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerResources : Spawner<Resource>
{
    [SerializeField] private Resource _resource;
    [SerializeField] private float _minPositionX;
    [SerializeField] private float _minPositionZ;
    [SerializeField] private float _maxPositionX;
    [SerializeField] private float _maxPositionZ;
    [SerializeField] private float _positionY;
    [SerializeField] private float _delay;

    private Vector3 _positionSpawn;
    private int _index;

    private void Start()
    {
        StartCoroutine(RepeatResource());
    }

    private IEnumerator RepeatResource()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);

        while (enabled)
        {
            GetGameObject();

            yield return delay;
        }
    }

    protected override Resource ChoosePrefab()
    {
        Prefab = _resource;
        
        return base.ChoosePrefab();
    }

    protected override void SetAction(Resource resource)
    {
        _index++;
        
        SetRandomPosition();
        
        resource.Delivered += ReleaseResource;
        resource.transform.position = _positionSpawn;
        resource.SetIndex(_index);
        
        base.SetAction(resource);
    }

    private void ReleaseResource(Resource resource)
    {
        resource.Delivered -= ReleaseResource;
        
        Release(resource);
    }

    private void SetRandomPosition()
    {
        _positionSpawn = new Vector3(Random.Range(_minPositionX, _maxPositionX), _positionY,
            Random.Range(_minPositionZ, _maxPositionZ));
    }
}