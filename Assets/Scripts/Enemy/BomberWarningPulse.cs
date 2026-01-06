using UnityEngine;

public class BomberWarningPulse : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sprite;

    [SerializeField]
    private Color pulseColor = Color.red;

    [SerializeField]
    private float pulseSpeed = 8.0f;

    [SerializeField]
    private float scaleAmp = 0.12f; //스케일 증폭량


    private bool running; //경고 연출 실행여부
    private Color baseColor; //기본(원래) 색상
    private Vector3 baseScale; //기본(원래) 스케일

    private void Awake()
    {
        if(sprite == null)
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
        }

        if(sprite != null)
        {
            baseColor = sprite.color;
        }

        baseScale = transform.localScale;
        running = false;

    }


    private void Update()
    {
        if(running == false)
        {
            return;
        }

        float t = Mathf.Abs(Mathf.Sin(Time.time * pulseSpeed));
        if(sprite != null)
        {
            Color c = Color.Lerp(baseColor, pulseColor, t);
            sprite.color = c;
        }

        float s = 1.0f + t * scaleAmp;
        Vector3 b = baseScale;
        transform.localScale = new Vector3(b.x * s, b.y * s, b.z);
    }


    /// <summary>
    /// 경고 연출을 시작
    /// </summary>
    public void StartWarning()
    {
        running = true;
    }

    
    /// <summary>
    /// 경고 연출을 중지하고 색상과 크기를 원래대로 복구
    /// </summary>
    public void StopWarning()
    {
        running = false;
        if(sprite != null)
        {
            sprite.color = baseColor;
        }

        transform.localScale = baseScale;
    }

}
