using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public Transform target;
    public Rigidbody2D body;
    public float stopDistance = 0.1f;

    public SpriteRenderer sr;
    public EnemyHealth health;
    
    private GameManager gameManager;


    private void Awake()
    {
        gameManager = GameObject.FindAnyObjectByType<GameManager>();
    }


    void Start()
    {
        if (target == null)
        {
            GameObject playerobj = GameObject.FindGameObjectWithTag("Player");
            if (playerobj != null)
            {
                target = playerobj.transform;
            }
        }
    }


    private void FixedUpdate()
    {
        bool canMove = false;
        if (gameManager != null)
        {
            canMove = gameManager.IsPlaying();
        }

        if (canMove == false)
        {
            return;
        }

        if(target == null)
        {
            return;
        }

        Vector2 toTarget = (Vector2)(target.position - transform.position);
        float dist = toTarget.magnitude; //magnitude : Vector의 크기(거리)를 계산

        if (dist <= stopDistance)
        {
            return;
        }

        Vector2 dir =  toTarget.normalized; //또는 Vector2 dir = toTarget / dist; 라고 해도 됨.(dist는 크기. 크기+방향값을 갖는 Vector에서 크기를 나눠주면 크기 1에 방향을 나타내는 벡터가 됨)

        Vector2 current = body.position;
        Vector2 next = current + (dir * moveSpeed * Time.fixedDeltaTime);
        body.MovePosition(next); //rigidbody2D에서 제공하는 MovePosition 함수


        float curHP = health.GetCurrentHealth();
        if (curHP > 0)
        {
            if (dir.x < 0)
            {

                sr.flipX = true;
            }
            else if (dir.x > 0)
            {

                sr.flipX = false;
            }
        }
    }


    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }


    public void SetMoveSpeed(float value)
    {
        moveSpeed = value;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

}
