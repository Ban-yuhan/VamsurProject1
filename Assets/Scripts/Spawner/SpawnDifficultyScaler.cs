using UnityEngine;

public class SpawnDifficultyScaler : MonoBehaviour
{

    [SerializeField]
    private float baseSpawnRatePerSecond = 1.0f; //기본 초당 몹 소환량
    [SerializeField] 
    private float spawnRateGrowthPerminute = 0.5f; //분당 증가량

    [SerializeField]
    private float minSpawnRadius = 8.0f; //플레이어 위치로부터 스폰 가능한 최소 거리
    [SerializeField]
    private float maxSpawnRadius = 14.0f; //플레이어 위치로부터 최대 거리

    [SerializeField]
    private int maxAliveEnemies = 60; //동시에 존재 가능한 적 수


    public float GetBaseSpawnRatePerSecond()
    {
        return baseSpawnRatePerSecond;
    }


    public float GetSpawnRateGrowthPerminute()
    {
        return spawnRateGrowthPerminute;
    }


    public float GetMinSpawnRadius()
    { 
    return minSpawnRadius;
    }


    public float GetMaxSpawnRadius()
    {
        return maxSpawnRadius;
    }


    public int GetMaxAliveEnemises()
    {
        return maxAliveEnemies;
    }
}


