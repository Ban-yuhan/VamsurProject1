using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Rigidbody2D body; //물리기반 이동방식을 위해  rb컴포넌트가 필요
    
    public SpriteRenderer sr;

    public GameManager gameManager;

    private Vector2 inputDirection;

    private Vector2 knockbackVelocity;
    public float knockbackDamping = 8.0f;

    [SerializeField]
    private AnimationController animController;
    
    void Update()
    {
        bool canMove = false;
        if(gameManager != null)
        {
            canMove = gameManager.IsPlaying();
        }

        if (canMove == true)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            Vector2 raw = new Vector2(x, y);
            if (raw.sqrMagnitude > 1.0f)
            {
                raw = raw.normalized; //벡터의 정규화 (크기를 1로 맞춤)
            }

            inputDirection = raw;
        }
        else
        {
            
            inputDirection = Vector2.zero; // → 제로 벡터로 만듦
            //또는 inputDirection = new Vector2(0.0f, 0.0f); 
        }
        
        if (animController != null)
        {
            animController.PlayIdleOrMove(inputDirection.magnitude); //inputDirection의 크기를 넘겨줌
        }

        PlayerHead();
    }


    private void FixedUpdate()
    {
        Vector2 targetVelocity = inputDirection * moveSpeed;

        if (knockbackVelocity.sqrMagnitude > 0.0001f)
        {
            float damp = Mathf.Exp(-knockbackDamping * Time.fixedDeltaTime);
            knockbackVelocity = knockbackVelocity * damp;
        }
        else
        {
            knockbackVelocity = Vector2.zero;
        }

        Vector2 finalVel = targetVelocity + knockbackVelocity;
        //body.linearVelocity = targetVelocity;

        body.linearVelocity = finalVel;
    }

    public void SetMoveSpeed(float value)
    {
        moveSpeed = value;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    void PlayerHead()
    {
        if (body.linearVelocity.x < 0f)
        {
            sr.flipX = true;

        }
        else if(body.linearVelocity.x > 0f)
        { 
            sr.flipX= false;
        }
       
    }

    public void AddKnockback(Vector2 force)
    {
        knockbackVelocity += force;
    }
}
