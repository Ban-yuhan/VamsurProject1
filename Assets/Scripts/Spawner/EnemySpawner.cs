using UnityEngine;
using System.Collections.Generic; //List 자료구조를 사용하기 위해 추가해야하는 전처리문

public class EnemySpawner : MonoBehaviour
{

    [SerializeField]
    private Transform playerTransform; //플레이어 기준 최소 최대거리 측정을 위한 플레이어 위치정보

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private SpawnDifficultyScaler scaler;

    [SerializeField]
    private List<GameObject> enemyPrefabs = new List<GameObject>(); //빈 공간 미리 할당

    private float spawnBudget; //누적 스폰포인트(초당 스폰 양을 시간에 곱해 축적)
    private int aliveEnemies; //현재 살아있는 적의 수 저장


    private void Awake()
    {
        if(gameManager == null)
        {
            gameManager = FindAnyObjectByType<GameManager>();
        }

        if(playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if(playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }

        if(scaler == null)
        {
            scaler = GetComponent<SpawnDifficultyScaler>(); //오브젝트가 가진 컴포넌트중 <>에 해당하는 컴포넌트를 가져옴 → 현재 스크립트와 같은 오브젝트에 부착됨
        }

        spawnBudget = 0.0f;
        aliveEnemies = 0;

    }


    void Update()
    {
        bool canSpawn = false;
        if (gameManager != null)
        {
            canSpawn = gameManager.IsPlaying();
        }

        if(canSpawn == false)
        {
            return;
        }

        if (playerTransform == null || scaler == null)
        {
            return;
        }

        float baseRate = scaler.GetBaseSpawnRatePerSecond();
        float growthPerMin = scaler.GetSpawnRateGrowthPerminute();

        float elapsed = gameManager.GetElapsedPlayTime();
        float elapsedMinutes = elapsed / 60.0f; //분단위로 환산

        float effectiveRate = baseRate + growthPerMin * elapsedMinutes; //초당 스폰 량

        float add = effectiveRate * Time.deltaTime;
        spawnBudget += add;

        int cap = scaler.GetMaxAliveEnemises();
        if(aliveEnemies >= cap)
        {
            return;
        }

        while (spawnBudget >= 1.0f && aliveEnemies < cap)
        {
            bool success = SpawnOneEnemy();
            if (success == true)
            {
                --spawnBudget;
                ++aliveEnemies;
            }
            else 
            {
                break;
            }
        }
    }


    bool SpawnOneEnemy()
    {
        //enemyPrefabs가 제대로 설정되어있지않으면 아무 작업도 하지 않음
        if (enemyPrefabs == null || enemyPrefabs.Count == 0)
        {
            return false;
        }

        int index = Random.Range(0, enemyPrefabs.Count);
        GameObject prefab = enemyPrefabs[index];
        if (prefab == null)
        {
            return false;
        }

        Vector3 center = playerTransform.position;
        Vector2 spawnPos = GetRandomPointOnRing(center, scaler.GetMinSpawnRadius(), scaler.GetMaxSpawnRadius());

        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity); //메모리 부족 등의 문제로 적이 제대로 생성되지 않을 수 있음
        if (enemy == null) //enemy가 잘 생성됐는지 체크
        {
            return false;
        }

        EnemyLifetimeHook hook = enemy.GetComponent<EnemyLifetimeHook>();
        if (hook != null)
        {
            hook.SetSpawner(this); //this 파라미터 : 클래스 자기 자신을 파라미터로 전달
        }

        return true;
    }


    Vector2 GetRandomPointOnRing(Vector3 center, float minRadius, float maxRadius)
    {
        float minR = Mathf.Max(0.0f, minRadius); //0.0f와 minRadius중에 더 큰 값을 반환하여 변수에 저장
        float maxR = Mathf.Max(minR + 0.1f, maxRadius);

        //방향과 반지름을 랜덤으로 결정
        float angle = Random.Range(0.0f, 360.0f); //0~360중 랜덤 값을 저장
        float rad = angle * Mathf.Deg2Rad;

        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        float radius = Random.Range(minR, maxR);

        Vector2 pos = (Vector2)center + dir * radius;

        return pos;
    }


    public void NotifyEnemyDies() //적이 한 마리 죽을 때 마다 해당 함수 실행
    {
        if (aliveEnemies > 0)
        {
            --aliveEnemies;
        }
    }

}
