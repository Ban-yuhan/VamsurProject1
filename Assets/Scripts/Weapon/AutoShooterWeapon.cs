using System.Collections.Generic;
using UnityEngine;

public class AutoShooterWeapon :  WeaponBase //부모클래스에서 선언한 추상함수를 구현하지 않으면 오류가 발생(→ 추상함수는 반드시 구현되어야함)
{
    [SerializeField]
    private float targetSearchRadius = 8.0f;

    [SerializeField]
    private LayerMask targetLayerMask;

    [SerializeField]
    private float spreadDegrees = 10.0f;

    private float rotatingAngle; //타겟이 없을때 사용할 회전 각도

    protected override void Awake()
    {
        base.Awake(); //부모 클래스의 Awake() 호출
        rotatingAngle = 0.0f;
    }


    protected override void AcquireFireDirections(out List<Vector2> directions)
    {
        directions = new List<Vector2>();

        Vector2 aim = GetAimDirection();//목표 방향 계산
        if(aim.sqrMagnitude <= 0.0001f)//방향을 제대로 설정하지 못했다면
        {
            aim = Vector2.right;
        }

        int count = GetProjectileCount();
        float baseAngle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
        
        if(count <= 1)
        {
            directions.Add(aim.normalized);
            return;
        }

        float start = baseAngle - spreadDegrees * (count - 1) * 0.5f; //처음 시작 각도 계산

        for(int i = 0; i < count; ++i)
        {
            float angle = start + spreadDegrees * i;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            directions.Add(dir.normalized);
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

        rotatingAngle -= 90.0f * Time.deltaTime; //초당 90도씩 회전
        float rad = rotatingAngle * Mathf.Deg2Rad; //각도를 라디안 값으로 변환 (프로그래밍에선 항상 라디안으로 변경한 후 값을 사용하는 경우가 대다수)
        Vector2 fallback = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)); //cos는 x위치, sin은 y위치 지정 가능

        return fallback.normalized;
    }


    Transform FindNearestTarget() //가장 가까운 적을 찾는 기능 구현
    {
        Collider2D[] hits;

        //플레이어 position으로 부터 targetSearchRadius를 반지름으로 가진 원 반경에 targetLayerMask를 가진 적 탐지
        hits = Physics2D.OverlapCircleAll(transform.position, targetSearchRadius, targetLayerMask);

        if (hits == null || hits.Length == 0)
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
}
