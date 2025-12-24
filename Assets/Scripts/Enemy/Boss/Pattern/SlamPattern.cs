using UnityEngine;
using System.Collections;

/// <summary>
/// 보스 주변 원형 범위 강타.
/// </summary>
public class SlamPattern : AttackPatternBase
{
    [SerializeField]
    private float radius = 3.5f;

    [SerializeField]
    private float damage = 18.0f;

    [SerializeField]
    private float knockback = 8.0f;

    [SerializeField]
    private LayerMask targetMask; //피해를 줄 오브젝트의 레이어 값

    protected override IEnumerator Execute()
    {
        Collider2D[] hits;
        if (targetMask.value != 0)
        {
            hits = Physics2D.OverlapCircleAll(transform.position, radius, targetMask); //레이어에 지정된 오브젝트가 반경안에 들어왔는지 체크.
        }
        else
        {
            hits = Physics2D.OverlapCircleAll(transform.position, radius); //target마스크에 레이어 지정을 하지 않으면, 피아식별 없이 모두 데미지 적용
        }

        if (hits != null) //들어온 대상이 있으면 실행
        {
            for (int i = 0; i < hits.Length; i = i + 1) //플레이어가 하나이기때문에 for문을 한 번만 실행
            {
                DamageRouter router = hits[i].GetComponent<DamageRouter>();
                if (router != null)
                {
                    DamageContext ctx = new DamageContext();
                    ctx.baseDamage = damage;
                    ctx.canCrit = false;
                    ctx.knockbackForce = knockback;
                    ctx.attacker = gameObject;

                    router.Receive(ctx);
                }
            }
        }

        HitstopManager hs = FindAnyObjectByType<HitstopManager>(); //HitstopManager를 불러와서 잠시 일시정지
        if (hs != null)
        {
            hs.DoHitstop(0.08f);
        }

        CameraShaker shaker = FindAnyObjectByType<CameraShaker>(); //카메라 흔들림 실행
        if (shaker != null)
        {
            shaker.Shake(0.25f, 0.2f);
        }

        yield return null;
    }

    private void OnDrawGizmosSelected() //반경을 볼 수 있게 표시해주는 함수
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
