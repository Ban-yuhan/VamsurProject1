using UnityEngine;

/// <summary>
/// 카메라 화면 바깥 도넛 영역에서 스폰 좌표를 샘플링하고
/// 플레이어의 근접 반경을 배제하여 안전한 스폰 위치를 제공.
/// 카메라를 기준으로 화면 경계를 월드 좌표로 계산.
/// </summary>
public class SafeSpawnRingProvider : MonoBehaviour
{
    [SerializeField]
    private Camera targetCamera;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private float innerPadding = 2.0f; //안쪽 경계. 화면 안쪽으로 더 줄여서 등장 금지 영역을 설정하기 위한 여백

    [SerializeField]
    private float outerOffset = 4.0f; //바깥쪽 경계. 화면 바깥쪽으로 확장한 외곽 링 두께

    [SerializeField]
    private float PlayerExcludeRadius = 3.0f; // 플레이어 근접 배제 반경

    [SerializeField]
    private int sampleTries = 8; //좌표 샘플 최대 시도 횟수.


    /// <summary>
    /// 현재 프레임에서 안전한 스폰 좌표 하나를 뽑는다.
    /// </summary>
    /// <param name="position">뽑은 좌표를 저장할 참조형 변수</param>
    /// <returns></returns>
    public bool TrySample(out Vector3 position) //bool형을 쓰는이유 : 좌표가 제대로 뽑혔다면 true를 반환하기 위함
    {
        position = Vector3.zero;

        if (targetCamera == null)
        {
            return false;
        }

        if(player == null)
        {
            return false;
        }

        float halfHeight = targetCamera.orthographicSize; //orthographicSize : 카메라 화면의 높이값의 절반을 가져옴
        float halfWidth = halfHeight * targetCamera.aspect; // 카메라 화면 너비의 절반 계산. (가로세로 비율을 곱해줌)

        Vector3 center = targetCamera.transform.position;

        //사각형 영역을 지정할 수 있는 구조체. 화면 안쪽의 생성 금지 역영 설정.
        Rect innerRect = new Rect(center.x - halfWidth + innerPadding, center.y - halfHeight + innerPadding, (halfWidth * 2.0f) - innerPadding * 2.0f, (halfHeight * 2.0f) - innerPadding * 2.0f);

        //화면 밖의 바운드 영역 설정.
        Rect outerRect = new Rect(center.x - halfWidth - innerPadding, center.y - halfHeight - innerPadding, (halfWidth * 2.0f) + innerPadding * 2.0f, (halfHeight * 2.0f) + innerPadding * 2.0f);

        for(int i =0; i<sampleTries; ++i)
        {
            //외곽 사각 둘레를 따라 임의의 변을 고르고 좌표를 뽑는다.
            int edgePick = Random.Range(0, 4); //0 : 좌, 1 : 우, 2 : 아래, 3 : 위

            float x = 0.0f;
            float y = 0.0f;

            if (edgePick == 0) //화면 좌측에서 좌표 뽑기
            {
                x = outerRect.xMin; //사각형의 최소x를 반환
                y = Random.Range(outerRect.yMin, outerRect.yMax); //사각형의 최소 y값~최대 y값에서 랜덤 위치를 반환.
            }
            else if(edgePick == 1) //화면 우측에서 좌표 뽑기
            {
                x = outerRect.xMax;
                y = Random.Range(outerRect.yMin, outerRect.yMax);
            }
            else if(edgePick == 2)//화면 아래에서 좌표 뽑기
            {
                x = Random.Range(outerRect.xMin, outerRect.xMax);
                y = outerRect.yMin;
            }
            else if (edgePick == 3)//화면 위에서 좌표 뽑기
            {
                x = Random.Range(outerRect.xMin, outerRect.xMax);
                y = outerRect.yMax;
            }

            Vector3 p = new Vector3(x, y, 0.0f);

            if(innerRect.Contains(new Vector2(x, y)) == true)
            {
                continue; //생성금지영역이 겹칠경우 재시도
            }

            float dist = Vector2.Distance(new Vector2(x, y), new Vector2(player.position.x, player.position.y));
            if (dist < PlayerExcludeRadius) //뽑은 좌표가 플레이어 중심으로 지정된 반경 내에 포함될 경우
            {
                continue; 
            }

            position = p;
            return true;
        }

        return false;
    }
}
