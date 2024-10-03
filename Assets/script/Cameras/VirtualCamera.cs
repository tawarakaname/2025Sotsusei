using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // このオブジェクトをシーンが切り替わっても消えないようにする
        DontDestroyOnLoad(gameObject);
    }

}
