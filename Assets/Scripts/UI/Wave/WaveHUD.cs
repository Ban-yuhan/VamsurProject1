using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 웨이브 HUD 표시(웨이브 번호, 다음 웨이브까지 남은 시간)
/// - 표시만 담당
/// </summary>

public class WaveHUD : MonoBehaviour
{
    [SerializeField]
    private Text waveText; // 현재 웨이브 정보 출력 텍스트 ex) "Wave 3"

    [SerializeField]
    private Text timerText; //다음 웨이브까지 남은 시간 출력 텍스트 ex) "Next in 12.3s"
    
    //모든 Text는 TMP가 아닌 기본 Text 사용
    
    public GameState currentState;

    private float pendingTimer;

    ///<summary>
    ///현재 웨이브 번호를 갱신
    /// </summary>
    public void SetWave(int wave)
    {
        if (waveText != null)
        {
            waveText.text = "Wave " + wave;
        }
    }


    ///<summary>
    ///다음 웨이브까지 남은 시간을 갱신(초)
    /// </summary>
    public void SetTimer(float seconds)
    {
        pendingTimer = seconds;
        if(timerText != null)
        {
            timerText.text = "Next in " + pendingTimer.ToString("0.0") + "s"; //ToString에 0.0은 소숫점 첫째 자리까지만 출력하겠다는 의미
        }
    }

    ///<summary>
    ///UI를 부드럽게 보이게 하기 위해 unscaledDeltaTime으로 숫자만 미세 업데이트
    /// </summary>
    private void Update()
    {
        //표시 부드러움 용도.
        if (pendingTimer > 0.0f && currentState == GameState.Playing)
        {
            pendingTimer = Mathf.Max(0.0f, pendingTimer - Time.unscaledDeltaTime);
            if (timerText != null)
            {
                timerText.text = "Next in " + pendingTimer.ToString("0.0") + "s";
            }
        }
    }
}
