using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum ReplyPos
{
    Right,
    Left
};

public class VideoCallController : MonoBehaviour
{
    [Header("Gameplay")]

    [Tooltip("Duration of the chat in seconds.")]
    public float m_ChatDuration = 60.0f;

    [Tooltip("Duration of the bye sequence in seconds.")]
    public float m_ByeDuration = 3.0f;

    [Tooltip("Seconds to show the drained screen.")]
    public float m_DrainedScreenTime = 2.0f;

    /// <summary>
    /// Controls if we are at the bye sequence
    /// </summary>
    bool m_AtBye = default;

    /// <summary>
    /// Controls if we are at the drained battery image
    /// </summary>
    bool m_Drained = default;

    /// <summary>
    /// Controls if the mini-game was completed
    /// </summary>
    bool m_Completed = default;

    [Header("Mobile")]

    [Tooltip("Mobile image of the video call.")]
    public Image m_Mobile = default;

    [Tooltip("Mobile default sprite.")]
    public Sprite m_MobileDefault = default;

    [Tooltip("Mobile battery drained sprite.")]
    public Sprite m_MobileDrained = default;

    [Header("Battery")]

    [Tooltip("Battery icon image.")]
    public Image m_Battery = default;

    [Tooltip("Battery icon sprites.")]
    public List<Sprite> m_Batteries = new List<Sprite>();

    /// <summary>
    /// Controls the state and level of battery
    /// </summary>
    int m_BatteryState = default;

    /// <summary>
    /// Duration of each battery level, calculated by chat duration / (number of battery levels - 1)
    /// </summary>
    float m_BatteryTime = 20.0f;

    /// <summary>
    /// Timer to control the countdown of each battery level
    /// </summary>
    float m_BatteryTimer = default;

    [Header("Hour")]

    [Tooltip("Hour displayed image.")]
    public Image m_Hour = default;

    [Tooltip("Hour displayed sprites.")]
    public List<Sprite> m_Hours = new List<Sprite>();

    /// <summary>
    /// Controls the hour displayed
    /// </summary>
    int m_HourState = default;

    /// <summary>
    /// Duration of each hour displayed, calculated by (chat duration + bye duration) / number of hours
    /// </summary>
    float m_HourTime = 10.0f;

    /// <summary>
    /// Timer to control the countdown of each hour displayed
    /// </summary>
    float m_HourTimer = default;

    [Header("Connection")]

    [Tooltip("Wifi connection image.")]
    public Image m_Connection = default;

    [Tooltip("Wifi connected sprite.")]
    public Sprite m_Connected = default;

    [Tooltip("Wifi disconnected sprite.")]
    public Sprite m_Disconnected = default;

    [Header("Disonnection")]

    [Tooltip("Wifi disconnection button.")]
    public GameObject m_Disconnection = default;

    [Tooltip("Number of connections checks in the whole chat duration, that could lead to a disconnection.")]
    public int m_ConnectionChecks = 10;

    /// <summary>
    /// Timer to control the countdown of each connection check
    /// </summary>
    float m_ConnectionTime = 10.0f;

    /// <summary>
    /// Timer to control the countdown of each connection check
    /// </summary>
    float m_ConnectionTimer = default;

    [Range(0.0f, 10.0f), Tooltip("Base probability of a disconnection when checking the connection. Goes from 0 (None) to 10(All).")]
    public int m_BaseDiscnProbability = 1;

    /// <summary>
    /// Disconnection probability, that will start at the base and increase if the probability is not meet.
    /// When the probability is meet, the value goes back to the base. So, as it fails, its probabilities increases.
    /// </summary>
    int m_DiscnProbability = default;

    [Tooltip("Force with which the disconnection icon shakes.")]
    public float m_ShakeForce = 5.0f;

    [Header("Messages")]

    [Tooltip("All the chat images of the video call.")]
    public List<Image> m_Chats = new List<Image>();

    [Range(0.0f, 10.0f), Tooltip("Probability of showing a reply instead of a chat. Goes from 0 (None) to 10(All).")]
    public int m_ReplyProbability = 3;

    [Tooltip("Time between messages.")]
    public float m_MessageTime = 2.0f;

    /// <summary>
    /// Timer to control the countdown of each message
    /// </summary>
    float m_MessageTimer = default;

    [Tooltip("Reply on the bottom left of the screen image.")]
    public Image m_ReplyLeft = default;

    [Tooltip("Reply on the bottom right of the screen image.")]
    public Image m_ReplyRight = default;

    [Tooltip("Default sprite of the reply on the bottom left of the screen.")]
    public Sprite m_RpLfDefault = default;

    [Tooltip("Selected sprite of the reply on the bottom left of the screen.")]
    public Sprite m_RpLfSelected = default;

    [Tooltip("Bye default sprite of the reply on the bottom left of the screen.")]
    public Sprite m_RpLfByeDefault = default;

    [Tooltip("Bye selected sprite of the reply on the bottom left of the screen.")]
    public Sprite m_RpLfByeSelected = default;

    [Tooltip("Default sprite of the reply on the bottom right of the screen.")]
    public Sprite m_RpRgDefault = default;

    [Tooltip("Selected sprite of the reply on the bottom right of the screen.")]
    public Sprite m_RpRgSelected = default;

    [Tooltip("Bye default sprite of the reply on the bottom right of the screen.")]
    public Sprite m_RpRgByeDefault = default;

    [Tooltip("Bye selected sprite of the reply on the bottom right of the screen.")]
    public Sprite m_RpRgByeSelected = default;

    /// <summary>
    /// Sound event instance of FMOD that controls the talk sound
    /// </summary>
    FMOD.Studio.EventInstance m_SoundInstanceTalk = default;

    /// <summary>
    /// Sound event instance of FMOD that controls the keyboard sound
    /// </summary>
    FMOD.Studio.EventInstance m_SoundInstanceKeyboard = default;


    // Start is called before the first frame update
    void Start()
    {
        // Init and play all background sounds
        InitBackgroundSounds();
        StartBackgroundSounds();

        // Init the variables and the chat
        InitVars();
        InitChat();
    }

    // Update is called once per frame
    void Update()
    {
        // We check if the mini-game was completed
        CheckCompleted();

        // Update all the timers
        UpdateTimers();

        // Always check battery and hour timers
        if (m_BatteryTimer <= 0.0f)
            BatteryDrain();

        if (m_HourTimer <= 0.0f)
            UpdateHour();

        // If there's no connection, shake the image and ignore the message and connection timer
        if (m_Disconnection.activeSelf)
            ShakeDisconnection();

        else if (m_MessageTimer <= 0.0f)
            ShowChatOrReply();

        else if (m_ConnectionTimer <= 0.0f)
            CheckConnection();
    }

    /// <summary>
    /// Create the Sound event instances
    /// </summary>
    void InitBackgroundSounds()
    {
        m_SoundInstanceTalk = FMODUnity.RuntimeManager.CreateInstance(StringHelper.videoCallChattingSoundEvent);
        m_SoundInstanceKeyboard = FMODUnity.RuntimeManager.CreateInstance(StringHelper.videoCallKeyboardSoundEvent);
    }

    /// <summary>
    /// Start the Sound event instances
    /// </summary>
    void StartBackgroundSounds()
    {
        m_SoundInstanceTalk.start();
        m_SoundInstanceKeyboard.start();
    }

    /// <summary>
    /// Stop the Sound event instances
    /// </summary>
    void StopBackgroundSounds()
    {
        m_SoundInstanceTalk.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        m_SoundInstanceKeyboard.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    /// <summary>
    /// Inits variables to start the mini-game
    /// </summary>
    void InitVars()
    {
        // The last battery time doesn't count to the chat duration, it'll last bye duration
        m_BatteryTimer = m_BatteryTime = m_ChatDuration / (m_Batteries.Count - 1);

        // The hour will be updating all the videocall, including chat and bye
        m_HourTimer = m_HourTime = (m_ChatDuration + m_ByeDuration) / m_Hours.Count;

        m_ConnectionTimer = m_ConnectionTime = m_ChatDuration / m_ConnectionChecks;

        m_DiscnProbability = m_BaseDiscnProbability;
    }

    /// <summary>
    /// Inits variables to start the mini-game
    /// </summary>
    void InitChat()
    {
        HideMessages();
        m_Disconnection.SetActive(false);
    }

    /// <summary>
    /// Hides all the chat and reply messages
    /// </summary>
    void HideMessages()
    {
        ShowAllChatting(false);

        GameObjectHelper.SetActive(m_ReplyLeft, false);
        GameObjectHelper.SetActive(m_ReplyRight, false);
    }

    /// <summary>
    /// Shows all the chat messages if 'show' is true, hides all otherwise
    /// </summary>
    /// <param name="show">Shows all if true, hides all otherwise</param>
    void ShowAllChatting(bool show = true)
    {
        for (int i = 0; i < m_Chats.Count; i++)
            m_Chats[i].gameObject.SetActive(show);
    }

    /// <summary>
    /// Checks if the mini-game was completed, and goes back to the room if so
    /// </summary>
    void CheckCompleted()
    {
        if (m_Completed)
        {
            // Save the completed mini-game and go back to the room
            PlayerData.m_VideoCallCompleted = true;
            SceneManager.LoadScene(StringHelper.roomScene);
        }
    }

    /// <summary>
    /// Updates all the active timers of the video call
    /// </summary>
    void UpdateTimers()
    {
        float deltaTime = Time.deltaTime;

        m_BatteryTimer -= deltaTime;
        m_HourTimer -= deltaTime;

        // The connection and message timer are only updated if the no connection is not active
        if (!m_Disconnection.activeSelf)
        {
            m_ConnectionTimer -=  deltaTime;
            m_MessageTimer -= deltaTime;
        }

        UpdateDrained();
    }

    void UpdateDrained()
    {
        if (m_Drained)
        {
            m_DrainedScreenTime -= Time.deltaTime;

            if (m_DrainedScreenTime <= 0.0f)
                m_Completed = true;
        }
    }

    /// <summary>
    /// Drains by one the level of the battery if we are not at the bye section
    /// </summary>
    void BatteryDrain()
    {
        UpdateBattery();

        // If the battery is at the last level, set at bye to true.
        // If it was already at true, the by section ended, and so, the battery is finally drained.
        if (m_BatteryState == m_Batteries.Count - 1)
        {
            if (!m_AtBye)
            {
                m_AtBye = true;
                HideMessages();
                ShowReply();
            }
            else
                BatteryDrained();
        }
    }

    /// <summary>
    /// The battery is drained, so the mobile sprite is updated, the sound stops and everything in hidden
    /// </summary>
    void BatteryDrained()
    {
        m_Mobile.sprite = m_MobileDrained;

        StopBackgroundSounds();

        HideMessages();
        HideMobileIcons();

        m_Drained = true;
    }

    /// <summary>
    /// Hide all the UI mobile icons
    /// </summary>
    void HideMobileIcons()
    {
        m_Disconnection.SetActive(false);

        GameObjectHelper.SetActive(m_Battery, false);
        GameObjectHelper.SetActive(m_Hour, false);
        GameObjectHelper.SetActive(m_Connection, false);
    }

    /// <summary>
    /// Shake the disconnection icon
    /// </summary>
    void ShakeDisconnection()
    {
        Vector3 position = m_Disconnection.transform.position;
        position.x += m_ShakeForce;
        m_ShakeForce = -m_ShakeForce;
        m_Disconnection.transform.position = position;
    }

    /// <summary>
    /// Show a chat or a reply
    /// </summary>
    void ShowChatOrReply()
    {
        if (m_AtBye) return;

        // If we are not at bye section, restart the timer and show either a chat or a reply
        HideMessages();

        m_MessageTimer = m_MessageTime;

        if (NumberHelper.RandomInRange(1, 10) > m_ReplyProbability)
            ShowChat(NumberHelper.RandomInRange(0, 3));

        else
            ShowReply();
    }

    /// <summary>
    /// Show the message from a random chat
    /// </summary>
    /// <param name="pos"></param>
    void ShowChat(int pos)
    {
        for (int i = 0; i < m_Chats.Count; i++)
            m_Chats[i].gameObject.SetActive(i == pos);
    }

    /// <summary>
    /// Decrease the battery level if there are any levels left
    /// </summary>
    void UpdateBattery()
    {
        if (m_BatteryState < m_Batteries.Count - 1)
        {
            m_BatteryState++;
            m_Battery.sprite = m_Batteries[m_BatteryState];

            // Restart the battery timer depending on the level
            if(m_BatteryState != m_Batteries.Count - 1)
                m_BatteryTimer = m_BatteryTime;

            else
                m_BatteryTimer = m_ByeDuration;
        }
    }

    /// <summary>
    /// Increase the hour if there are any hours left
    /// </summary>
    void UpdateHour()
    {
        if (m_HourState < m_Hours.Count - 1)
        {
            m_HourState++;
            m_Hour.sprite = m_Hours[m_HourState];

            // Restart the hour timer
            m_HourTimer = m_HourTime;
        }
    }

    /// <summary>
    /// Check if the connection fails or not and restarts the timer
    /// </summary>
    void CheckConnection()
    {
        if (m_BatteryState < m_Batteries.Count - 1)
        {
            m_ConnectionTimer = m_ConnectionTime;
            RandLooseConnection();
        }
    }

    /// <summary>
    /// Randomly disconnect if the disconnection probability are meet.
    /// Increase the probability otherwise.
    /// </summary>
    void RandLooseConnection()
    {
        if (NumberHelper.RandomInRange(1, 10) <= m_DiscnProbability)
        {
            m_DiscnProbability = m_BaseDiscnProbability;
            Disconnect();
        }
        else
            m_DiscnProbability++;
    }

    /// <summary>
    /// Disconnect, showing its button
    /// </summary>
    void Disconnect()
    {
        m_Connection.sprite = m_Disconnected;
        GameObjectHelper.SetActive(m_Disconnection);

        StopBackgroundSounds();
    }

    /// <summary>
    /// Called from the disconnect Button.OnClick, reconnect
    /// </summary>
    public void Reconnect()
    {
        m_Connection.sprite = m_Connected;
        GameObjectHelper.SetActive(m_Disconnection, false);

        StartBackgroundSounds();
    }

    /// <summary>
    /// Shows both replies having into account if we are at the bye section or not
    /// </summary>
    public void ShowReply()
    {
        if (m_AtBye)
        {
            m_ReplyLeft.sprite = m_RpLfByeDefault;
            m_ReplyRight.sprite = m_RpRgByeDefault;
        }
        else
        {
            m_ReplyLeft.sprite = m_RpLfDefault;
            m_ReplyRight.sprite = m_RpRgDefault;
        }

        GameObjectHelper.SetActive(m_ReplyLeft);
        GameObjectHelper.SetActive(m_ReplyRight);
    }

    /// <summary>
    /// Called from the Button.OnClick, a click on the left reply
    /// </summary>
    public void ReplyLeftClick()
    {
        if (m_AtBye)
            ReplyByeClick(ReplyPos.Left);
        else
            ReplyClick(ReplyPos.Left);
    }

    /// <summary>
    /// Called from the Button.OnClick, a click on the right reply
    /// </summary>
    public void ReplyRightClick()
    {
        if (m_AtBye)
            ReplyByeClick(ReplyPos.Right);
        else
            ReplyClick(ReplyPos.Right);
    }

    /// <summary>
    /// Handle the click on the 'replyPos' button
    /// </summary>
    /// <param name="replyPos">Position of the reply clicked</param>
    void ReplyClick(ReplyPos replyPos)
    {
        m_ReplyLeft.sprite = m_RpLfSelected;
        m_ReplyRight.sprite = m_RpRgSelected;

        ShowReply(replyPos);
    }

    /// <summary>
    /// Handle the click on the 'replyPos' bye button
    /// </summary>
    /// <param name="replyPos">Position of the reply clicked</param>
    void ReplyByeClick(ReplyPos replyPos)
    {
        m_ReplyLeft.sprite = m_RpLfByeSelected;
        m_ReplyRight.sprite = m_RpRgByeSelected;

        ShowReply(replyPos);

        ShowAllChatting();
    }

    /// <summary>
    /// Shows the selected reply button and plays the click sound
    /// </summary>
    /// <param name="replyPos">Position of the reply clicked</param>
    void ShowReply(ReplyPos replyPos)
    {
        FMODUnity.RuntimeManager.PlayOneShot(StringHelper.videoCallClickSoundEvent, transform.position);

        GameObjectHelper.SetActive(m_ReplyLeft, replyPos == ReplyPos.Left);
        GameObjectHelper.SetActive(m_ReplyRight, replyPos == ReplyPos.Right);
    }
}
