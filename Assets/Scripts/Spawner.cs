using UnityEngine;
using UnityEngine.Pool;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private int _poolCapaciti = 15;
    [SerializeField] private int _poolMaxSize = 30;

    private ObjectPool<T> _pool;
    
    protected T Prefab;
    
    protected void Awake()
    {
        _pool = new ObjectPool<T>(
            createFunc: () => Instantiate(ChoosePrefab()),
            actionOnGet: (obj) => SetAction(obj),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapaciti,
            maxSize: _poolMaxSize);
    }

    protected virtual T ChoosePrefab()
    {
        return Prefab;
    }
    
    protected virtual void SetAction(T obj)
    {
        obj.gameObject.SetActive(true);
    }
    
    protected void GetGameObject()
    {
        _pool.Get();
    }

    protected void Release(T obj)
    {
        _pool.Release(obj);
    }
}