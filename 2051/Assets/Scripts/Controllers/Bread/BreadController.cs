using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class BreadController : MonoBehaviour
{
    [Header("Mixing")]

    [Tooltip("Panel parent of the mixing section of the mini-game.")]
    public GameObject m_MixingPanel = default;

    [Tooltip("Flour drag and drop UI element.")]
    public DragAndDropUIElement m_FlourDAD = default;

    [Tooltip("Water jar drag and drop UI element.")]
    public DragAndDropUIElement m_WaterDAD = default;

    [Tooltip("Salt drag and drop UI element.")]
    public DragAndDropUIElement m_SaltDAD = default;

    [Tooltip("Yeast drag and drop UI element.")]
    public DragAndDropUIElement m_YeastDAD = default;

    [Tooltip("Mixing bowl image.")]
    public Image m_Bowl = default;

    [Tooltip("Water image.")]
    public Image m_Water = default;

    [Tooltip("Sprites for the different mixing bowl states.")]
    public List<Sprite> m_BowlStates = new List<Sprite>();

    /// <summary>
    /// Control the state of the mixing bowl to update the image
    /// </summary>
    int m_BowlState = default;

    [Tooltip("Sprites for the different water jar states.")]
    public List<Sprite> m_Waters = new List<Sprite>();

    /// <summary>
    /// Control the water level of the jar to update the image
    /// </summary>
    int m_WaterLevel = default;

    [Tooltip("Number of times that the dough has to be kneaded.")]
    public int m_KneadTimes = 10;

    /// <summary>
    /// Number of times that the dough was kneaded
    /// </summary>
    int m_Kneaded = default;

    [Header("Oven")]

    [Tooltip("Panel parent of the oven section of the mini-game.")]
    public GameObject m_OvenPanel = default;

    [Tooltip("Bread drag and drop UI element.")]
    public DragAndDropUIElement m_BreadDAD = default;

    [Tooltip("Oven Image.")]
    public Image m_Oven = default;

    [Tooltip("Sprites for the different oven states.")]
    public List<Sprite> m_OvenStates = new List<Sprite>();

    /// <summary>
    /// Control the state of the oven to update the image
    /// </summary>
    int m_OvenState = -1;

    [Tooltip("Countdown time to select the position.")]
    public float m_OvenTime = 10.0f;

    /// <summary>
    /// Time counter for the oven
    /// </summary>
    float m_OvenCounter = default;

    [Tooltip("Oven timer.")]
    public GameObject m_OvenTimer = default;

    [Tooltip("Oven controller.")]
    public GameObject m_OvenController = default;

    [Tooltip("Countdown time to select the position.")]
    public GameObject m_CookedController = default;


    // Start is called before the first frame update
    void Start()
    {
        InitMixing();
        ChangeToMixing();
    }

    // Update is called once per frame
    public void Update()
    {
        UpdateOven();
    }

    /// <summary>
    /// Inits the mixing panel
    /// </summary>
    void InitMixing()
    {
        m_WaterLevel = m_Waters.Count - 1;
        m_Water.sprite = m_Waters[m_WaterLevel];
        m_Bowl.sprite = m_BowlStates[m_BowlState];
    }

    /// <summary>
    /// Changes the active panel to mixing and restarts the mixing state
    /// </summary>
    void ChangeToMixing()
    {
        m_OvenPanel.SetActive(false);
        m_MixingPanel.SetActive(true);

        RestartMixing();
    }

    /// <summary>
    /// Restarts the mixing
    /// </summary>
    void RestartMixing()
    {
        m_BowlState = default;
        m_Kneaded = default;
        UpdateIngredients();
        m_Bowl.sprite = m_BowlStates[m_BowlState];
    }

    /// <summary>
    /// Adds an ingredient to the bowl
    /// </summary>
    public void AddIngredient()
    {
        UpdateBowlState();

        m_FlourDAD.RestartPosition();
        m_WaterDAD.RestartPosition();
        m_SaltDAD.RestartPosition();
        m_YeastDAD.RestartPosition();

        UpdateIngredients();
    }

    /// <summary>
    /// Updates the bowl state to the next one
    /// </summary>
    void UpdateBowlState()
    {
        m_BowlState++;

        if (m_BowlState >= m_BowlStates.Count)
            m_BowlState -= 3;

        if (m_BowlState < m_BowlStates.Count)
        {
            m_Bowl.sprite = m_BowlStates[m_BowlState];

            if (m_BowlState == 2)
                UseWater();
        }
    }

    /// <summary>
    /// Use water and decrease its level
    /// </summary>
    void UseWater()
    {
        if (m_WaterLevel > 0)
        {
            m_WaterLevel--;
            m_Water.sprite = m_Waters[m_WaterLevel];
        }
    }

    /// <summary>
    /// Update the ingredient that can be used
    /// </summary>
    void UpdateIngredients()
    {
        m_FlourDAD.m_CanMove = m_BowlState == 0;
        m_WaterDAD.m_CanMove = m_BowlState == 1;
        m_SaltDAD.m_CanMove = m_BowlState == 2;
        m_YeastDAD.m_CanMove = m_BowlState == 3;

        m_FlourDAD.gameObject.SetActive(m_BowlState < 1);
        m_SaltDAD.gameObject.SetActive(m_BowlState < 3);
        m_YeastDAD.gameObject.SetActive(m_BowlState < 4);

        IngredientSound();
    }

    /// <summary>
    /// Play the sound of the respective ingredient
    /// </summary>
    void IngredientSound()
    {
        switch (m_BowlState)
        {
            case 1:
                FMODUnity.RuntimeManager.PlayOneShot(StringHelper.breadFlourSoundEvent, transform.position);
                break;
            case 2:
                FMODUnity.RuntimeManager.PlayOneShot(StringHelper.breadWaterSoundEvent, transform.position);
                break;
            case 3:
            case 4:
                FMODUnity.RuntimeManager.PlayOneShot(StringHelper.breadSaltSoundEvent, transform.position);
                break;
        }
    }

    /// <summary>
    /// Knead the dough
    /// </summary>
    public void Knead()
    {
        if (m_BowlState < 4) return;

        if (m_Kneaded < m_KneadTimes)
        {
            m_Kneaded++;
            UpdateBowlState();
            FMODUnity.RuntimeManager.PlayOneShot(StringHelper.breadKneadSoundEvent, transform.position);
        }
        else
            ChangeToOven();
    }

    /// <summary>
    /// Changes the active panel to oven and restarts the oven state
    /// </summary>
    void ChangeToOven()
    {
        m_OvenPanel.SetActive(true);
        m_MixingPanel.SetActive(false);

        RestartOven();
    }

    /// <summary>
    /// Restarts the oven
    /// </summary>
    void RestartOven()
    {
        m_OvenController.SetActive(true);
        m_CookedController.SetActive(false);

        m_BreadDAD.m_CanMove = true;
        m_BreadDAD.gameObject.SetActive(true);

        m_OvenCounter = default;
        m_OvenState = -1;
        UpdateOvenState();
    }

    /// <summary>
    /// Starts the oven by putting the bread inside
    /// </summary>
    public void InitOven()
    {
        if (m_OvenState == default)
        {
            FMODUnity.RuntimeManager.PlayOneShot(StringHelper.breadPutInOvenSoundEvent, transform.position);

            m_BreadDAD.RestartPosition();
            m_BreadDAD.m_CanMove = false;
            m_BreadDAD.gameObject.SetActive(false);

            UpdateOvenState();
        }
    }

    /// <summary>
    /// Updates the oven if it's working
    /// </summary>
    void UpdateOven()
    {
        if (IsOvenWorking() && m_OvenState <= UpdateOvenCounter())
        {
            UpdateOvenState();

            if (m_OvenCounter == m_OvenTime)
                FMODUnity.RuntimeManager.PlayOneShot(StringHelper.breadOvenBellSoundEvent, transform.position);
        }
    }

    /// <summary>
    /// Returns true if the oven is working, false otherwise
    /// </summary>
    /// <returns>Returns true if the oven is working, false otherwise</returns>
    bool IsOvenWorking()
    {
        return m_OvenState > 0 && IsOvenInStateBeforeLast(1) && m_OvenCounter < m_OvenTime;
    }

    /// <summary>
    /// Returns true if the oven state is equal to max - before or less, false otherwise
    /// </summary>
    /// <param name="before">Number of states from the last from which it checks</param>
    /// <returns>True if the oven state is equal to max - before or less, false otherwise</returns>
    bool IsOvenInStateBeforeLast(int before = default)
    {
        return m_OvenState < m_OvenStates.Count - 1 - before;
    }

    /// <summary>
    /// Updates the oven counter with the delta time and returns the new state starting from 0
    /// </summary>
    /// <returns>The new state starting from 0</returns>
    float UpdateOvenCounter()
    {
        m_OvenCounter = Mathf.Min(m_OvenTime, m_OvenCounter + Time.deltaTime);

        UpdateOvenTimer();

        return m_OvenCounter / (m_OvenTime / (m_OvenStates.Count - 3));
    }

    /// <summary>
    /// Rotates the oven timer
    /// </summary>
    void UpdateOvenTimer()
    {
        if (m_OvenTimer)
            m_OvenTimer.transform.RotateAround(m_OvenTimer.transform.position, new Vector3(0, 0, 1), 360 * (Time.deltaTime / m_OvenTime));
    }

    /// <summary>
    /// Updates the oven state to the next one
    /// </summary>
    void UpdateOvenState()
    {
        if (m_OvenState < m_OvenStates.Count)
        { 
            m_OvenState++;
            m_Oven.sprite = m_OvenStates[m_OvenState];
        }
    }

    /// <summary>
    /// Get's the bread from the oven if it's ready
    /// </summary>
    public void GetBreadFromOven()
    {
        if (IsOvenInStateFromLast(1))
        {
            FMODUnity.RuntimeManager.PlayOneShot(StringHelper.breadGetFromOvenSoundEvent, transform.position);

            UpdateOvenState();
            m_OvenController.SetActive(false);
            m_CookedController.SetActive(true);
        }
    }

    /// <summary>
    /// Get the bread from the table after it's done and start a new one if there's water left.
    /// Otherwise mark the mini-game as completed and go back to the room
    /// </summary>
    public void GetBreadDone()
    {
        if (IsOvenInStateFromLast())
        {
            if (m_WaterLevel > 0)
                ChangeToMixing();

            else
            {
                // Save the completed mini-game and go back to the room
                PlayerData.m_BreadCompleted = true;
                SceneManager.LoadScene(StringHelper.roomScene);
            }
        }
    }

    /// <summary>
    /// Returns true if the oven state is equal to max - before, false otherwise
    /// </summary>
    /// <param name="before">Number of states from the last from which it checks</param>
    /// <returns>True if the oven state is equal to max - before, false otherwise</returns>
    bool IsOvenInStateFromLast(int before = default)
    {
        return m_OvenState == m_OvenStates.Count - 1 - before;
    }
}
