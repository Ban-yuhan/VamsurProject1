using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 여러 ObjectPool을 묶어 키로 관리한다
/// Spawn/Release를 간단히 호출할 수 있게 해준다
/// </summary>
public class PoolManager : MonoBehaviour
{
    [SerializeField]
    private List<ObjectPool> pools = new List<ObjectPool>();

    private readonly Dictionary<string, ObjectPool> map = new Dictionary<string, ObjectPool>(); //나중에 적 몬스터나 다른 오브젝트도 ObjectPool을 사용하게 되면, Dictionary에 저장하여 string에 해당하는 ObjectPool을 불러올 수 있도록 설정


    private void Awake()
    {
        BuildMap();
    }


    /// <summary>
    /// 등록된 풀을 키로 맵에 올린다.
    /// </summary>
    private void BuildMap()
    {
        map.Clear();
        for(int i = 0; i < pools.Count; ++i)
        {
            ObjectPool p = pools[i];
            if( p == null)
            {
                continue;
            }

            string key = p.GetKey();
            if (string.IsNullOrEmpty(key) == true)
            {
                continue;
            }

            if(map.ContainsKey(key) == false)
            {
                map.Add(key, p); //Dictionary에 등록되어있지 않은 key라면 등록.
            }
            else
            {
                Debug.LogWarning("PoolManager: duplicated key = : + key"); //이미 등록되어있는 key 면 경고 메세지 출력
            }
        }
    }


    /// <summary>
    /// 키를 이용해서 오브젝트 하나를 꺼낸다
    /// </summary>
    public PooledObject Spawn(string key, Vector3 pos, Quaternion rot) //key : Dictionary의 stiring형의 key가 일치하는 풀의 오브젝트를 꺼냄
    {
        ObjectPool p;
        if(map.TryGetValue(key, out p) == false) //TryGetValue : Dictionary의 key에 해당하는 오브젝트를 p에 할당. 만약 할당되지 않았다면 아래를 실행
        {
            Debug.LogWarning("PoolManager: no pool for key = " + key);
            return null;
        }

        PooledObject obj = p.Spawn();
        if(obj != null)
        {
            Transform t = obj.transform;
            t.position = pos;
            t.rotation = rot;
        }

        return obj;
    }


    /// <summary>
    /// 오브젝트를 반환
    /// </summary>
    public void Release(PooledObject obj)
    {
        if(obj == null)
        {
            return;
        }

        ObjectPool pool = obj.GetComponentInParent<ObjectPool>();
        if(pool != null)
        {
            pool.Release(obj); //부모 오브젝트의 ObjectPool을 가져와서 Release 실행
            return;
        }

        //만약 소유 풀을 찾지 못한 경우 그냥 비활성화 처리
        obj.gameObject.SetActive(false);
    }
}
