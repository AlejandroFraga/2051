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

    public int m_RandomFactor=9

//    public List<string> m_ChatEvents = default;

//    public Image m_MessageA = default;

//    public Image m_MessageB = default;

//    public Image m_RandomMessage = default;





    // Start is called before the first frame update
    void Start()
    {
        m_RandomFactorHolder= m_RandomFactor
    }

    // Update to modify hour and battery
    void Update() 
    { 
        if (Time.time > m_NextBatteryTime) 
        {
            m_NextBatteryTime = Time.time + m_UpdatePeriodBattery;
            UpdateBattery();
            UpdateHour();



        }
        if (Time.time > m_NextConnectionTime)
        {
            m_NextConnectionTime = Time.time + m_ConnectionCheckPeriod;
            UpdateConnection();



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
            SpawnLowBattery();



        }
        else {
            m_RandomFactor--
        }

    }

    private void SpawnLowBattery()
    {
        // LowBattery Message (called from battery degradation)


    }

    private void HoldLowBattery()
    {
        // LowBattery Message (called from battery degradation)


    }

    public void ChatPop()
    {
        // Random conversation between participants


    }

    public void MessagePop()
    {
        // Random appearance of message options

        
    }



}
