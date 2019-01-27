using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;

public class CleanObject : MonoBehaviour
{
    // Start is called before the first frame update
    private enum Type {
        Loon, Toon, Nick, Penn, Quart, Dime, Cup, Chip, Paper, Puddle
    }
    private Type type;
    public AudioSource aud;
    private AudioClip clip;
    void Start()
    {
        //Debug.Log(Enum.GetValues(Type.Cup.GetType()).Length);
        type = (Type)UnityEngine.Random.Range(0, Enum.GetValues(Type.Cup.GetType()).Length);
        GameObject obj = Instantiate(
            Resources.Load<GameObject>("CleanObjs/" + type.ToString()), transform);
//        Debug.Log((int)type);
        MeshRenderer meshR = obj.transform.GetChild(0).GetComponent<MeshRenderer>();
        Material[] mats = new Material[2];
        mats[0] = meshR.materials[0];
        mats[1] = GMan.self.outliner;
        meshR.materials = mats;
        if ((int)type < 6) {
            clip = Resources.Load<AudioClip>("Audio/Sound/CoinClink");
            aud.clip = clip;
            aud.volume -= .2f;
        } else {
            try {
                clip = Resources.Load<AudioClip>("Audio/Sound/" + type.ToString());
                aud.clip = clip;
            } catch (Exception e) {

            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
