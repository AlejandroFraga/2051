using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreadController : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        m_WaterLevel = m_Waters.Count - 1;
        m_Water.sprite = m_Waters[m_WaterLevel];
        m_Bowl.sprite = m_BowlStates[m_BowlState];

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
        if (m_BowlState < m_BowlStates.Count - 1)
        {
            m_BowlState++;
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
        if (m_BowlState > 3)
        {
            UpdateBowl();
        }
    }
}
