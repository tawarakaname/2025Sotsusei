using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class timecounter : MonoBehaviour
{

    private float step_time;

    private void Start()
    {
        Invoke("SceneChange", 10);

    }

    void SceneChange()
    {
        Debug.Log("シーン変えるよ");
        SceneManager.LoadScene("test");

    }

}
