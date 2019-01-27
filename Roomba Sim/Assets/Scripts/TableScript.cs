using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableScript : MonoBehaviour
{
    Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        body = transform.GetComponent<Rigidbody>();
    }

    void OnTriggerStay(Collider col) {
        if (col.gameObject.tag == "Floor") {
            body.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationY;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
