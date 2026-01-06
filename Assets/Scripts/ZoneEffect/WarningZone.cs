using UnityEngine;

public class WarningZone : BaseZone
{
    protected override void ApplyEffect()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, Radius, targetMask);
        if(hits != null && hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; ++i)
            {
                Collider2D hit = hits[i];

                IDamageable damageable = hit.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.ApplyDamage(valueDamage);
                }
            }
        }
    }
}
