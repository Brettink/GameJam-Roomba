using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMus : MonoBehaviour
{
    public static MenuMus self;

    void Start() {
        self = this;
        DontDestroyOnLoad(self);
    }
}
