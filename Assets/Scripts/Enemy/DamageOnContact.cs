using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    [SerializeField]
    private float contactDamage = 5.0f;

    [SerializeField]
    private float damageInterval = 0.8f; //데미지를 입히는 간격

    [SerializeField]
    private float knockbackForce = 4.0f;

    private float damageCooldown;

    [SerializeField]
    private BoxCollider2D col2D;

  
    void Update()
    {
        if (damageCooldown > 0.0f)
        {
            damageCooldown -= Time.deltaTime;
            
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        bool useTrigger = col2D.isTrigger;
        if (useTrigger == false)
        {
            TryDamage(collision.collider); //파라미터를 collision.collider를 전달해야함
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        bool useTrigger = col2D.isTrigger;
        if (useTrigger == true)
        {
            TryDamage(collision); //함수의 파라미터를(지금은 collision) 넣어주면 됨
        }
    }


    void TryDamage(Collider2D col)
    {
        if(col == null)
        {
            return;
        }

        if (damageCooldown > 0.0f)
        {
            return;
        }

        if (col.CompareTag("Player") == false)
        {
            return;
        }

        //IDamageable damageable = col.GetComponent<IDamageable>(); 
        PlayerHealth player = col.GetComponent<PlayerHealth>();
        if (player != null)
        {
            //damageable.ApplyDamage(contactDamage); //Player가 IDamageable을 상속받은 상태이기 때문에 IDamageable의 ApplyDamage함수로 Player에게 데미지를 입힘
            player.ApplyDamage(contactDamage);
            //PlayerHealth player = col.GetComponent<PlayerHealth>();

            Vector2 dir = (col.transform.position - transform.position).normalized;
            Vector2 force = dir * knockbackForce; //방향 * 크기

            player.ApplyKnockback(force);

            damageCooldown = damageInterval;
        }
    }
}
