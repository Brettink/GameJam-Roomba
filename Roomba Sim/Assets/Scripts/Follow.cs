using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public float dist;
    private float lastRot, angleTo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate() {
        //float angle = GMan.playerLoc.rotation.eulerAngles.y;
        //angleTo = angle - lastRot;
        //lastRot = angle;
        //transform.RotateAround(GMan.playerLoc.position, Vector3.up, angleTo);
        //Vector3 targetDir = GMan.playerLoc.position - transform.position;

        //Vector3 pos = GMan.playerLoc.position;
        //Vector3 selfPos = transform.position;
        //transform.position = new Vector3(selfPos.x, pos.y + 1f, selfPos.z);
    }
}
