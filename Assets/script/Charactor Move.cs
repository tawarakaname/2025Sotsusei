using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SphereMove : MonoBehaviour
{

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "warpAtoB")
        {
            SceneManager.LoadScene("B");
            this.transform.position = new Vector3(0, 0, 8);
        }
        if (collision.gameObject.name == "warpBtoC")
        {
            SceneManager.LoadScene("C");
            this.transform.position = new Vector3(0, 10, 8);
        }
    }
}