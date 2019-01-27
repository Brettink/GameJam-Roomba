using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class GMan : MonoBehaviour
{
    private class MessArea
    {
        public MessArea()
        {
            m_lsecSections = new List<MessSection>();
            m_fTotalArea = 0;
        }
        private List<MessSection> m_lsecSections;
        private float m_fTotalArea;
        private class MessSection
        {
            public float m_fArea;
            public float m_fWeight;

            private float m_fXMin, m_fXMax, m_fZMin, m_fZMax;
            public MessSection(float fXMin, float fXMax, float fZMin, float fZMax)
            {
                m_fXMin = fXMin;
                m_fXMax = fXMax;
                m_fZMin = fZMin;
                m_fZMax = fZMax;

                m_fArea = (m_fXMax - m_fXMin) * (m_fZMax - m_fZMin);
            }
            public Vector3 GetPointInBounds()
            {
                Vector3 vResult = new Vector3();
                vResult.x = UnityEngine.Random.Range(m_fXMin, m_fXMax);
                vResult.z = UnityEngine.Random.Range(m_fZMin, m_fZMax);
                return vResult;
            }
            public void SetWeight(float fTotalArea)
            {
                m_fWeight = m_fArea / fTotalArea;
            }

        }

        public void AddSection(float fXMin, float fXMax, float fZMin, float fZMax)
        {
            MessSection nuSection = new MessSection(fXMin, fXMax, fZMin, fZMax);
            m_lsecSections.Add(nuSection);
            m_fTotalArea += nuSection.m_fArea;
        }
        public void AddSection(Bounds oBoundary)
        {
            AddSection(
                oBoundary.center.x - oBoundary.extents.x,
                oBoundary.center.x + oBoundary.extents.x,
                oBoundary.center.z - oBoundary.extents.z,
                oBoundary.center.z + oBoundary.extents.z
            );

        }
        public void LockWeights()
        {
            foreach (MessSection oSection in m_lsecSections)
            {
                oSection.SetWeight(m_fTotalArea);
            }
        }

        private MessSection GetSectionFromWeight(float fRandom)
        {
            float fWeightSoFar = 0;
            foreach (MessSection oSection in m_lsecSections)
            {
                if (fRandom<=(fWeightSoFar+oSection.m_fWeight))
                {
                    return oSection;
                }
                fWeightSoFar += oSection.m_fWeight;
            }
            return m_lsecSections[m_lsecSections.Count - 1];
        }

        public Vector3 GetPoint()
        {
            float fRand = UnityEngine.Random.Range(0.0f, 1.0f);
            return GetSectionFromWeight(fRand).GetPointInBounds();
        }
    }


    public Material outliner;
    public static int difficulty = 1;
    public static int numToClean = (int)(difficulty * Math.Abs(Math.Cos(difficulty) * 20) * 15);

    public List<BoxCollider> m_lPotentialDirtyAreas;
    Bounds roomBounds;
    public GameObject rotator;
    //Dictionary<GameObject, float>
    public GameObject CleanObjPrefab;
    float timeToFin = 0f;

    public GameObject UI;

    

    private Text points, time, numL;
    public Image guageBar;
    public static GMan self;
    float boostPercVal = 1f;
    bool isFilling = false;
    public static bool hardRefill = false;
    Color normalFill, hardFill = Color.red;
    public float boostPerc {
        get { return boostPercVal;  }
        set {
            boostPercVal = value;
            if (value == 0f) {
                hardRefill = true;
            } else if (value == 1f) {
                hardRefill = false;
            }
            UpdateGuage();
        }
    }
    public static Transform playerLoc;
    // Start is called before the first frame update
    void UpdateGuage() {
        guageBar.fillAmount = boostPerc;
        guageBar.color = (hardRefill) ? hardFill : normalFill;
    }

    

    

    public void NoBoost() {
        isFilling = true;
    }

    public void MinusP() {
        isFilling = false;
        boostPerc -= .0125f;
        if (boostPerc <= 0f) {
            boostPerc = 0f;
        } else {
            playerLoc.gameObject.SendMessage("DoBoost");
        }
    }

    void Start()
    {
        roomBounds = GetComponent<BoxCollider>().bounds;
        self = this;
        timeToFin = Time.fixedTime + 60f * difficulty;
        points = UI.transform.GetChild(0).GetComponent<Text>();
        time = UI.transform.GetChild(1).GetComponent<Text>();
        numL = UI.transform.GetChild(2).GetComponent<Text>();
        numL.text = "# Left: " + numToClean;
        normalFill = guageBar.color;
        FillMessyArea(BuildMessyArea());
        playerLoc = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private MessArea BuildMessyArea()
    {
        MessArea oResult = new MessArea();
        foreach (BoxCollider collide in m_lPotentialDirtyAreas)
        {
            collide.isTrigger = true; // just making sure.
            oResult.AddSection(collide.bounds);
        }
        oResult.LockWeights();
        return oResult;
    }

    private void FillMessyArea(MessArea oArea)
    {
        for (int i = 0; i < numToClean; i++)
        {

            Vector3 pos = oArea.GetPoint();
            pos.y = 5f;
            GameObject obj = Instantiate(CleanObjPrefab);
            obj.transform.position = pos;
        }

    }

    void LateUpdate() {
        if (boostPerc < 1f && isFilling) {
            boostPerc += (hardRefill) ? .005f:0.01f;
            if (boostPerc >= 1f) {
                boostPerc = 1f;
            }
        }

        time.text = "Time: " + (int)(timeToFin - Time.fixedTime);
        float numOBjs = GameObject.FindGameObjectsWithTag("ObjC").Length;
        float perC = 100f - ((numOBjs / numToClean) * 100f);
        numL.text = "Cleanliness: " + Math.Round(perC, 2) + "%";
        Vector3 r = rotator.transform.localRotation.eulerAngles;
        r.y+=10f;
        rotator.transform.localRotation = Quaternion.Euler(r);
    }
}
