using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoCallController : MonoBehaviour
{

    public List<Sprite> m_battery = default;

    public Image m_BatteryImage = default;
    
    public int m_BatteryState = 0;

    public float m_BatteryDuration = 70.0f;

    private float m_NextBatteryTime = 0.0f;

    public float m_UpdatePeriodBattery = 20.0f;

    public List<Sprite> m_Hour = default;

    public Image m_HourImage = default;

    public int m_HourState = 0;

    private float m_NextHourTime = 0.0f;

    public float m_UpdatePeriodHour = 10.0f;

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

    public GameObject m_MessageFinA = default;

    public Image m_MessageFinA_Image = default;

    public GameObject m_MessageFinB = default;

    public Image m_MessageFinB_Image = default;

    public List<Image> m_chatting = default;

    public float m_timer_mensaje = 0.0f;

    public float m_timer_mensaje_period = 2.0f;

    public bool m_IsShaking = false;

    public int m_Shake_Force = 3;

    public Image m_background = default;

    public Image m_background_fin = default;


    // Start is called before the first frame update
    void Start()
    {
        m_FlagConnection = 1;
        m_RandomFactorHolder = m_RandomFactor;
        m_NextBatteryTime = m_UpdatePeriodBattery;
        m_NextHourTime = m_UpdatePeriodHour;
        NoConnection.SetActive(false);
        m_MessageA.SetActive(false);
        m_MessageB.SetActive(false);
        GameObjectHelper.SetVisible(m_MessageA_Image, false);
        GameObjectHelper.SetVisible(m_MessageB_Image, false);
        m_MessageFinA.SetActive(false);
        m_MessageFinB.SetActive(false);
        GameObjectHelper.SetVisible(m_MessageFinA_Image, false);
        GameObjectHelper.SetVisible(m_MessageFinB_Image, false);
        GameObjectHelper.SetVisible(m_chatting[0], false);
        GameObjectHelper.SetVisible(m_chatting[1], false);
        GameObjectHelper.SetVisible(m_chatting[2], false);
        GameObjectHelper.SetVisible(m_chatting[3], false);
        GameObjectHelper.SetVisible(m_background, true);
        GameObjectHelper.SetVisible(m_background_fin, false);

        
        
        
    }

    // Update to modify hour and battery
    void Update() 
    {
        if (m_IsShaking)
        {
            Vector3 position = NoConnection.transform.position;
            position.x += m_Shake_Force;
            m_Shake_Force=-m_Shake_Force;
            NoConnection.transform.position = position;
        }
        
        if (Time.time > m_NextBatteryTime) // Lógica de gasto de batería
        {
            if (m_BatteryState == 3)
            {
                GameObjectHelper.SetVisible(m_background, false);
                GameObjectHelper.SetVisible(m_background_fin, true);
                NoConnection.SetActive(false);
                m_MessageA.SetActive(false);
                m_MessageB.SetActive(false);
                GameObjectHelper.SetVisible(m_MessageA_Image, false);
                GameObjectHelper.SetVisible(m_MessageB_Image, false);
                m_MessageFinA.SetActive(false);
                m_MessageFinB.SetActive(false);
                GameObjectHelper.SetVisible(m_MessageFinA_Image, false);
                GameObjectHelper.SetVisible(m_MessageFinB_Image, false);
                GameObjectHelper.SetVisible(m_chatting[0], false);
                GameObjectHelper.SetVisible(m_chatting[1], false);
                GameObjectHelper.SetVisible(m_chatting[2], false);
                GameObjectHelper.SetVisible(m_chatting[3], false);
                GameObjectHelper.SetVisible(m_BatteryImage, false);
                GameObjectHelper.SetVisible(m_HourImage, false);
                GameObjectHelper.SetVisible(m_ConnectionImage, false);
                return;
            }
            m_NextBatteryTime = Time.time + m_UpdatePeriodBattery;
            UpdateBattery();



        }
        if (Time.time > m_NextHourTime) // Lógica de gasto de batería
        {
            m_NextHourTime = Time.time + m_UpdatePeriodHour;
            UpdateHour();



        }
        if (m_FlagConnection == 1)
        {
            if (Time.time > m_NextConnectionTime) // Lógica pérdida de conexión
            {
                if (m_BatteryState != 3)
                {
                    m_NextConnectionTime = Time.time + m_ConnectionCheckPeriod;
                    UpdateConnection();
                }

            }
            if (Time.time > m_timer_mensaje) // Lógica Mensajes Ajenos
            {
                ChatPop();
                m_timer_mensaje = Time.time + m_timer_mensaje_period;
                if (m_BatteryState == 3)
                {
                    m_timer_mensaje = Time.time + 1000;
                    MessageFinPush();
                }
                else
                {
                    if (Random.Range(0, 10) > 3)
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
    }

    public void UpdateBattery()
    {
        // Update Battery with timer
        m_BatteryState++;
        m_BatteryImage.sprite = m_battery[m_BatteryState];
        if (m_BatteryState==3)
        {
            m_timer_mensaje = Time.time + 2;
            m_NextBatteryTime = Time.time + (m_UpdatePeriodBattery / 4) + 1;
        }

    }

    public void UpdateHour()
    {
        //Update Hour with timer
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
        NoConnection.transform.position = new Vector3(Random.Range(100f, 200f), Random.Range(100f, 200f), 0f) ;
        m_IsShaking = true;
        NoConnection.SetActive(true);


    }

    public void DestroyNoConnection()
    {

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
        GameObjectHelper.SetVisible(m_MessageFinA_Image, false);
        GameObjectHelper.SetVisible(m_MessageFinB_Image, false);
        m_MessageFinA.SetActive(false);
        m_MessageFinB.SetActive(false);
    }

    public void MessagePush()
    {
        // Random conversation between participants 

            m_MessageA.SetActive(true);
            m_MessageB.SetActive(true);
    }
    public void MessageFinPush()
    {
        // Random conversation between participants 

            m_MessageFinA.SetActive(true);
            m_MessageFinB.SetActive(true);
            m_MessageA.SetActive(false);
            m_MessageB.SetActive(false);


    }

    public void MessageAPop()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/VIDEOLLAMADA/Click", transform.position);
        // Random appearance of message options
        m_MessageA.SetActive(false);
            m_MessageB.SetActive(false);
            GameObjectHelper.SetVisible(m_MessageA_Image, true);

    }

    public void MessageFinAPop()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/VIDEOLLAMADA/Click", transform.position);
        // Random appearance of message options
        m_MessageFinA.SetActive(false);
        m_MessageFinB.SetActive(false);
        GameObjectHelper.SetVisible(m_MessageFinA_Image, true);
        GameObjectHelper.SetVisible(m_chatting[0], true);
        GameObjectHelper.SetVisible(m_chatting[1], true);
        GameObjectHelper.SetVisible(m_chatting[2], true);
        GameObjectHelper.SetVisible(m_chatting[3], true);
    }
    public void MessageBPop()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/VIDEOLLAMADA/Click", transform.position);
        // Random appearance of message options
        m_MessageA.SetActive(false);
            m_MessageB.SetActive(false);
            GameObjectHelper.SetVisible(m_MessageB_Image, true);
        

    }

    public void MessageFinBPop()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/VIDEOLLAMADA/Click", transform.position);
        // Random appearance of message options
        m_MessageFinA.SetActive(false);
        m_MessageFinB.SetActive(false);
        GameObjectHelper.SetVisible(m_MessageFinB_Image, true);
        GameObjectHelper.SetVisible(m_chatting[0], true);
        GameObjectHelper.SetVisible(m_chatting[1], true);
        GameObjectHelper.SetVisible(m_chatting[2], true);
        GameObjectHelper.SetVisible(m_chatting[3], true);
    }

}
