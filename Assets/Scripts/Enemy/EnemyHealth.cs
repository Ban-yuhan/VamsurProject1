using UnityEngine;
using System.Collections;
public class EnemyHealth : MonoBehaviour, IDamageable, IRecover, IReceivesHitContext
{
    public float maxHealth = 100f;
    public SpriteRenderer spriteRenderer;
    public float hitFlashTime = 0.05f;

    public DropOnDeath drop;


    private float currentHealth;
    private float hitFlashTimer;

    private Color originalColor;

    private WaveDirector waveDirector;

    [SerializeField]
    private AnimationController animController;

    [SerializeField]
    private EnemyChase chase;


    //==================12.10 수정=================================
    private bool hasLastContext; //최근 문맥 보유 여부
    private HitContext lastContext; //최근 문맥 저장

    [SerializeField, Range(0.0f, 0.9f)] //serializeField 옆에 Range()를 지정하면, 해당 변수는 해당 Range내의 값으로만 수정 가능
    private float resistance = 0.1f; //피해 저항
    //=============================================================


    private void Awake()
    {
        currentHealth = maxHealth;
        if(spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        waveDirector = GameObject.FindAnyObjectByType<WaveDirector>();


        //==================12.10 수정=================================
        hasLastContext = false; //문맥 보유 초기화
        //=============================================================
    }


    void Update()
    {
        if(hitFlashTimer < hitFlashTime)
        {
            hitFlashTimer += Time.deltaTime;

            if (hitFlashTimer >= hitFlashTime)
            { 
            RestoreColor();
            }
        }
    }

    public void ApplyDamage(float amount)
    {
        if(currentHealth <= 0)
        {
            return;
        }

        //======================12.10 수정=======================================
        HitContext context; //사용할 문맥
        if(hasLastContext == true)
        {
            context = lastContext; //최근 문맥 사용
            hasLastContext= false; //사용 후 해제
        }
        else
        {
            context = HitContext.Create(null, transform, transform.position, amount, DamageType.Physical); //기본 문맥 합성
        }

        float finalDamage = DamageResolver.ComputeFinalDamage(context, resistance, amount); //최종 피해 계산

        currentHealth -= finalDamage;
        
        //=======================================================================

        //currentHealth -= amount;

        PlayHitFlash();

        //======================12.10 수정=======================================

        DamageAppliedEvent e = new DamageAppliedEvent(); // 피해확정 알림 데이터
        e.context = context; //문맥
        e.finalDamage = finalDamage; //최종 피해
        e.remainingHp = currentHealth; //남은 체력
        EventBus.PublishDamageApplied(e); //피해확정 알림 발행

        //=======================================================================


        if (currentHealth <= 0f) 
        {
            Die();
        }
    }


    void Die()
    {
        if(drop != null )
        {
            drop.SpawnDrops();
        }

        if(waveDirector != null)
        {
            waveDirector.ReportKill(); //다음 웨이브에 필요한 킬 카운트 조정
        }

        if (animController != null)
        {
            animController.PlayDeath();
        }

        StartCoroutine(CoDestroy());

    }

    IEnumerator CoDestroy()
    {
        chase.SetMoveSpeed(0);

        yield return new WaitForSeconds(1.0f);

        Destroy(gameObject);

    }

    void PlayHitFlash()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.gray;
            hitFlashTimer = 0f;
        }
    }

    void RestoreColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    public void SetMaxHealth(float value)
    {
        maxHealth = value;
        currentHealth = maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }


    public void SetCurrentHealth(float newHealth)
    {
        currentHealth = newHealth;
    }


    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void Heal(float value)
    {
        currentHealth = Mathf.Min(currentHealth + value, maxHealth);
    }


    /// <summary>
    /// 다음 피해 적용에서 사용할 히트 문맥을 저장한다
    /// </summary>
    public void SetHitContext(HitContext context)
    {
        lastContext = context; //문맥 저장
        hasLastContext = true; //보유 표시
    }



}
