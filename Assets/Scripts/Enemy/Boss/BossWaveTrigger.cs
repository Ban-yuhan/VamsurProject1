using UnityEngine;

/// <summary>
/// 정해진 시각에 보스 몬스터를 스폰한다.
/// 해당 보스가 사망하면 게임 클리어 상태로 전환한다.(선택적 기능)
/// 스폰 위치는 SafeSpawnRingProvider 기능을 이용해서 저장.
/// 
/// </summary>
public class BossWaveTrigger : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager; //게임 상태 전환을 위한 참조용 변수

    [SerializeField]
    private Transform player;

    [SerializeField]
    private GameObject bossPrefab;

    [SerializeField]
    private SafeSpawnRingProvider safeProvider;

    [SerializeField]
    private float spawnTimeSeconds = 120f; //보스가 출현하는 시간

    [SerializeField]
    private Vector2 fallbackOffset = new Vector2(13.0f, 0.0f); //보스 소환 임시 위치

    private bool spawned = false; //중복 소환을 막기 위한 변수
    private Transform bossInstance;


    private void OnEnable()
    {
        spawned = false;
        bossInstance = null;
        EventBus.OnDeath += OnDeath;
    }


    private void OnDisable()
    {
        EventBus.OnDeath -= OnDeath;
    }


    private void Update()
    {
        if(spawned == true)
        {
            return;
        }

        if(gameManager == null)
        {
            return;
        }

        if(player == null)
        {
            return;
        }

        if (gameManager.currentState != GameState.Playing)
        {
            return;
        }


        float t = gameManager.GetElapsedPlayTime(); //게임 플레이 경과 시간을 가져옴
        if(t >= spawnTimeSeconds)
        {
            TrySpawnBossOnce();
        }
    }


    /// <summary>
    /// 사망 이벤트를 받아, 사망한 대상이 보스인 경우 게임 클리어 상태로 전환 처리.
    /// </summary>
    /// <param name="deathEvent">사망 이벤트 정보를 담고 있는 구조체</param>
    void OnDeath(DeathEvent deathEvent)
    {
        if(gameManager == null)
        {
            return;
        }

        if (bossInstance == null)
        {
            return;
        }

        if(deathEvent.victim == bossInstance) //보스가 사망한 이벤트가 날아온 경우
        {
            if(gameManager.currentState == GameState.Playing)
            {
                gameManager.handleGameClear();
            }
        }
    }


    /// <summary>
    /// 보스 몬스터를 소환할 위치를 뽑는다.
    /// 기본적으로 화면 바깥 영역에서 좌표를 뽑는 처리 진행.
    /// 그렇지 못할 경우 플레이어의 위치에서 offset만큼 떨어진 위치를 반환
    /// </summary>
    /// <returns>스폰할 위치</returns>
    Vector3 GetSpawnPosition()
    {
        if(safeProvider != null)
        {
            Vector3 p;
            bool ok = safeProvider.TrySample(out p);

            if(ok == true)
            {
                return p;
            }
        }

        //safeProvider가 null일 경우 임시 위치에서 보스 소환(플레이어 위치 + fallbackoffset)
        Vector3 basePos = player.position;
        Vector3 offset = new Vector3(fallbackOffset.x, fallbackOffset.y, 0.0f);
        return basePos + offset;
    }


    /// <summary>
    /// 보스를 한 번만 소환하기 위한 함수.
    /// </summary>
    void TrySpawnBossOnce()
    {
        if(spawned == true)
        {
            return; //이미 소환 된 경우 재실행 X
        }

        if(bossPrefab == null) 
        {
            return;
        }

        Vector3 pos = GetSpawnPosition();
        GameObject go = Instantiate(bossPrefab, pos, Quaternion.identity);
        if (go != null)
        {
            bossInstance = go.transform;
            spawned = true;
        }
    }
}
