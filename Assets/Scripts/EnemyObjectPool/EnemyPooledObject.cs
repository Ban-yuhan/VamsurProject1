
using UnityEngine;

public class EnemyPooledObject : MonoBehaviour
{
    private EnemyObjectPool ownerPool;

    public void OnSpawned(EnemyObjectPool pool)
    {
        ownerPool = pool;
    }

    public void Release()
    {
        if (ownerPool != null)
        {
            ownerPool.Release(pooled: this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

}
