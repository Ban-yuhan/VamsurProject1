using UnityEngine;
using System.Collections.Generic;

public class BoomerangProjectile : MonoBehaviour
{
    [SerializeField]
    private Collider2D hitCollider;

    [SerializeField]
    private float returnFinishDistance = 0.6f; //부메랑이 플레이어에게 돌아왔다고 판달되는 거리

    //값을 부메랑 발사 스크립트에서 세팅할 수 있도록 하기 위해 private로 선언
    private Transform owner; //돌아갈 대상의 위치 정보
    private Vector2 outDirection; //처음 나아갈 방향
    private float moveSpeed;
    private float damage;
    private float maxDistance;
    private float hitCooldownSec;
    private LayerMask enemyLayer;

    private Vector2 startPos; //시작 위치
    private bool returning; //나아가는중인지 돌아오는 중인지 판단

    private Dictionary<Transform, float> lastHitTimeByTarget = new Dictionary<Transform, float>(); //Transform을 Key, 시간 정보를 값으로 사용


    private void Awake()
    {
        lastHitTimeByTarget.Clear();
    }


    private void Update()
    {
        if(returning == false)
        {
            UpdateOutgoint();
        }
        else
        {
            UpdateReturning();
        }

        transform.Rotate(0f, 0f, -4 * 360f * Time.deltaTime);

    }


    private bool CanApplyHit(float now, Transform target)
    {
        if (lastHitTimeByTarget.TryGetValue(target, out float lastTime) == true)
        {
            float delta = now - lastTime;
            if (delta < hitCooldownSec) //쿨타임이 아직 안되었으면
            {
                return false;
            }
        }

        return true;
    }


    void TryHit(Collider2D collision)
    {
        int mask = 1 << collision.gameObject.layer;
        if ((mask & enemyLayer.value) == 0)
        {
            return;
        }

        EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
        if (enemyHealth == null)
        {
            return;
        }

        if (CanApplyHit(Time.time, enemyHealth.transform) == false)
        {
            return;
        }

        lastHitTimeByTarget[enemyHealth.transform] = Time.time;
        enemyHealth.ApplyDamage(damage);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryHit(collision);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        TryHit(collision);
    }


    void UpdateReturning()
    {
        Vector2 to = owner.position - transform.position;

        float distSqr = to.sqrMagnitude; //목표까지의 거리. 단순 비교연산을 할 것이기 때문에 sqrMagnitude를 사용.Magnitude를 사용할 경우 부하가 더 걸림
        float finishSqr = returnFinishDistance * returnFinishDistance;

        if (distSqr <= finishSqr)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 dir = to.normalized;
        Vector2 delta = dir * moveSpeed * Time.deltaTime;
        transform.position += (Vector3)delta;
    }

    void UpdateOutgoint()
    {
        Vector2 delta = outDirection * moveSpeed * Time.deltaTime;
        transform.position += (Vector3)delta;

        Vector2 diff = (Vector2)transform.position - startPos;

        float travelSqr = diff.sqrMagnitude;
        float maxSqr = maxDistance * maxDistance;

        if (travelSqr >= maxSqr)
        {
            returning = true;
        }
    }


    public void Setup(Transform owner, Vector2 direction, float speed, float dmg, float distance, float cooldownSec, LayerMask layer)
    {
        this.owner = owner; //this : 클래스 자신을 의미

        outDirection = direction.normalized;
        moveSpeed = speed;
        damage = dmg;
        maxDistance = distance;
        hitCooldownSec = cooldownSec;
        enemyLayer = layer;

        startPos = transform.position;
        returning = false;

        lastHitTimeByTarget.Clear();

    }
}
