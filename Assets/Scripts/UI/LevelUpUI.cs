using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Reflection;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField]
    private GameObject panelRoot;

    [SerializeField]
    private Button[] cardButton; //업그레이드 옵션 선택을 위한 버튼

    [SerializeField]
    private Text[] cardText; //업그레이드 옵션 설명표시를 위한 text. TMP는 한글지원을 하지 않기 때문에 기존 Text사용

    [SerializeField]
    private Text titleText; //UI타이틀 표시를 위한 텍스트

    [SerializeField]
    private UpgradeManager upgradeManager;

    private List<UpgradeData> currentChoices = new List<UpgradeData>(); //선택한 옵션들을 저장할 List


    private void Awake()
    {
        if(panelRoot != null)
        {
            panelRoot.SetActive(false);
        }

        for (int i = 0; i < cardButton.Length; ++i)
        {
            cardButton[i].onClick.AddListener(() => { OnSelect(i); }); //이벤트에 함수를 지정(OnSelect 함수 호출) : 람다함수 or 익명함수 / index값을 파라미터로 전달
        }
    }
    
    
    public void Open()
    {
        currentChoices = upgradeManager.DrawThree(); //업그레이드 가능한 세 개의 정보 가져오기

        for(int i = 0; i<cardText.Length; ++i)
        {
            BindCardText(cardText[i], i);
        }

        if (panelRoot != null)
        {
            panelRoot.SetActive(true);
        }

        Time.timeScale = 0.0f;
    }


    public void OnSelect(int index)
    {
        if (index >= 0 && index < currentChoices.Count)
        {
            UpgradeData chosen = currentChoices[index];
            upgradeManager.ApplyUpgrade(chosen);

        }

        Close();

    }


    public void Close() //옵션 선택 시 게임을 다시 진행
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(false);
        }

        Time.timeScale = 1.0f;
    }


    void BindCardText(Text target, int index)
    {
        if (target == null)
        {
            return;
        }

        if (index < currentChoices.Count)
        {
            UpgradeData data = currentChoices[index];
            if (data != null)
            {
                target.text = $"{data.displayName}\n{data.description}";
            }
            else
            {
                target.text = "N/A";
            }
        }
        else
        {
            target.text = "N/A";
        }
    }
}
