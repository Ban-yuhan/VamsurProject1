using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// 짧은 히트 스탑을 구현한다. 중복 요청 시 더 긴 남은 시간으로 갱신.
/// </summary>
public class HitstopManager : MonoBehaviour
{
    public bool isActive;
    public float remain;

    
    void Update()
    {
        if(isActive == true)
        {
            remain -= Time.unscaledDeltaTime; //TimeScale을 0으로 만들기 때문에 Time.deltaTime을 사용할 수 없음 → UnscaledDeltaTime사용
            if(remain <= 0.0f)
            {
                End();
            }
        }
    }


    public void DoHitstop(float duration)
    {
        if(duration <= 0.0f)
        {
            return;
        }

        if (isActive == false)
        {
            StartCoroutine(HitstopRoutine(duration));
        }
        else
        {
            remain = Mathf.Max(remain, duration);
        }
    }


    private IEnumerator HitstopRoutine(float duration)
    {
        isActive = true;
        remain = duration;

        float prev = Time.timeScale; //원래의 timeScale 저장
        Time.timeScale = 0f; //완전 정지(필요 시 0.05 등으로 변경)

        //UI 등은 UnscaledTime으로 갱신되므로 영향X
        while (remain > 0.0f)
        {
            yield return null;
        }

        Time.timeScale = prev;
        isActive = false;
    }


    private void End()
    {
        remain = 0.0f;
    }
}
