using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEngine.SceneManagement;
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

    [ReadOnly]
    public int m_BowlState = 0;

    public List<Sprite> m_Waters = default;

    [ReadOnly]
    public int m_WaterLevel = default;

    public int m_KneadTimes = 10;

    [ReadOnly]
    public int m_Kneaded = 0;

    public DragAndDrop m_BreadDAD = default;

    public GameObject m_OvenPanel = default;

    public Image m_Oven = default;

    public List<Sprite> m_OvenStates = default;

    [ReadOnly]
    public int m_OvenState = -1;

    public float m_OvenTime = 10.0f;

    [ReadOnly]
    public float m_OvenCounter = 0f;

    public GameObject m_OvenTimer = default;

    public int m_MsBeforeNext = 2000;

    bool m_Completed = false;

    public GameObject m_OvenController = default;

    public GameObject m_CookedController = default;

    // Start is called before the first frame update
    void Start()
    {
        InitMixing();
        ChangeToMixing();
    }

    public void Update()
    {
        if (m_Completed) return;

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

                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/PAN/Campana horno", transform.position);

                    // Acaba el horno
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
        m_BowlState = 0;

        m_Kneaded = 0;

        UpdateIngredients();
    }

    public void AddIngredient()
    {
        if (m_Completed) return;

        UpdateBowl();

        m_FlourDAD.RestartPosition();
        m_WaterDAD.RestartPosition();
        m_SaltDAD.RestartPosition();
        m_YeastDAD.RestartPosition();

        UpdateIngredients();
    }

    void UpdateIngredients()
    {
        // Ingrediente añadido. m_BowlState == 1 => Harina, 2 => Agua, 3 => Sal, 4 => Levadura 

        m_FlourDAD.m_CanMove = m_BowlState == 0;
        m_WaterDAD.m_CanMove = m_BowlState == 1;
        m_SaltDAD.m_CanMove = m_BowlState == 2;
        m_YeastDAD.m_CanMove = m_BowlState == 3;

        m_FlourDAD.gameObject.SetActive(m_BowlState < 1);
        //m_WaterDAD.gameObject.SetActive(m_BowlState < 2);
        m_SaltDAD.gameObject.SetActive(m_BowlState < 3);
        m_YeastDAD.gameObject.SetActive(m_BowlState < 4);
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

        //El terror del sonido
        if (m_BowlState == 1) {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/PAN/Harina", transform.position);
        } else if (m_BowlState == 2)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/PAN/Echar agua", transform.position);
        } else if (m_BowlState == 3)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/PAN/Salero", transform.position);
        } else if (m_BowlState == 4)
        {
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/PAN/Salero", transform.position);
        }

    }

    void UseWater()
    {
        if (m_WaterLevel > 0)
        {
            m_WaterLevel--;
            m_Water.sprite = m_Waters[m_WaterLevel];
        }
    }

    public void Knead()
    {
        if (m_Completed) return;

        // Amasas

        if (m_BowlState > 3 && m_Kneaded < m_KneadTimes)
        {
            m_Kneaded++;
            UpdateBowl();
            
             FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/PAN/Amasar", transform.position);
            
        }
        else
            ChangeToOven();
    }

    void ChangeToOven()
    {
        m_OvenPanel.SetActive(true);
        m_MixingPanel.SetActive(false);

        RestartOven();
    }

    void RestartOven()
    {
        m_OvenController.SetActive(true);
        m_CookedController.SetActive(false);

        m_BreadDAD.m_CanMove = true;
        m_BreadDAD.gameObject.SetActive(true);
        m_OvenCounter = 0f;
        m_OvenState = -1;
        UpdateOven();
    }

    public void InitOven()
    {
        if (m_OvenState == 0)
        {
            // Metes en el horno

            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/PAN/Meter bandeja", transform.position);

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

    public void GetBreadFromOven()
    {
        if (m_OvenState == m_OvenStates.Count - 2)
        {
            // Sacas del horno

            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/PAN/Sacar bandeja", transform.position);

            UpdateOven();
            m_OvenController.SetActive(false);
            m_CookedController.SetActive(true);
        }
    }

    public void GetBreadDone()
    {
        if (m_OvenState == m_OvenStates.Count - 1)
        {
            if (m_WaterLevel > 0)
                ChangeToMixing();

            else
            {
                m_Completed = true;

                // Save the completed level
                PlayerData playerData = SaveSystem.Load();
                playerData.m_BreadCompleted = true;
                SaveSystem.Save(playerData);

                SceneManager.LoadScene("RoomScene");
            }
        }
    }
}
