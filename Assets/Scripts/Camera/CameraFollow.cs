using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float followLerp = 0.15f; // 추적 속도

    public float fixedZ = -10f; //카메라가 씬과 겹치지 않토록 z축으로 얼마나 떨어져있을지 고정시킬 변수


    private void LateUpdate()
    {
        if(target == null)
        {
            return;
        }    

        Vector3 current = transform.position; //카메라 자신의 포지션
        Vector3 desired = new Vector3(target.position.x, target.position.y, fixedZ);

        Vector3 smoothed = Vector3.Lerp(current, desired, followLerp); //현재 위치에서 목표 위치로 추적속도로 보간.

        transform.position = smoothed;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
