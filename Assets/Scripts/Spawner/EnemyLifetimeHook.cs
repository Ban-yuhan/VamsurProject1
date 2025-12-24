using UnityEngine;

public class EnemyLifetimeHook : MonoBehaviour
{

    private EnemySpawner spawner;


    public void SetSpawner(EnemySpawner value)
    {
        spawner = value;
    }

    private void OnDestroy() //오브젝트가 파괴될 때 호출
    {
        if (spawner != null)
        {
            spawner.NotifyEnemyDies();
        }
    }
}
