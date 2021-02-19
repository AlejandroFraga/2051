using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class YogaController : MonoBehaviour
{
    /// <summary>
    /// Number of postures to do in the mini-game
    /// </summary>
    int m_NumPostures = 4;

    /// <summary>
    /// All the postures that have been already done
    /// </summary>
    List<int> m_DonePostures = new List<int>();

    [Header("Gameplay")]

    [Tooltip("Minimum time to hold the position.")]
    public float m_MinTimeToHold = 2.0f;

    [Tooltip("Maximum time to hold the position.")]
    public float m_MaxTimeToHold = 3.0f;

    /// <summary>
    /// Time between the minimum and the maximum to hold the position
    /// </summary>
    float m_TimeToHold = default;

    [Tooltip("Seconds before the countdown to select the position.")]
    public float m_BeforeCountdownTime = 2.0f;

    /// <summary>
    /// Control if we are before the countdown
    /// </summary>
    bool m_WaitBeforeCountdown = default;

    /// <summary>
    /// Countdown time counter
    /// </summary>
    float m_BeforeCountdown = default;

    [Tooltip("Countdown time to select the position.")]
    public float m_CountdownTime = 5.0f;

    /// <summary>
    /// Countdown time counter
    /// </summary>
    float m_Countdown = default;

    [Tooltip("Message in the screen to update with info.")]
    public TextMeshProUGUI m_ScreenMessage = default;

    /// <summary>
    /// Control when the player is holding
    /// </summary>
    bool m_Holding = default;

    /// <summary>
    /// Number of points earned, 1 for each correct posture, 0 for incorrect
    /// </summary>
    int m_Points = default;

    [Header("Video posture")]

    [Tooltip("Sprites for the video postures.")]
    public List<Sprite> m_VideoPostures = new List<Sprite>();

    /// <summary>
    /// The current posture of the video
    /// </summary>
    int m_VideoPosture = default;

    [Tooltip("Image in which the video postures will be displayed.")]
    public Image m_VideoPostureImage = default;

    [Tooltip("Sliders of the video to show the completion percentage.")]
    public List<Slider> m_VideoSliders = new List<Slider>();

    [Tooltip("Sliders of the video fill image to change colors if the posture was correct or not.")]
    public List<Image> m_VideoSlidersFills = new List<Image>();

    /// <summary>
    /// Current video slider
    /// </summary>
    Slider m_VideoSlider = default;

    [Tooltip("Default sprite of the sliders of the video.")]
    public Sprite m_VideoSliderFillDefault = default;

    [Tooltip("Sprite of the sliders of the video when the posture was done successfully.")]
    public Sprite m_VideoSliderFillSuccess = default;

    [Tooltip("Sprite of the sliders of the video when the posture was failed.")]
    public Sprite m_VideoSliderFillFail = default;

    [Header("Player posture")]

    [Tooltip("Sprites for the player postures.")]
    public List<Sprite> m_Postures = new List<Sprite>();

    /// <summary>
    /// The current posture of the player
    /// </summary>
    int m_SelectedPosture = default;

    [Tooltip("Image in which the player postures will be displayed.")]
    public Image m_SelectedPostureImage = default;

    [Tooltip("Button to select the previous posture.")]
    public GameObject m_PreviousButton = default;

    [Tooltip("Button to select the next posture.")]
    public GameObject m_NextButton = default;


    // Start is called before the first frame update
    void Start()
    {
        // If there is no video image or the isn't any postures, return
        if (!m_VideoPostureImage || m_VideoPostures.Count < 1) return;

        InitVars();
        NewPosture();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_WaitBeforeCountdown)
            UpdateBeforeCountdown();

        else if (m_Holding)
            HoldingThePosture();

        else if (m_VideoSlider.value == default)
            UpdateCountdown();
    }

    /// <summary>
    /// Inits variables to start the mini-game
    /// </summary>
    void InitVars()
    {
        m_Points = default;
        m_WaitBeforeCountdown = default;

        m_DonePostures.Clear();
        m_VideoSlider = m_VideoSliders[0];

        ResetSlidersFill();
    }

    /// <summary>
    /// Resets the sliders fill to the default sprite
    /// </summary>
    void ResetSlidersFill()
    {
        for (int i = 0; i < m_VideoSliders.Count && i < m_VideoSlidersFills.Count; i++)
        {
            m_VideoSliders[i].value = default;
            m_VideoSlidersFills[i].sprite = m_VideoSliderFillDefault;
        }
    }

    /// <summary>
    /// Start the wait before countdown
    /// </summary>
    void StartWaitBeforeCountdown()
    {
        m_WaitBeforeCountdown = true;
        m_BeforeCountdown = m_BeforeCountdownTime;
    }

    /// <summary>
    /// Update the wait before countdown
    /// </summary>
    void UpdateBeforeCountdown()
    {
        m_BeforeCountdown -= Time.deltaTime;

        // When the countdown reaches 0, select a new posture, if there isn't any left, check the result
        if (m_BeforeCountdown <= 0.0f)
        {
            m_WaitBeforeCountdown = default;

            if (!NewPosture())
                CheckFinalResult();
        }
    }

    /// <summary>
    /// Sets a new video posture if there any that hasn't been done yet, returning true.
    /// If there are no postures left, returns false.
    /// </summary>
    /// <returns>True if there are postures left, false otherwise</returns>
    bool NewPosture()
    {
        if (m_NumPostures > m_DonePostures.Count)
        {
            m_VideoSlider = m_VideoSliders[m_DonePostures.Count];

            UpdateTimeToHold();
            UpdateVideoPosture();
            StartCountdown();
            ActivateButtons();

            return true;
        }
        return false;
    }

    /// <summary>
    /// Check the final result of the mini-game, after all of the postures ended
    /// </summary>
    void CheckFinalResult()
    {
        // If gained 3 or 4 points, save the completed mini-game and go back to the room
        if (m_Points > 2)
        {
            PlayerData.m_YogaCompleted = true;
            SceneManager.LoadScene(StringHelper.roomScene);
        }
        // Otherwise, start all over again
        else
            Start();
    }

    /// <summary>
    /// Holding the selected player posture, when the time to hold is reached, the posture is completed
    /// </summary>
    void HoldingThePosture()
    {
        m_VideoSlider.value = Mathf.Min(m_VideoSlider.value + Time.deltaTime, m_VideoSlider.maxValue);

        if (m_VideoSlider.value == m_VideoSlider.maxValue)
            PostureCompleted();
    }

    /// <summary>
    /// The posture was holded successfully
    /// </summary>
    void PostureCompleted()
    {
        // Stop holding
        m_Holding = default;

        // Give feedback to the player with the screen message, a fail sound and updating the slider
        if (m_NumPostures > m_DonePostures.Count)
            m_ScreenMessage.text = StringHelper.yogaNiceMessage;
        else
            m_ScreenMessage.text = StringHelper.yogaCompletedMessage;

        FMODUnity.RuntimeManager.PlayOneShot(StringHelper.yogaRewardSoundEvent, transform.position);

        m_VideoSlidersFills[m_DonePostures.Count - 1].sprite = m_VideoSliderFillSuccess;

        // Update the points
        m_Points++;

        // We set the before countdow to prepare until the next countdown
        StartWaitBeforeCountdown();
    }

    /// <summary>
    /// Gets the value rounded (ceil) to seconds of the countdown
    /// </summary>
    /// <returns></returns>
    int CountdownSeconds()
    {
        return Mathf.CeilToInt(m_Countdown);
    }

    /// <summary>
    /// Start the countdown to select the right position
    /// </summary>
    void StartCountdown()
    {
        m_Countdown = m_CountdownTime;
    }

    /// <summary>
    /// Update the countdown to select the right position
    /// </summary>
    void UpdateCountdown()
    {
        m_Countdown = Mathf.Max(default, m_Countdown - Time.deltaTime);

        m_ScreenMessage.text = CountdownSeconds().ToString();

        if (m_Countdown == default)
            CountdownEnded();
    }

    /// <summary>
    /// The posture countdown reached 0 before the player selected the right position
    /// </summary>
    void CountdownEnded()
    {
        // Give feedback to the player with the screen message, a fail sound and updating the slider
        m_ScreenMessage.text = StringHelper.yogaBadMessage;

        FMODUnity.RuntimeManager.PlayOneShot(StringHelper.yogaFailSoundEvent, transform.position);

        m_VideoSlider.value = m_VideoSlider.maxValue;
        m_VideoSlidersFills[m_DonePostures.Count - 1].sprite = m_VideoSliderFillFail;

        // We set the before countdow to prepare until the next countdown
        StartWaitBeforeCountdown();
    }

    /// <summary>
    /// Update the time that the next posture has to be holded
    /// </summary>
    void UpdateTimeToHold()
    {
        if (m_WaitBeforeCountdown) return;

        m_VideoSlider.value = default;
        m_TimeToHold = NumberHelper.RandomInRange(m_MinTimeToHold, m_MaxTimeToHold);
        m_VideoSlider.maxValue = m_TimeToHold;
    }

    /// <summary>
    /// Update the video posture with a new one that hasn't been done yet
    /// </summary>
    void UpdateVideoPosture()
    {
        if (m_WaitBeforeCountdown || m_DonePostures.Count == m_VideoPostures.Count) return;

        // While the random posture selected was already done, select a new random one
        do
        {
            m_VideoPosture = NumberHelper.RandomInRange(default, m_VideoPostures.Count - 1);
        }
        while (m_DonePostures.Contains(m_VideoPosture));

        // Add the posture to the ones done list and update the sprite of the video image
        m_DonePostures.Add(m_VideoPosture);
        m_VideoPostureImage.sprite = m_VideoPostures[m_VideoPosture];
    }

    /// <summary>
    /// Navigate to the previous player posture
    /// </summary>
    public void PreviousPosture()
    {
        if (m_WaitBeforeCountdown) return;

        FMODUnity.RuntimeManager.PlayOneShot(StringHelper.yogaButtonSoundEvent, transform.position);

        m_SelectedPosture += m_Postures.Count - 1;
        m_SelectedPosture %= m_Postures.Count;
        m_SelectedPostureImage.sprite = m_Postures[m_SelectedPosture];
    }

    /// <summary>
    /// Navigate to the next player posture
    /// </summary>
    public void NextPosture()
    {
        if (m_WaitBeforeCountdown) return;

        FMODUnity.RuntimeManager.PlayOneShot(StringHelper.yogaButtonSoundEvent, transform.position);

        m_SelectedPosture++;
        m_SelectedPosture %= m_Postures.Count;
        m_SelectedPostureImage.sprite = m_Postures[m_SelectedPosture];
    }

    /// <summary>
    /// Hold the current posture of the player
    /// </summary>
    public void HoldPosture()
    {
        if (m_WaitBeforeCountdown) return;

        if (m_VideoPosture == m_SelectedPosture)
        {
            m_ScreenMessage.text = StringHelper.yogaHoldMessage;
            FMODUnity.RuntimeManager.PlayOneShot(StringHelper.yogaMatSoundEvent, transform.position);

            m_Holding = true;
            ActivateButtons(false);
        }
    }

    /// <summary>
    /// Let go the posture of the player, and so, stop holding
    /// </summary>
    public void LetGoPosture()
    {
        m_Holding = default;

        // If the posture was let go before finished, tell the player to keep holding
        if(!m_WaitBeforeCountdown)
            m_ScreenMessage.text = StringHelper.yogaKeepHoldingMessage;
    }

    /// <summary>
    /// Activates the player posture navigation buttons if show is true, if false deactivates them
    /// </summary>
    /// <param name="show">If true activates the buttons, if false, deactivates them</param>
    void ActivateButtons(bool show = true)
    {
        m_PreviousButton.SetActive(show);
        m_NextButton.SetActive(show);
    }
}
