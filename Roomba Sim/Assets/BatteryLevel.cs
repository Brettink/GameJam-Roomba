using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryLevel : MonoBehaviour
{
    public float fMaxLevel;
    public float fBaseDelta;
    public AudioSource m_asAlarm;

    private MeshRenderer[] m_amBatteryLights;
    private float[] m_afBatteryLevels;
    private float m_fCurrentLevel;
    private bool m_bLowRange;

    // Start is called before the first frame update
    void Start()
    {
        m_amBatteryLights = GetComponentsInChildren<MeshRenderer>();
        int iCount = m_amBatteryLights.Length;
        m_afBatteryLevels = new float[iCount];
        float fStep = fMaxLevel / (iCount*2.0f);
        for (int iStep = 0;iStep <iCount;iStep++)
        {
            m_afBatteryLevels[iStep] = fStep + (fStep * 2 * iStep);
            //m_amBatteryLights[iStep].material.EnableKeyword("_EMISSION");
        }
        m_fCurrentLevel = fMaxLevel;
        m_bLowRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        m_fCurrentLevel -= (fBaseDelta * Time.deltaTime);
        Color cShowColor =  Color.green;
        if (m_fCurrentLevel < fMaxLevel * 0.375f)
        {
            if (m_bLowRange == false)
            {
                //Play alert sound
                m_asAlarm.Play();
            }
            cShowColor = Color.red;
            m_bLowRange = true;
        }
        for (int iStep = 0; iStep < m_afBatteryLevels.Length; iStep++)
        {
            if (m_fCurrentLevel > m_afBatteryLevels[iStep])
            {
                //m_amBatteryLights[iStep].material.SetColor("_EMISSION", cShowColor);
                m_amBatteryLights[iStep].material.color = cShowColor;
            } else
            {
                //m_amBatteryLights[iStep].material.SetColor("_EMISSION", Color.black);
                m_amBatteryLights[iStep].material.color = Color.black;
            }
        }
    }

    public void SetChargeLevelAbsolute(float fChargePercent)
    {
        float fChargeTo = Mathf.Clamp(fChargePercent, 0, 1);
        m_fCurrentLevel = fMaxLevel * fChargePercent;
        m_bLowRange = fChargeTo < 0.375f;
    }

    public void AddChargeLevelRelative(float fChargePerSecond)
    {
        m_fCurrentLevel += (fMaxLevel * fChargePerSecond * Time.deltaTime);
        m_bLowRange = m_fCurrentLevel < (0.4f * fMaxLevel);
    }
}
