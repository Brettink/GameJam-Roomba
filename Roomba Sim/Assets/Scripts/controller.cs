using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    public AudioSource Clip;
    public AudioClip Startup, Loop, Winddown, RobbitJump, Bump;
    float rot = 0f;
    public float jumpRate = 10f, speed = 1f, 
        boostSpeed = 75f, turnSpeed = 1.5f, 
        normalMax = 8f, boostMax = 20f;
    float timeToFill = 0f;
    bool canJump = true;
    public MeshRenderer rend;
    Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    public void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "Floor") {
            canJump = true;
        }
        if (col.gameObject.tag != "Floor") {
            SwitchClip(Bump, .7f);
        }
    }
    public void OnCollisionStay(Collision col) {
        if (col.gameObject.tag == "Floor") {
            canJump = true;
        }
    }

    public void OnCollisionExit(Collision col) {
        if (col.gameObject.tag == "Floor") {
            canJump = false;
        }
    }

    public void OnTriggerEnter(Collider col) {
        if (col.name == "coll") {
            col.transform.parent.gameObject.GetComponent<CleanObject>().aud.Play();
            StartCoroutine(Wait(col.transform.parent.gameObject));
        }
        if (col.name == "Charger") {
            Material[] mats = rend.materials;
            mats[1] = GMan.self.outliner2;
            rend.materials = mats;
        }
    }

    public void OnTriggerExit(Collider col) {
        if (col.name == "Charger") {
            Material[] mats = rend.materials;
            mats[1] = null;
            rend.materials = mats;
        }
    }

    public void OnTriggerStay(Collider col) {
        if (col.name == "Charger") {
            Debug.Log("Charge");
            GMan.self.batLvlScript.AddChargeLevelRelative(0.25f);
        }
    }

    IEnumerator Wait(GameObject toDestroy) {
        yield return new WaitForSeconds(0.1f);
        Object.Destroy(toDestroy);
        yield return null;
    }

    public void DoBoost() {
        body.AddRelativeForce(Vector3.forward * boostSpeed);
    }

    public void SwitchClip(AudioClip clip) {
        SwitchClip(clip, Clip.volume);
    }

    public void SwitchClip(AudioClip clip, float volAdjust) {
        Clip.Stop();
        Clip.volume = volAdjust;
        Clip.clip = clip;
        Clip.Play();
    }

    // Update is called once per frame
    void Update()
    {
        bool isBoost = false;
        //Vector3 rotA = transform.eulerAngles;
        if (body.velocity.y < -.5f) {
            Vector3 myRot = transform.rotation.eulerAngles;
            myRot.x = Mathf.Lerp(myRot.x, 0f, myRot.x / 3f);
            myRot.z = Mathf.Lerp(myRot.z, 0f, myRot.z / 3f);
            transform.rotation = Quaternion.Euler(myRot);
        }
        if (Input.GetKey(KeyCode.A)) { rot = -turnSpeed; } 
        else if (Input.GetKey(KeyCode.D)) { rot = turnSpeed; } 
        else { rot = 0; }
        if (rot != 0) {
            transform.Rotate(Vector3.up, rot);
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            SwitchClip(Startup);
        }
        if (Input.GetKey(KeyCode.W)) {
            if (!Clip.isPlaying) {
                SwitchClip(Loop);
            }
            if (canJump) {
                body.AddRelativeForce(Vector3.forward*speed, ForceMode.Impulse);
            }
        }
        if (Input.GetKeyUp(KeyCode.W)) {
            SwitchClip(Winddown);
        }
        if (Input.GetKey(KeyCode.LeftShift)) {
            if (!GMan.hardRefill) {
                isBoost = true;
                GMan.self.SendMessage("MinusP");
            }
        } else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            isBoost = false;
            timeToFill = Time.fixedTime + 1f;
        } else {
            if (Time.time >= timeToFill) {
                GMan.self.SendMessage("NoBoost");
            }
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (canJump) {
                SwitchClip(RobbitJump);
            }
        }
        if (Input.GetKey(KeyCode.Space)) {
            if (canJump) {
                body.AddRelativeForce(Vector3.up * jumpRate, ForceMode.Impulse);
            }
        }
        body.velocity = Vector3.ClampMagnitude(body.velocity, (isBoost)?boostMax:normalMax);
        //Debug.Log(transform.rotation.y);
    }
}
