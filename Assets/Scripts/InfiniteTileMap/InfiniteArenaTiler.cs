using UnityEngine;
using System.Collections.Generic;


public class InfiniteArenaTiler : MonoBehaviour
{
    [SerializeField]
    private Transform player; //플레이어의 Transform정보 저장

    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField]
    private float tileSize = 10.0f;

    [SerializeField]
    private int halfGridX = 2; //플레이어 기준 가로방향 절반 칸 수.

    [SerializeField]
    private int halfGridY = 2; //플레이어 기준 세로방향 절반 칸 수

    [SerializeField]
    private bool snapToWhole = true; //타일 경계와 정확히 맞추는지 여부.

    [SerializeField]
    private List<Transform> tiles = new List<Transform>();

    private int gridWidth; //전체 가로 칸 수.
    private int gridHeight; //전체 세로 칸 수.
    private int currentCenterX; //현재 중심 격자의 가로 좌표.
    private int currentCenterY; //현재 중심 격자의 세로 좌표.
    private bool initialized; //초기화 여부


    private void Start()
    {
        if(player == null)
        {
            return;
        }

        if(tilePrefab == null)
        {
            return; 
        }

        if(tileSize <= 0)
        {
            tileSize = 1.0f; //최소 타일사이즈가 1은 되야하기때문에 최소값 조절
        }

        if(halfGridX < 1)
        {
            halfGridX = 1;
        }

        if (halfGridY < 1)
        {
            halfGridY = 1;
        }

        //가로, 세로길이가 항상 홀수가 됨
        gridWidth = halfGridX * 2 + 1;
        gridHeight = halfGridY * 2 + 1; 

        int total = gridWidth * gridHeight; //생성할 타일의 총 개수 계산

        for(int i = 0; i < total; ++i)
        {
            GameObject go = Instantiate(tilePrefab, transform);
            if (go != null)
            {
                go.transform.localScale = new Vector3(tileSize, tileSize, go.transform.localScale.z);
                tiles.Add(go.transform);
            }
        }

        Vector2Int center = GetPlayerCell(); //처음 시작했을때의 중심 좌표를 가져옴
        currentCenterX = center.x;
        currentCenterY = center.y;

        RepositionAll();
        initialized = true;

    }


    private void Update()
    {
        if(initialized == false) //타일 생성이 제대로 완료되지 않으면 리턴.
        {
            return;
        }

        if(player == null)
        {
            return;
        }

        Vector2Int cell = GetPlayerCell(); //현재 격자의 좌표를 가져옴
        if(cell.x != currentCenterX || cell.y != currentCenterY) //플레이어가 위치한 타일의 중심좌표가 이전의 타일의 중심 좌표가 같지 않다면(다른 격자(타일)로 이동했는지 여부 확인)
        {
            currentCenterX = cell.x;
            currentCenterY = cell.y;    
            RepositionAll(); //타일 재배치
        }
    }


    /// <summary>
    /// 타일들을 현재 중심 격자 기준으로 모두 재배치한다.
    /// </summary>
    void RepositionAll()
    {
        int index = 0;

        for (int gy = -halfGridY; gy <= halfGridY; ++gy)
        {
            for (int gx = -halfGridX; gx <= halfGridX; ++gx)
            {
                if(index >= tiles.Count)
                {
                    return;
                }

                int cellX = currentCenterX + gx; //배치할 격자 가로 좌표
                int CellY = currentCenterY + gy; //배치할 격자 세로 좌표

                float px = cellX * tileSize; //배치할 실제 가로 위치
                float py = CellY * tileSize; //배치할 실제 세로 위치

                tiles[index].position = new Vector3(px, py, tiles[index].position.z);

                ++index; //다음 리스트의 index로 넘어감   
            }
        }
    }


    /// <summary>
    /// 플레이어 위치로부터 격자 좌표를 계산
    /// </summary>
    /// <returns>격자 좌표</returns>
    Vector2Int GetPlayerCell()
    {
        Vector3 p = player.position;
        float gx = p.x / tileSize; //가로 방향 비율
        float gy = p.y / tileSize; //세로 방향 비율

        int cx;
        int cy;

        if(snapToWhole == true)
        {
            cx = Mathf.FloorToInt(gx); //gx값을 내림 처리하여 Int로 반환
            cy = Mathf.FloorToInt(gy); //gy값을 내림 처리하여 Int로 반환
        }
        else
        {
            cx = Mathf.RoundToInt(gx); //gx값을 반올림 처리하여 Int로 반환
            cy = Mathf.RoundToInt(gy); //gy값을 반올림 처리하여 Int로 반환
        }

        return new Vector2Int(cx, cy); //cx와 cy를 동시에 반환할 수 없기 때문에 Vector2 형식으로 반환해 줌
    }
}
