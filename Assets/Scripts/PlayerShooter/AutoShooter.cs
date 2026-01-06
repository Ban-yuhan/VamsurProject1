using UnityEngine;

public class AutoShooter : MonoBehaviour
{

    public float attackCooldown = 0.5f;
    public GameObject projectilePrefab;

    public float projectileSpeed = 10.0f;
    public float projectileLifeTime = 3.0f;
    public float damage = 5.0f;
    public int projectileCount = 1;


    public float targetSearchRadius = 8.0f; //적 탐지 범위(반지름)
    public LayerMask targetLayerMask;
    public GameManager gameManager;

    private float cooldownTimer;
    private float rotatingAngle;

    public Transform firePoint; //발사 위치


    private void Awake()
    {
        cooldownTimer = 0.0f;
        rotatingAngle = 0.0f;
    }


    void Update()
    {
        bool canFire = false;
        if(gameManager != null)
        {
            canFire = gameManager.IsPlaying();
        }

        if (canFire == false)
        {
            return;
        }

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0.0f)
        { 
            FireOneShot();
            cooldownTimer = attackCooldown;
        }
    }


    void FireOneShot()
    {
        Vector3 spawnPos = firePoint.position;

        Vector2 dir = GetAimDirection();
        float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        int count = Mathf.Max(1, projectileCount);
        float spread = 10.0f;
        float start = baseAngle - spread * (count - 1) * 0.5f;

        for (int i = 0; i < count; ++i)
        {
            float angle = start + spread * i;
            Vector2 shotDir = new Vector2(Mathf.Cos(angle*Mathf.Deg2Rad), Mathf.Sin(angle*Mathf.Deg2Rad));

            if (projectilePrefab != null)
            {
                GameObject obj = Instantiate(projectilePrefab, spawnPos, Quaternion.Euler(0.0f, 0.0f, angle - 90f));

                //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                //obj.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle - 90f); //총알이 발사될 때 타겟의 방향으로 회전하며 나아감 (타겟 방향을 바라보도록 회전)

                Projectile proj = obj.GetComponent<Projectile>();
                if (proj != null)
                {
                    proj.SetDirection(shotDir);
                    proj.Configure(projectileSpeed, projectileLifeTime, damage); //총알의 능력치 수정
                }
            }
        }       
    }


    Vector2 GetAimDirection()
    {
        Transform nearest = FindNearestTarget();

        if (nearest != null)
        {
            Vector2 toTarget = nearest.position - transform.position;
            return toTarget.normalized; // 방향정보 반환
        }

        rotatingAngle += 90.0f * Time.deltaTime; //초당 90도씩 회전
        float rad = rotatingAngle * Mathf.Deg2Rad; //각도를 라디안 값으로 변환 (프로그래밍에선 항상 라디안으로 변경한 후 값을 사용하는 경우가 대다수)
        Vector2 fallback = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)); //cos는 x위치, sin은 y위치 지정 가능
        
        return fallback.normalized;
    }
    

    Transform FindNearestTarget() //가장 가까운 적을 찾는 기능 구현
    {
        Collider2D[] hits;

        //플레이어 position으로 부터 targetSearchRadius를 반지름으로 가진 원 반경에 targetLayerMask를 가진 적 탐지
        hits = Physics2D.OverlapCircleAll(transform.position, targetSearchRadius, targetLayerMask);

        if(hits == null || hits.Length == 0)
        {
            return null;
        }

        float bestDist = float.MaxValue; //최종적으로 가장 가까운 적의 거리정보 저장
        Transform best = null;

        for (int i = 0; i < hits.Length; ++i)
        {
            Transform t = hits[i].transform; //t 변수에 검색된 적의 transform 정보 저장
            //float dist = (t.position - transform.position).sqrMagnitude; // (적위치 - 플레이어위치)제곱 → dist변수에 저장 : 플레이어와 적 사이의 거리계산
            float dist = Vector2.Distance(t.position, transform.position); //두 오브젝트 사이의 거리를 구해주는 유니티 자체 제공
            
            if (dist < bestDist) //기존 가장 가까운 적의 거리보다 새로 감지된 적의 거리및 transform이(가) 더 가까우면 갱신
            {
                bestDist = dist;
                best = t;
            }
        }

        return best;
    }


    public float GetAttackCooldown()
    {
        return attackCooldown;
    }

    public void SetAttackCooldown(float cooldown)
    {
        attackCooldown = cooldown;
    }

    public float GetDamage()
    {
        return damage;
    }

    public void SetDamage(float value)
    {
        damage = value;
    }

    public float GetProjectileSpeed()
    {
        return projectileSpeed;
    }

    public void SetProjectileSpeed(float speed)
    {
        projectileSpeed = speed;
    }

    public int GetProjectileCount()
    {
        return projectileCount;
    }

    public void SetProjectileCount(int count)
    {
        projectileCount = count;
    }
}
