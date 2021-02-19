using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomController : MonoBehaviour
{
    [Header("Gameplay")]

    [Tooltip("Complete room in greyscale.")]
    public GameObject m_RoomGrey = default;

    [Tooltip("Complete room in color.")]
    public GameObject m_RoomColor = default;

    [Tooltip("Yoga mini-game incompleted.")]
    public GameObject m_YogaBase = default;

    [Tooltip("Yoga mini-game completed.")]
    public GameObject m_YogaCompleted = default;

    [Tooltip("Yoga mini-game button.")]
    public GameObject m_YogaButton = default;

    [Tooltip("Video call mini-game incompleted.")]
    public GameObject m_VideoCallBase = default;

    [Tooltip("Video call mini-game completed.")]
    public GameObject m_VideoCallCompleted = default;

    [Tooltip("Video call mini-game button.")]
    public GameObject m_VideoCallButton = default;

    [Tooltip("Bread mini-game incompleted.")]
    public GameObject m_BreadBase = default;

    [Tooltip("Bread mini-game completed.")]
    public GameObject m_BreadCompleted = default;

    [Tooltip("Bread mini-game button.")]
    public GameObject m_BreadButton = default;

    /// <summary>
    /// Controls if the game was completed
    /// </summary>
    bool m_Completed = default;

    [Tooltip("Time to wait from completion to end scene.")]
    public float m_timeUntilEnd = 5.0f;

    /// <summary>
    /// Time counter to control the time until the end
    /// </summary>
    float m_timeCounter = default;

    // Start is called before the first frame update
    void Start()
    {
        // If all the mini-games are completed, mark the whole game as completed
        if (AllMiniGamesCompleted())
        {
            MarkCompleted();
            return;
        }

        // Otherwise show the buttons and incompleted for the remaining mini-games
        m_YogaBase.SetActive(!PlayerData.m_YogaCompleted);
        m_YogaButton.SetActive(!PlayerData.m_YogaCompleted);
        m_YogaCompleted.SetActive(PlayerData.m_YogaCompleted);

        m_VideoCallBase.SetActive(!PlayerData.m_VideoCallCompleted);
        m_VideoCallButton.SetActive(!PlayerData.m_VideoCallCompleted);
        m_VideoCallCompleted.SetActive(PlayerData.m_VideoCallCompleted);

        m_BreadBase.SetActive(!PlayerData.m_BreadCompleted);
        m_BreadButton.SetActive(!PlayerData.m_BreadCompleted);
        m_BreadCompleted.SetActive(PlayerData.m_BreadCompleted);
    }

    // Update is called once per frame
    public void Update()
    {
        // If completed, count the time until end
        if (m_Completed)
        {
            m_timeCounter += Time.deltaTime;

            if (m_timeCounter >= m_timeUntilEnd)
                Completed();
        }
    }

    /// <summary>
    /// Returns if all the mini-games are completed
    /// </summary>
    /// <returns>All the mini-games are completed</returns>
    bool AllMiniGamesCompleted()
    {
        return PlayerData.m_BreadCompleted && PlayerData.m_VideoCallCompleted && PlayerData.m_YogaCompleted;
    }

    /// <summary>
    /// Show the color room and set active to false to all the rest of elements
    /// </summary>
    void MarkCompleted()
    {
        m_Completed = true;

        m_RoomColor.SetActive(true);

        m_YogaBase.SetActive(false);
        m_YogaButton.SetActive(false);
        m_YogaCompleted.SetActive(false);

        m_VideoCallBase.SetActive(false);
        m_VideoCallButton.SetActive(false);
        m_VideoCallCompleted.SetActive(false);

        m_BreadBase.SetActive(false);
        m_BreadButton.SetActive(false);
        m_BreadCompleted.SetActive(false);
    }

    /// <summary>
    /// Called from m_YogaButton.OnClick, open the yoga mini-game
    /// </summary>
    public void OpenYogaMinigame()
    {
        SceneManager.LoadScene(StringHelper.yogaScene);
    }

    /// <summary>
    /// Called from m_VideoCallButton.OnClick, open the video call mini-game
    /// </summary>
    public void OpenVideoCallMinigame()
    {
        SceneManager.LoadScene(StringHelper.videoCallScene);
    }

    /// <summary>
    /// Called from m_BreadButton.OnClick, open the bread mini-game
    /// </summary>
    public void OpenBreadMinigame()
    {
        SceneManager.LoadScene(StringHelper.breadScene);
    }

    /// <summary>
    /// Go to the ending scene, as the game was completed
    /// </summary>
    void Completed()
    {
        SceneManager.LoadScene(StringHelper.endingScene);
    }
}
