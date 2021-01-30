using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YogaController : MonoBehaviour
{
    [Header("Gameplay")]

    [Tooltip("Minimum number of postures in the mini-game.")]
    public int m_MinPostures = 3;

    [Tooltip("Maximum number of postures in the mini-game.")]
    public int m_MaxPostures = 5;

    // Number of postures in the mini-game
    private int m_NPostures = default;

    private bool m_Holding = false;

    [Header("Video posture")]

    public List<Sprite> m_VideoPostures = default;

    public int m_VideoPosture = 0;

    public Image m_VideoPostureImage = default;

    public Slider m_VideoSlider = default;

    [Header("Player posture")]

    public List<Sprite> m_Postures = default;

    public int m_SelectedPosture = 0;

    public Image m_SelectedPostureImage = default;

    // Start is called before the first frame update
    void Start()
    {
        if (!m_VideoPostureImage || m_VideoPostures.Count < 1) return;

        UpdateNumberOfPostures();

        UpdateVideoPosture();

    }

    // Update is called once per frame
    void Update()
    {
        if (m_Holding)
        {
            m_VideoSlider.value = Mathf.Min(m_VideoSlider.value + Time.deltaTime, m_VideoSlider.maxValue);

            if (m_VideoSlider.value == m_VideoSlider.maxValue)
            {
                m_VideoSlider.value = 0;
                UpdateVideoPosture();
            }
        }
    }

    void UpdateNumberOfPostures()
    {
        m_NPostures = NumberHelper.RandomInRange(m_MinPostures, m_MaxPostures);
    }

    void UpdateVideoPosture()
    {
        m_VideoPosture = NumberHelper.RandomInRange(0, m_VideoPostures.Count - 1);
        m_VideoPostureImage.sprite = m_VideoPostures[m_VideoPosture];
        m_Holding = false;
    }

    public void PreviousPosture()
    {
        m_SelectedPosture += m_Postures.Count - 1;
        m_SelectedPosture %= m_Postures.Count;
        m_SelectedPostureImage.sprite = m_Postures[m_SelectedPosture];
    }

    public void NextPosture()
    {
        m_SelectedPosture++;
        m_SelectedPosture %=  m_Postures.Count;
        m_SelectedPostureImage.sprite = m_Postures[m_SelectedPosture];
    }

    public void HoldPostureDown()
    {
        if (m_VideoPosture == m_SelectedPosture)
            m_Holding = true;
    }

    public void HoldPostureUp()
    {
        m_Holding = false;
    }
}
