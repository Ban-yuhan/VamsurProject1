using UnityEngine;

public abstract class BaseZone : MonoBehaviour
{
    [SerializeField]
    protected float valueHeal = 10.0f; //회복에 적용할 수치

    [SerializeField]
    protected float valueDamage = 5f; //데미지 적용 수치


    [SerializeField]
    protected float Cooldown = 1.0f; //적용 쿨타임

    [SerializeField]
    protected float Radius = 1.0f; //체크할 반경

    [SerializeField]
    protected LayerMask targetMask; //대상 레이어마스크

    protected float Timer = 0.0f;

    protected abstract void ApplyEffect();

    protected void Update()
    {
        if (Timer < Cooldown)
        {
            Timer += Time.deltaTime;

            if (Timer > Cooldown)
            {
                ApplyEffect();
                Timer = 0.0f;
            }
        }
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
