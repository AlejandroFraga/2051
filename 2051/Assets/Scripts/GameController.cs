using UnityEngine;
using Unity.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject m_RoomGrey = default;

    public GameObject m_RoomColor = default;

    public GameObject m_YogaBase = default;

    public GameObject m_YogaCompleted = default;

    public GameObject m_YogaButton = default;

    public GameObject m_VideCallBase = default;

    public GameObject m_VideCallCompleted = default;

    public GameObject m_VideCallButton = default;

    public GameObject m_BreadBase = default;

    public GameObject m_BreadCompleted = default;

    public GameObject m_BreadButton = default;

    PlayerData playerData = default;

    bool m_Completed = false;

    public float m_timeUntilEnd = 5f;

    [ReadOnly]
    public float m_timeCounter = 0f;

    // Start is called before the first frame update
    void Start()
    {
        playerData = SaveSystem.Load();

        if (playerData.m_BreadCompleted && playerData.m_VideoCallCompleted && playerData.m_YogaCompleted)
        {
            m_Completed = true;
            MarkCompleted();
            return;
        }

        m_BreadBase.SetActive(!playerData.m_BreadCompleted);
        m_BreadButton.SetActive(!playerData.m_BreadCompleted);
        m_BreadCompleted.SetActive(playerData.m_BreadCompleted);

        m_VideCallBase.SetActive(!playerData.m_VideoCallCompleted);
        m_VideCallButton.SetActive(!playerData.m_VideoCallCompleted);
        m_VideCallCompleted.SetActive(playerData.m_VideoCallCompleted);

        m_YogaBase.SetActive(!playerData.m_YogaCompleted);
        m_YogaButton.SetActive(!playerData.m_YogaCompleted);
        m_YogaCompleted.SetActive(playerData.m_YogaCompleted);
    }

    public void Update()
    {
        if (m_Completed)
        {
            m_timeCounter = Mathf.Min(m_timeUntilEnd, m_timeCounter + Time.deltaTime);

            if (m_timeCounter >= m_timeUntilEnd)
            {
                Completed();
            }
        }
    }

    void MarkCompleted()
    {
        m_RoomColor.SetActive(true);

        m_BreadBase.SetActive(false);
        m_BreadCompleted.SetActive(false);

        m_VideCallBase.SetActive(false);
        m_VideCallCompleted.SetActive(false);

        m_YogaBase.SetActive(false);
        m_YogaCompleted.SetActive(false);
    }

    public void OpenYogaMinigame()
    {
        SceneManager.LoadScene("YogaScene");
    }

    public void OpenVideoCallMinigame()
    {
        SceneManager.LoadScene("VideoCallScene");
    }

    public void OpenBreadMinigame()
    {
        SceneManager.LoadScene("BreadScene");
    }

    void Completed()
    {
        SceneManager.LoadScene("EndingScene");
    }
}
