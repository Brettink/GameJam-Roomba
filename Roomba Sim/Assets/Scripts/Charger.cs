using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : MonoBehaviour
{
   void OnTriggerStay(Collider col) {
        if (col.gameObject.tag == "Player") {
            Debug.Log("Charge");
            GMan.self.batLvlScript.AddChargeLevelRelative(0.5f);
        }
    }
}
