using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoCallController : MonoBehaviour
{

    public List<Sprite> m_battery = default;

    public Image m_BatteryImage = default;
    
    public int m_BatteryState = 0;

    public float m_BatteryDuration = 60.0f;

    private float m_NextBatteryTime = 0.0f;

    public float m_UpdatePeriodBattery = 20.0f;

    public List<Sprite> m_Hour = default;

    public Image m_HourImage = default;

    public int m_HourState = 0;

    public List<Sprite> m_Connection = default;

    public Image m_ConnectionImage = default;

    private float m_NextConnectionTime = 0.0f;

    public float m_ConnectionCheckPeriod = 5.0f;

    public GameObject NoConnection = default;

    public int m_FlagConnection = 1;

    public int m_RandomFactor = 9;

    public int m_RandomFactorHolder = default;

    public GameObject m_MessageA = default;

    public Image m_MessageA_Image = default;

    public GameObject m_MessageB = default;

    public Image m_MessageB_Image = default;

    public List<Image> m_chatting = default;

    public float m_timer_mensaje = 0.0f;

    public float m_timer_mensaje_period = 3.0f;



    // Start is called before the first frame update
    void Start()
    {
        m_FlagConnection = 1;
        m_RandomFactorHolder = m_RandomFactor;
        m_NextBatteryTime = m_UpdatePeriodBattery;
        NoConnection.SetActive(false);
        m_MessageA.SetActive(false);
        m_MessageB.SetActive(false);
        GameObjectHelper.SetVisible(m_MessageA_Image, false);
        GameObjectHelper.SetVisible(m_MessageB_Image, false);
        GameObjectHelper.SetVisible(m_chatting[0], false);
        GameObjectHelper.SetVisible(m_chatting[1], false);
        GameObjectHelper.SetVisible(m_chatting[2], false);
        GameObjectHelper.SetVisible(m_chatting[3], false);
    }

    // Update to modify hour and battery
    void Update() 
    { 
        if (Time.time > m_NextBatteryTime) // Lógica de gasto de batería
        {
            m_NextBatteryTime = Time.time + m_UpdatePeriodBattery;
            UpdateBattery();
            UpdateHour();



        }
        if (m_FlagConnection == 1)
        {
            if (Time.time > m_NextConnectionTime) // Lógica pérdida de conexión
            {
                m_NextConnectionTime = Time.time + m_ConnectionCheckPeriod;
                UpdateConnection();



            }
            if (Time.time > m_timer_mensaje) // Lógica Mensajes Ajenos
            {
                ChatPop();
                m_timer_mensaje = Time.time + m_timer_mensaje_period;
                if (Random.Range(0, 10) > 4)
                {
                    ChatPush();
                }
                else
                {
                    MessagePush();

                }

            }
        }
    }

    public void UpdateBattery()
    {
        // Update Battery with timer
        m_BatteryState++;
        m_BatteryImage.sprite = m_battery[m_BatteryState];

    }

    public void UpdateHour()
    {
        //Update Hoour with timer
        m_HourState++;
        m_HourImage.sprite = m_Hour[m_HourState];

    }

    public void UpdateConnection()
    {
        // Function to Randomly drop connection (Top image + Random location)
        if (Random.Range(0, 10) > m_RandomFactor)
        {
            m_ConnectionImage.sprite = m_Connection[1];
            m_RandomFactor = m_RandomFactorHolder;
            m_FlagConnection = 0;
            SpawnNoConnection();



        }
        else {
            m_RandomFactor--;
        }

    }

    private void SpawnNoConnection()
    {
        // LowBattery Message (called from UpdateConnection)
        Vector3 NoConnectionPos = new Vector3(Random.Range(-0.7f, 0.7f), Random.Range(-0.4f, 0.4f),0f);
        Instantiate(NoConnection, NoConnectionPos, Quaternion.identity);
        NoConnection.SetActive(true);

    }

    public void DestroyNoConnection()
    {
        // LowBattery Message (called from battery degradation)
        //GameObjectHelper.SetVisible('imagen', false);
        m_ConnectionImage.sprite = m_Connection[0];
        m_FlagConnection = 1;
        NoConnection.SetActive(false);


    }

    public void ChatPush()
    {
        // Random conversation between participants
        GameObjectHelper.SetVisible(m_chatting[Random.Range(0, 3)], true);

    }
    public void ChatPop()
    {
        // Elimina cualquier conversación 
        GameObjectHelper.SetVisible(m_chatting[0], false);
        GameObjectHelper.SetVisible(m_chatting[1], false);
        GameObjectHelper.SetVisible(m_chatting[2], false);
        GameObjectHelper.SetVisible(m_chatting[3], false);
        GameObjectHelper.SetVisible(m_MessageA_Image, false);
        GameObjectHelper.SetVisible(m_MessageB_Image, false);
        m_MessageA.SetActive(false);
        m_MessageB.SetActive(false);
    }

    public void MessagePush()
    {
        // Random conversation between participants 
        m_MessageA.SetActive(true);
        m_MessageB.SetActive(true);

    }

    public void MessageAPop()
    {
        // Random appearance of message options
        m_MessageA.SetActive(false);
        GameObjectHelper.SetVisible(m_MessageA_Image, true);
    }

    public void MessageBPop()
    {
        // Random appearance of message options
        m_MessageB.SetActive(false);
        GameObjectHelper.SetVisible(m_MessageB_Image, true);
    }

}
