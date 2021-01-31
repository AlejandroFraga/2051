using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreadController : MonoBehaviour
{
    public GameObject m_MixingPanel = default;

    public DragAndDrop m_FlourDAD = default;

    public DragAndDrop m_WaterDAD = default;

    public DragAndDrop m_SaltDAD = default;

    public DragAndDrop m_YeastDAD = default;

    public Image m_Bowl = default;

    public Image m_Water = default;

    public List<Sprite> m_BowlStates = default;

    int m_BowlState = 0;

    public List<Sprite> m_Waters = default;

    int m_WaterLevel = default;

    public int m_KneadTimes = 10;

    int m_Kneaded = 0;

    public DragAndDrop m_BreadDAD = default;

    public GameObject m_OvenPanel = default;

    public Image m_Oven = default;

    public List<Sprite> m_OvenStates = default;

    int m_OvenState = 0;

    public float m_OvenTime = 10.0f;

    [ReadOnly]
    public float m_OvenCounter = 0f;

    public GameObject m_OvenTimer = default;

    // Start is called before the first frame update
    void Start()
    {
        InitMixing();
        ChangeToMixing();
    }

    public void Update()
    {
        if (m_OvenState > 0 && m_OvenState < m_OvenStates.Count - 2)
        {
            if (m_OvenCounter < m_OvenTime)
            {
                m_OvenCounter = Mathf.Min(m_OvenTime, m_OvenCounter + Time.deltaTime);

                m_OvenTimer.transform.RotateAround(m_OvenTimer.transform.position, new Vector3(0, 0, 1), 360 * (Time.deltaTime / m_OvenTime));

                float newState = m_OvenCounter / (m_OvenTime / (m_OvenStates.Count - 3));

                if (m_OvenCounter == m_OvenTime)
                {
                    UpdateOven();
                    Debug.Log("Bread ready");
                }
                else if (m_OvenState < newState)
                {
                    UpdateOven();
                }
            }
        }
    }

    void InitMixing()
    {
        m_WaterLevel = m_Waters.Count - 1;
        m_Water.sprite = m_Waters[m_WaterLevel];
        m_Bowl.sprite = m_BowlStates[m_BowlState];
    }

    void ChangeToMixing()
    {
        m_OvenPanel.SetActive(false);
        m_MixingPanel.SetActive(true);

        RestartMixing();
    }

    void RestartMixing()
    {
        m_FlourDAD.m_CanMove = true;
        m_WaterDAD.m_CanMove = false;
        m_SaltDAD.m_CanMove = false;
        m_YeastDAD.m_CanMove = false;
    }

    public void AddIngredient()
    {
        UpdateBowl();

        m_FlourDAD.RestartPosition();
        m_WaterDAD.RestartPosition();
        m_SaltDAD.RestartPosition();
        m_YeastDAD.RestartPosition();

        m_FlourDAD.m_CanMove = m_BowlState == 0;
        m_WaterDAD.m_CanMove = m_BowlState == 1;
        m_SaltDAD.m_CanMove = m_BowlState == 2;
        m_YeastDAD.m_CanMove = m_BowlState == 3;
    }

    void UpdateBowl()
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

    public void UseWater()
    {
        if (m_WaterLevel > 0)
        {
            m_WaterLevel--;
            m_Water.sprite = m_Waters[m_WaterLevel];
        }
    }

    public void Knead()
    {
        if (m_BowlState > 3 && m_Kneaded < m_KneadTimes)
        {
            m_Kneaded++;
            UpdateBowl();
        }
        else
        {
            ChangeToOven();
        }
    }

    void ChangeToOven()
    {
        m_OvenPanel.SetActive(true);
        m_MixingPanel.SetActive(false);

        RestartOven();
    }

    void RestartOven()
    {
        m_BreadDAD.gameObject.SetActive(true);
    }

    public void InitOven()
    {
        if (m_OvenState == 0)
        {
            m_BreadDAD.RestartPosition();
            m_BreadDAD.m_CanMove = false;
            m_BreadDAD.gameObject.SetActive(false);

            UpdateOven();
        }
    }

    void UpdateOven()
    {
        if (m_OvenState < m_OvenStates.Count)
        { 
            m_OvenState++;
            m_Oven.sprite = m_OvenStates[m_OvenState];
        }
    }

    public void GetBread()
    {
        if (m_OvenState == m_OvenStates.Count - 2)
            UpdateOven();
    }
}
