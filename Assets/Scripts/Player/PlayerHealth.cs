using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable, IRecover
{
    [SerializeField]
    private float maxHealth = 100f;

    [SerializeField]
    private float currentHealth;

    [SerializeField]
    private float invulnerableTime = 0.8f;

    private float invulnerableTimer;

    private float HealColorTime = 0.8f;

    private float HealColorTimer;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Color normalColor = Color.white;

    [SerializeField]
    private Color hitColor = Color.red;

    [SerializeField]
    private Color healColor = Color.green;

    [SerializeField]
    private Rigidbody2D body;

    [SerializeField]
    private float knockbackDamper = 0.9f; //히트시 얼마나 넉백 될 것인지

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private AnimationController animController;


    private void Awake()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (invulnerableTimer > 0f)
        {
            invulnerableTimer -= Time.deltaTime;
            if (invulnerableTimer <= 0f)
            { 
            EndInvulnerability();
            }
        }

        if (HealColorTimer > 0f)
        {
            HealColorTimer -= Time.deltaTime;
            if (HealColorTimer <= 0f)
            {
                EndHealColor();
            }
        }

        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    ApplyDamage(10);

        //}
    }


    public void ApplyDamage(float amount)
    {
        if (gameManager != null)
        {
            if (gameManager.IsPlaying() == false)
            {
                return;
            }
        }

        if (amount <= 0f)
        {
            return;
        }

        if(invulnerableTimer > 0f)
        {
            return;
        }

        currentHealth -= amount;

        PlayHitFeedback();

        BeginInvulnerability();

        if (currentHealth < 0f)
        {
            currentHealth = 0f;
            HandleDeath();
        }
    }


    public void ApplyKnockback(Vector2 force)
    {

        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.AddKnockback(force);
        }
        else
        {
            //body.linearVelocity = body.linearVelocity * knockbackDamper;
            body.AddForce(force, ForceMode2D.Impulse); //rigidbody가 제공하는 함수. 정해진 방향으로 한 번만 힘을 가함
        }
    }


    void HandleDeath()
    {
        if (gameManager != null)
        {
            gameManager.HandleGameOver();
        }

        if (animController != null)
        {
            animController.PlayDeath();
        }
    }


    void BeginInvulnerability()
    {
        invulnerableTimer = invulnerableTime;

    }


    void EndInvulnerability()
    {
        invulnerableTimer = 0.0f;        
        spriteRenderer.color = normalColor;
    }


    void BeginHealColor()
    {
        HealColorTimer = HealColorTime;
    }


    void EndHealColor()
    {
        HealColorTimer = 0.0f;
        spriteRenderer.color = normalColor;
    }

    
    void PlayHitFeedback()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = hitColor;
        }
    }

    void PlayHealFeedback()
    {
        if(spriteRenderer != null)
        {
            spriteRenderer.color = healColor;
        }
    }   
    


    public void SetMaxHealth(float value)
    {
        maxHealth = value;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }


    public float GetCurrentHealth()
    {
        return currentHealth;
    }


    public void SetCurrentHealth(float value)
    {
        currentHealth = value;
    }


    public float GetMaxHealth()
    { 
        return maxHealth;
    }

    public void Heal(float value)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth = Mathf.Min(currentHealth + value, maxHealth);
            PlayHealFeedback();
            BeginHealColor();
        }
    }
}
