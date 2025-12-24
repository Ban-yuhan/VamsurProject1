using UnityEngine;

public class DropOnDeath : MonoBehaviour
{
    [SerializeField]
    private GameObject experienceOrbPrefab;

    [SerializeField]
    private int count = 1; //생성할 개수

    [SerializeField]
    private float scatterRadius = 0.5f; //여러개의 경험치구슬이 떨어질 때 겹치치 않도록 하는 생성 반경

    public void SpawnDrops()
    {
        if (experienceOrbPrefab == null)
        {
            return;
        }

        int randomCount = Random.Range(1, count + 4);

        for (int i = 0; i < randomCount; ++i)
        {
            Vector2 offset = Random.insideUnitCircle * scatterRadius; //반지름을 scatterRadius로 하는 원 내의 Random 위치를 offset에 저장
            Vector3 pos = transform.position + new Vector3(offset.x, offset.y, 0.0f);
            Instantiate(experienceOrbPrefab, pos, Quaternion.identity);
        }
    }
}
