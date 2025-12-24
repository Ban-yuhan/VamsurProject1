using UnityEngine;
using System.Collections.Generic; //리스트 사용 네임스페이스

/// <summary>
/// 플레이어 주변에 칼날 여러개를 만들고 원형으로 회전.
/// 칼날이 적과 닿으면 데미지 적용
/// 너무 자주 데미지를 주지 않기 위해 적 별 쿨다운 적용.
/// </summary>
public class OrbitingBladesWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject bladePrefap;

    [SerializeField]
    private int bladesCount = 5; //생성할 칼날 개수

    [SerializeField]
    private float radius = 2.0f; //칼날을 생성할 플레이어 주위 반경

    [SerializeField]
    private float rotationSpeedDeg = 180.0f; //초당 회전속도(각도단위)

    [SerializeField]
    private float damage = 2.0f; //적 타격 시 적용할 데미지

    [SerializeField]
    private float hitCooldownSee = 0.2f; //데미지 적용 쿨다운

    private List<Transform> blades = new List<Transform>(); //생성한 칼날의 위치 정보를 담을 리스트
    private Dictionary<Transform, float> lastHitTimeByTargetId = new Dictionary<Transform, float>(); //적 별로 마지막으로 명중한 시각. 쿨다운을 따로 적용하기 위함.


    private void Awake()
    {
        blades.Clear();
        lastHitTimeByTargetId.Clear();

        for(int i = 0; i < bladesCount; ++i)
        {
            GameObject blade = Instantiate(bladePrefap, transform.position, Quaternion.identity);
            if(blade != null ) 
            {
                blade.transform.SetParent(transform, true);

                BladeHitReceiver receiver = blade.GetComponent<BladeHitReceiver>();
                if(receiver != null)
                {
                    receiver.Setup(this);
                }

                blades.Add(blade.transform);
            }
        }
    }


    private void Update()
    {
            
    }


    public void TryDealDamage(EnemyHealth enemyHealth)
    {
        Transform trans = enemyHealth.transform;
        float now = Time.time; //현재 시간을 저장
        float lastTime;

        bool hasTime = lastHitTimeByTargetId.TryGetValue(trans, out lastTime);

        if (hasTime == true)
        {
            if(now - lastTime > hitCooldownSee)
            {
                return;
            }
        }

        lastHitTimeByTargetId[trans] = now;
        enemyHealth.ApplyDamage(damage);

    }
}
