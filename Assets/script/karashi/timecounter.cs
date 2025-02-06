using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

//OP専用のシーンチェンジコード
//他のシーンには持ち越さない

public class timecounter : MonoBehaviour
{

    private void Start()
    {
        Invoke("SceneChange", 36f);

    }

    void SceneChange()
    {
        SceneManager.LoadScene("Z");

    }

}
