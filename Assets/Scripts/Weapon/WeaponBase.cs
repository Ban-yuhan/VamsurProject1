using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 추상 클래스(abstract)
///  - 추상 클래스 타입의 오브젝트 생성 불가
///  - 단, 추상 클래스 타입의 변수에  상속받은 자식 클래스의 객체를 대입하는 것은 가능
/// </summary>
public abstract class WeaponBase : MonoBehaviour 
{

    [SerializeField]
    private float attackCooldown = 0.6f;

    [SerializeField]
    private float damage = 1.0f;

    [SerializeField]
    private float projectileSpeed = 10.0f;

    [SerializeField]
    private float projectileLifeTime = 3.0f;

    [SerializeField]
    private int projectileCount = 1;

    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private GameObject projectilePrefab;

    //===========================
    [SerializeField]
    private PoolManager poolManager;

    [SerializeField]
    private string projectileKey = "Projectile";  //key로 사용할 문자열

    //===========================

    protected GameManager gameManager;

    private float cooldownTimer;


    protected virtual void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        cooldownTimer = 0.0f;
    }


    public void SetProjectilePrefab(GameObject prefab)
    {
        projectilePrefab = prefab;
    }


    public void SetFirePoint(Transform t)
    {
        firePoint = t;  
    }


    public Transform GetFirePoint()
    {
        return firePoint;
    }


    public void SetProjectileCount(int value)
    {
        projectileCount = value;
    }


    public int GetProjectileCount()
    {
        return projectileCount;
    }


    public void SetProjectileLifeTime(float value)
    {
        projectileLifeTime = value;
    }


    public float GetProjectileLifeTime()
    {
        return projectileLifeTime;
    }


    public void SetProjectileSpeed(float value)
    {
        projectileSpeed = value;
    }


    public float GetProjectileSpeed()
    {
        return projectileSpeed;
    }


    public void SetDamage(float value)
    {
        damage = value;
    }


    public float GetDamage()
    {
        return damage;
    }


    public void SetAttackCooldown(float value)
    {
        attackCooldown = value;
    }


    public float GetAttackCooldown()
    {
        return attackCooldown;
    }


    //추상 함수. 아무런 기능 없이 정의만 되어있음. ∵ 상속받은 자식 클래스마다 다른 함수내용을 가지게 되기 때문에, 함수선언만 해놓고 자식 클래스에서 내용 작성.
    protected abstract void AcquireFireDirections(out List<Vector2> directions); //발사 방향을 계산하는 함수.


    void FireInternal() //접근제한자가 없음 → 기본적으로 private로 지정
    {
        Vector3 spawnPos = firePoint.position;

        List<Vector2> directions = new List<Vector2>();
        AcquireFireDirections(out directions); //out : 참조형 호출. 변수 앞에 쓰면 참조형 변수가 됨.

        if(directions == null || directions.Count == 0)
        {
            return;
        }

        for (int i = 0; i < directions.Count; ++i)
        {
            Vector2 dir = directions[i];
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; //Atan2 : 오브젝트를 회전시키기 위해 사용

            //GameObject projObj = Instantiate(projectilePrefab, spawnPos, Quaternion.Euler(0.0f, 0.0f, angle-90f));
            PooledObject projObj = poolManager.Spawn(projectileKey, spawnPos, Quaternion.Euler(0.0f, 0.0f, angle - 90));
            if (projObj == null)
            {
                continue;
            }

            Projectile proj = projObj.GetComponent<Projectile>();
            if (proj != null)
            {
                proj.SetDirection(dir);
                proj.Configure(projectileSpeed, projectileLifeTime, damage);
                proj.SetOwner(gameObject);
            }
        }
    }


    void Update()
    {
        bool canFire = false;
        if (gameManager != null)
        {
            canFire = gameManager.IsPlaying();
        }

        if (canFire == false)
        {
            return;
        }

        cooldownTimer -= Time.deltaTime;

        if(cooldownTimer <= 0)
        {
            FireInternal();
            cooldownTimer = attackCooldown;
        }
    }
}
