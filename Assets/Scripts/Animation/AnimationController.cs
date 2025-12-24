using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private string IdleOrMoveParameter;

    [SerializeField]
    private string deathParameter;


    public void PlayIdleOrMove(float Move)
    {
        if (animator != null)
        {
            bool isMove = Move == 0.0f ? false : true; //move가 0이면 false, 아니면 true
            animator.SetBool(IdleOrMoveParameter, isMove);
        }
    }

    public void PlayDeath() //반복 재생이 아닌 한 번만 재생 → trigger로 재생
    {
        if (animator != null)
        {
            animator.SetTrigger(deathParameter);
        }
    }

}
