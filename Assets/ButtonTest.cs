using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTest : MonoBehaviour
{
    void Update()
    {
        // PS4コントローラーの⚪︎ボタンは「Fire2」として認識されます
        if (Input.GetButtonDown("Fire0"))
        {
            Debug.Log("⚪︎ボタンが押されました！");
        }
    }
}
