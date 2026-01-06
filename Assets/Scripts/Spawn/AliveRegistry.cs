using UnityEngine;
using System.Collections.Generic;

//살아있는 적 캐릭터 등록하는 스크립트
public class AliveRegistry : MonoBehaviour
{
    private HashSet<GameObject> aliveSet = new HashSet<GameObject>(); ////살아있는 오브젝트 모음 | HashSet : 컨테이너 형 변수
    /*만약 동일한 오브젝트가 List에 여러번 할당된 경우 코드상 문제는 없지만, 실제 살아있는 오브젝트 수와 List의 오브젝트 수가 일치하지 않으면서 문제가 생길 가능성 존재
    → 중복 할당이 불가능한 HashSet을 사용하여 이러한 문제를 방지 */

    //List는 Index를 사용해 쉽게 데이터를 꺼낼 수 있지만, HashSet은 처음부터 순차적으로 돌면서 필요한 데이터를 꺼내와야한다는 불편함이 있음. 다만, 그런 작업이 필요 없기 때
    //문에 HashSet을 통해 살아있는 적 수를 확인.

    public void Register(GameObject go) //살아있는 적 수를 추가하는 함수. 적이 생성되는 곳에서 호출.
    {
        if(go == null)
        {
            return;
        }
        aliveSet.Add(go); //집합(HashSet)에 추가
    }


    public void Unregister(GameObject go) //살아있는 적 수를 감소시키는 함수. 적이 파괴되는 곳에서 호출.
    {
        if(go == null)
        {
            return;
        }
        aliveSet.Remove(go); //집합(HashSet)에서 제거
    }


    public int GetAliveCount()
    {
        return aliveSet.Count; //현재 살아있는 수
    }
}
