using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameState currentState;

    public float targetplayTime; //클리어 조건이 될 목표 시간

    public KeyCode pauseKey = KeyCode.Escape; //일시 정지를 위한 키 지정.

    public KeyCode restartKey = KeyCode.R;

    public float defaultTimeScale = 1.0f; //기본 타임 스케일

    public UIManager uiManager;

    private float elapsedPlayTime; // 실제로 플레이가 진행된 누적 시간.

    private float savedTimeScale;

    private bool isPlaying;

    [SerializeField]
    private bool InfiniteMode;




    
    void Start()
    {
        currentState = GameState.Ready;
        elapsedPlayTime = 0f;
        isPlaying = false; 

        savedTimeScale = defaultTimeScale;
        Time.timeScale = defaultTimeScale;
    }


    void Update()
    {
        if (currentState == GameState.Playing && isPlaying == true)
        {
            elapsedPlayTime += Time.deltaTime;

            if (elapsedPlayTime >= targetplayTime && InfiniteMode == false)
            {
                handleGameClear();
            }
                       
        }

        if(currentState == GameState.Ready)
        {
            if(Input.GetKeyDown(KeyCode.Space) == true)
            {
                StartGame();
                uiManager.CloseReady();
            }
        }

        /*
        //테스트용 코드. 나중에 지워도 됨
        if(currentState == GameState.Playing)
        {
            if (Input.GetKeyDown(KeyCode.K) == true)
            {
                HandleGameOver();
            }
        }
        */

        if(Input.GetKeyDown(pauseKey) == true)
        {
            TogglePause();
        }

        if(Input.GetKeyDown(restartKey) == true)
        {
            RestartGame();
        }
    }


    public void Pause()
    {
        if(currentState == GameState.Playing)
        {
            currentState = GameState.Paused;
            savedTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            uiManager.OpenPause();

        }
    }


    public void Resume()
    {
        if (currentState == GameState.Paused)
        {
            currentState = GameState.Playing;
            Time.timeScale = savedTimeScale;
            uiManager.ClosePause();
        }
    }


    public void TogglePause() // 이 함수에서 Pause함수와 Resume함수를 모두 호출 (현재의 상태에 따라 Pause 또는 Resume 을 실행)
    {
        if (currentState == GameState.Playing)
        {
            Pause();
        }
        else if(currentState == GameState.Paused)
        {
            Resume();
        }
    }


    public void StartGame()
    {
        elapsedPlayTime = 0.0f; //새 스테이지를 시작할 때 타이머를 0으로 초기화
        currentState = GameState.Playing;
        isPlaying = true;
        // 추후 추가할 기능
        // 플레이어 초기화
        // 적 스폰 시작
        // UI 업데이트

        Debug.Log("Game Started");
    }


    public void HandleGameOver()
    {
        currentState = GameState.GameOver;

        isPlaying = false;

        //GameOver UI 표시
        //재시작 버튼 노출
        //데이터 정리
        StartCoroutine(CoHandleGameOver());
    }

    IEnumerator CoHandleGameOver()
    {
        yield return new WaitForSeconds(2.0f);

        Time.timeScale = 0.0f;
        currentState = GameState.PostResult;

        uiManager.OpenGameOver();

        Debug.Log("Game Over");
    }



    public void handleGameClear()
    {
        currentState = GameState.Clear;

        isPlaying = false;

        //보상 처리
        //다음 스테이지 해금
        //결과 화면 표시

        if (uiManager != null)
        {
            uiManager.OpenClear();
        }

        Time.timeScale = 0.0f;
        currentState = GameState.PostResult;

        Debug.Log("Game Clear");
    }


    public void RestartGame()
    {
        Time.timeScale = defaultTimeScale;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //현재 실행되고있는 Scene의 이름을 가져옴. 이 외에도 직접 씬 이름을 기입하거나 씬 번호를 기입해서 특정 씬을 불러올 수 있음

    }


    public bool IsPlaying()
    { 
        if(currentState == GameState.Playing && isPlaying == true)
        {
            return true;
        }

        return false;
    }


    public float GetElapsedPlayTime()
    {
        return elapsedPlayTime;
    }


}
