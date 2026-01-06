using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject panelClear;

    [SerializeField]
    private GameObject panelPause;

    [SerializeField]
    private GameObject panelGameOver;

    [SerializeField]
    private GameObject panelReady;

    private void Start()
    {
        if (panelClear != null)
        {
            panelClear.SetActive(false);
        }

        if (panelPause != null)
        {
            panelPause.SetActive(false);
        }

        if (panelGameOver != null)
        {
            panelGameOver.SetActive(false);
        }
    }

    public void OpenClear()
    {
        if(panelClear != null)
        {
            panelClear.SetActive(true);
        }
    }

    public void OpenPause()
    {
        if (panelPause != null)
        {
            panelPause.SetActive(true);
        }
    }

    public void ClosePause()
    {
        if (panelPause != null)
        {
            panelPause.SetActive(false);
        }
    }

    public void OpenGameOver()
    {
        if(panelGameOver != null)
        {
            panelGameOver.SetActive(true);
        }
    }


    public void CloseReady()
    {
        if (panelReady != null)
        {
            panelReady.SetActive(false);
        }
    }
}
