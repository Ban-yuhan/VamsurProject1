using UnityEngine;
using System.Collections.Generic;

public class UpgradeHistory : MonoBehaviour
{
    [SerializeField]
    private int recentCapacity = 8; //최근 표시/선택 기록 버퍼 크기

    private Queue<string> recentShowGroups = new Queue<string>(); //표시된 그룹 id
    private Queue<string> recentPickedGroups = new Queue<string>(); //선택된 그룹 id

    private Dictionary<string, int> tagAbsenceLevels = new Dictionary<string, int>(); // 태그 별 연속 미출현 카운트. 몇 번 연속으로 미출현했는지 정보를 저장


   public int GetAbsenceLevelsForTag(string tag) //Dictionary에 들어가 있는 정보 중 전달받은 tag에 해당되는 정보를 반환
    {
        if(tagAbsenceLevels.ContainsKey(tag) == true)
        {
            return tagAbsenceLevels[tag];
        }

        return 0;
    }


    public int GetRecentPickedCountOfGroup(string groupId, int lookback)
    {
        if(lookback <= 0)
        {
            return 0;
        }

        string id = string.IsNullOrEmpty(groupId) == true ? "-" : groupId; //IsNullOrEmpty() : ()안의 변수가 Null이거나 Empty인지 확인. 
        //확인 후 비어있으면 "-"를, 비어있지않으면 groupId를 반환

        int count = 0;
        string[] arr = recentPickedGroups.ToArray(); //Queue 자료구조를 배열(Array)자료구조로 변환해 arr 변수에 저장

        for (int i = arr.Length - 1; i >= 0; --i)
        {
            if (arr[i] == id)
            {
                {
                    ++count;
                }
                --lookback;
            }
        }

        return count;
    }


    /// <summary>
    /// 레벨업 1회가 지나갈 때 마다 호출하여 '미출현' 카운트를 증가시킨다.
    /// 해당 레벨에서 어떤 태그가 표시되면 Recordshown 함수에서 0으로 리셋된다.
    /// </summary>
    public void TickAbsenceForAllTags(HashSet<string> observedTags)
    {
        List<string> keys = new List<string>(tagAbsenceLevels.Keys); //()안의 데이터를 모두 선언한 변수에 저장

        for (int i = 0; i < keys.Count; ++i)
        {
            string k = keys[i];
            if(observedTags.Contains(k) == false) //observedTag에 k가 포함되어있지않다면 아래 실행
            {
                ++tagAbsenceLevels[k];
            }
        }
    }


    void EnqueueGroup(Queue<string> q, string groupId) //Enqueue :  queue 자료구조 안에 데이터를 넣는 과정을  enqueue라고 부름
    {
        string id = string.IsNullOrEmpty(groupId) == true ? "-" : groupId;
        q.Enqueue(id); // Enqueue : Queue에 데이터를 추가
        while (q.Count > recentCapacity)
        {
            q.Dequeue(); //Dequeue : queue 자료구조에 있는 데이터를 꺼내서 삭제
        }
    }


    public void RecordPicked(UpgradeData data)
    {
        if (data != null)
        {
            EnqueueGroup(recentPickedGroups, data.groupId);
        }
    }


    public void RecordShown(UpgradeData data)
    {
        if (data != null)
        {
            EnqueueGroup(recentShowGroups, data.groupId);
            if (data.tags != null)
            {
                for (int i = 0; i < data.tags.Length; ++i)
                {
                    string t = data.tags[i];
                    tagAbsenceLevels[t] = 0; //t에 해당하는 Dictionary의 데이터를 0으로 만들어줌
                }
            }
        }
    }
}
