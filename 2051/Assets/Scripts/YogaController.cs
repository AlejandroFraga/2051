using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class YogaController : MonoBehaviour
{
    [Header("Gameplay")]

    [Tooltip("Number of postures to do in the mini-game.")]
    public int m_NumPostures = 4;

    List<int> m_DonePostures = default;

    [ReadOnly]
    public float m_TimeToHold = default;

    public float m_MinTimeToHold = 3.0f;

    public float m_MaxTimeToHold = 5.0f;

    public float m_CountdownTime = 3.0f;

    public int m_MsBeforeCountdown = 2000;

    bool m_BeforeCountdown = false;

    [ReadOnly]
    public float m_Countdown = default;

    public TextMeshProUGUI m_ScreenMessage = default;

    bool m_Holding = false;

    bool m_Completed = false;

    int m_Points = 0;

    [Header("Video posture")]

    public List<Sprite> m_VideoPostures = default;

    public int m_VideoPosture = 0;

    public Image m_VideoPostureImage = default;

    public List<Slider> m_VideoSliders = default;

    public List<Image> m_VideoSlidersFills = default;

    [ReadOnly]
    public Slider m_VideoSlider = default;

    public Sprite m_VideoSliderFillMain = default;

    public Sprite m_VideoSliderFillGreen = default;

    public Sprite m_VideoSliderFillRed = default;

    [Header("Player posture")]

    public List<Sprite> m_Postures = default;

    public int m_SelectedPosture = 0;

    public Image m_SelectedPostureImage = default;

    public GameObject m_PreviousButton = default;

    public GameObject m_NextButton = default;


    //Sound

    FMOD.Studio.EventInstance yogaSound;

    // Start is called before the first frame update
    void Start()
    {
        if (!m_VideoPostureImage || m_VideoPostures.Count < 1) return;

        m_Completed = false;
        m_DonePostures = new List<int>();
        for (int i = 0; i < m_VideoSliders.Count; i++)
        {
            m_VideoSliders[i].value = 0;
            m_VideoSlidersFills[i].sprite = m_VideoSliderFillMain;
        }

        m_VideoSlider = m_VideoSliders[m_DonePostures.Count];

        UpdateTimeToHold();

        UpdateVideoPosture();

        StartCountdown();

        ShowButtons();

        m_BeforeCountdown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Completed) return;

        if (m_BeforeCountdown)
        {
            m_BeforeCountdown = false;

            Time.timeScale = 0;

            System.Threading.Thread.Sleep(m_MsBeforeCountdown);

            Time.timeScale = 1;

            if (!NewPosture())
            {
                if (m_Points > 2)
                {
                    // Acabas el juego y ganaste

                    m_ScreenMessage.text = "Completed!";
                    m_Completed = true;
                }
                else
                {
                    // Acabas el juego y perdiste, empieza de nuevo

                    Start();
                }
            }
        }
        else if (m_Holding)
        {
            m_VideoSlider.value = Mathf.Min(m_VideoSlider.value + Time.deltaTime, m_VideoSlider.maxValue);

            if (m_VideoSlider.value == m_VideoSlider.maxValue)
            {
                // Acabas bien la postura

                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/YOGA/Reward", transform.position);

                m_VideoSlidersFills[m_DonePostures.Count - 1].sprite = m_VideoSliderFillGreen;

                m_Points++;

                m_ScreenMessage.text = "Nice!";

                m_BeforeCountdown = true;
                
                
            }
        }
        else if (m_VideoSlider.value == 0)
        {
            m_Countdown = Mathf.Max(0, m_Countdown - Time.deltaTime);

            m_ScreenMessage.text = (Mathf.CeilToInt(m_Countdown)).ToString();

            if (m_Countdown == 0)
            {
                // Acabas mal la postura

                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/YOGA/Fail", transform.position);

                m_VideoSlider.value = m_VideoSlider.maxValue;
                m_VideoSlidersFills[m_DonePostures.Count - 1].sprite = m_VideoSliderFillRed;

                m_ScreenMessage.text = "Bad!";

                m_BeforeCountdown = true;


            }
        }
    }

    bool NewPosture()
    {
        if (m_NumPostures > m_DonePostures.Count)
        {
            m_VideoSlider = m_VideoSliders[m_DonePostures.Count];

            UpdateTimeToHold();

            UpdateVideoPosture();

            StartCountdown();

            ShowButtons();

            return true;
        }

        return false;
    }

    void StartCountdown()
    {
        m_Countdown = m_CountdownTime;
    }

    void UpdateTimeToHold()
    {
        if (m_Completed) return;

        m_VideoSlider.value = 0;
        m_TimeToHold = NumberHelper.RandomInRange(m_MinTimeToHold, m_MaxTimeToHold);
        m_VideoSlider.maxValue = m_TimeToHold;
    }

    void UpdateVideoPosture()
    {
        if (m_Completed || m_DonePostures.Count == m_VideoPostures.Count) return;

        do
        {
            m_VideoPosture = NumberHelper.RandomInRange(0, m_VideoPostures.Count - 1);
        }
        while (m_DonePostures.Contains(m_VideoPosture));

        m_DonePostures.Add(m_VideoPosture);

        m_VideoPostureImage.sprite = m_VideoPostures[m_VideoPosture];
        m_Holding = false;
    }

    public void PreviousPosture()
    {
        // Botón izquierda

        if (m_Completed) return;

        m_SelectedPosture += m_Postures.Count - 1;
        m_SelectedPosture %= m_Postures.Count;
        m_SelectedPostureImage.sprite = m_Postures[m_SelectedPosture];

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/YOGA/Botones", transform.position);


    }

    public void NextPosture()
    {
        // Botón derecha

        if (m_Completed) return;

        m_SelectedPosture++;
        m_SelectedPosture %=  m_Postures.Count;
        m_SelectedPostureImage.sprite = m_Postures[m_SelectedPosture];

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/YOGA/Botones", transform.position);
    }

    public void HoldPostureDown()
    {
        // Aguantar la postura

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/YOGA/Esterilla", transform.position);

        if (m_Completed) return;

        if (m_VideoPosture == m_SelectedPosture)
        {
            m_Holding = true;
            ShowButtons(false);
            m_ScreenMessage.text = "Hold!";
        }
    }

    public void HoldPostureUp()
    {
        // Cuando dejas de aguantar la postura

        m_Holding = false;
    }

    void ShowButtons(bool show = true)
    {
        m_PreviousButton.SetActive(show);
        m_NextButton.SetActive(show);
    }
}
