using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    [SerializeField]
    private float baseExpToLevel = 5.0f; //레벨업에 필요한 경험치(1lv → 2lv)

    [SerializeField]
    private float growthPerLevel = 2.5f; //레벨당 증가량(필요 경험치 증가량)

    [SerializeField]
    private LevelUpUI levelupUI;

    private float currentExp;
    private int currentLevel = 1;

    private GameManager gameManager;


    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        currentExp = 0.0f;
    }


    public void AddExperience(float amount)
    {
        if (gameManager != null)
        {
            if (gameManager.IsPlaying() == false)
            {
                return;
            }
        }

        if(amount <= 0.0f)
        {
            return;
        }

        currentExp += amount;

        int safety = 32; //반복문이 무한루프에 빠지지 않도록 방지하는데 사용할 변수

        while (safety > 0)
        {
            float need = GetRequiredExpForNextLevel(); //다음레벨까지 필요한 경험치량을 가져옴
            if (currentExp >= need)
            {
                currentExp -= need;
                ++currentLevel;
                OnLevelUP();
            }
            else
            {
                break;
            }

            --safety;
        }

    }


    public float GetRequiredExpForNextLevel()
    {
        float need = baseExpToLevel + growthPerLevel * (currentLevel - 1);
        return Mathf.Max(1.0f, need); //둘 중에 더 큰 값 반환
    }


    void OnLevelUP()
    {
        Debug.Log("Level Up! \n Lv : " + currentLevel);

        if (levelupUI != null)
        {
            levelupUI.Open();
        }
    }


    public int GetCurrentLevel()
    {
        return currentLevel;
    }


    public float GetCurrentExp()
    {
        return currentExp;
    }
}
