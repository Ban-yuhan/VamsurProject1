using UnityEngine;

public class AliveRegistryAuto : MonoBehaviour
{
    [SerializeField]
    private AliveRegistry aliveRegistry; //레지스트리 참조

    private void Awake()
    {
        aliveRegistry = GameObject.FindAnyObjectByType<AliveRegistry>(); //null방지를 위해 FindAnyObject로 변수에 레지스트리 할당
    }

    private void OnDestroy()
    {
        if (aliveRegistry != null)
        {
            aliveRegistry.Unregister(gameObject); //파괴 시 해당 오브젝트의 데이터 집합(HashSet)에서 삭제
        }
    }
}
