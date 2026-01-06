using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPool : MonoBehaviour
{
    [SerializeField]
    private string poolKey = "Enemy"; // PoolManager에서 찾을 때 쓰는 키

    [SerializeField]
    private EnemyPooledObject prefab; // 프리팹

    [SerializeField]
    private int warmCount = 50; //미리 만들어 놓을 개수

    private readonly Queue<EnemyPooledObject> queue = new Queue<EnemyPooledObject>(); //readonly : queue 자료구조를 읽기 전용으로 설정함.
    private int totalCreated;

    private void Awake()
    {
        Warm(warmCount);
    }


    public void Warm(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            CreateOneInactive();
        }
    }


    public void Release(EnemyPooledObject pooled)
    {
        if (pooled == null)
        {
            return;
        }

        pooled.gameObject.SetActive(false); //사용한 총알을 비활성화
        queue.Enqueue(pooled); //비활성화 한 총알을 queue자료구조에 넣음
    }


    public EnemyPooledObject Spawn()
    {
        EnemyPooledObject obj = null;

        if(queue.Count > 0)
        {
            obj = queue.Dequeue();
        }
        else
        {
            obj = CreateOneInactive();
        }

        if(obj != null)
        {
            obj.gameObject.SetActive(true);
            obj.OnSpawned(pool: this);
        }

        return obj;
    }


    public string GetKey()
    {
        return poolKey;
    }

    private EnemyPooledObject CreateOneInactive()
    {
        if (prefab == null)
        {
            Debug.LogWarning("EnemybjectPool: prefab is null.");
            return null;
        }

        EnemyPooledObject obj = Instantiate(prefab, transform);
        ++totalCreated; 
        obj.gameObject.SetActive(false); 
        queue.Enqueue(obj); 
        return obj;
    }
}
