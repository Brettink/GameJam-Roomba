using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class GMan : MonoBehaviour
{
    public static int difficulty = 1;
    public static int numToClean = (int)(difficulty * Math.Abs(Math.Cos(difficulty) * 20) * 50);
    Bounds roomBounds = new Bounds(Vector3.zero, (Vector3.one - Vector3.up) * 30);
    public GameObject rotator;
    //Dictionary<GameObject, float>
    public GameObject CleanObjPrefab;
    float timeToFin = 0f;
    float batLvlVal = 4f;
    float batLvl {
        get { return batLvlVal; }
        set {
            batLvlVal = value;
            UpdateBatInd();
        }
    }
    public GameObject UI;
    public Transform batLvlMaster;
    GameObject[] bats;
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

    void UpdateBatInd() {
        foreach (GameObject obj in bats) {
            obj.SetActive(false);
        }
        for (int i = 0; i < batLvl; i++) {
            bats[i].SetActive(true);
        }
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
        self = this;
        timeToFin = Time.fixedTime + 60f * difficulty;
        points = UI.transform.GetChild(0).GetComponent<Text>();
        time = UI.transform.GetChild(1).GetComponent<Text>();
        numL = UI.transform.GetChild(2).GetComponent<Text>();
        numL.text = "# Left: " + numToClean;
        normalFill = guageBar.color;
        bats = new GameObject[batLvlMaster.childCount];
        for (int i = 0; i < batLvlMaster.childCount; i++) {
            bats[i] = batLvlMaster.GetChild(i).gameObject;
        }
        playerLoc = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i < numToClean; i++) {
            float x = UnityEngine.Random.Range(roomBounds.min.x, roomBounds.max.x);
            float z = UnityEngine.Random.Range(roomBounds.min.z, roomBounds.max.z);

            Vector3 pos = new Vector3(x, 1f, z);
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
        batLvl -= .01f;
        if (batLvl <= 0f) {
            batLvl = 0f;
        }
        time.text = "Time: " + (int)(timeToFin - Time.fixedTime);
        Vector3 r = rotator.transform.localRotation.eulerAngles;
        r.y+=10f;
        rotator.transform.localRotation = Quaternion.Euler(r);
    }
}
