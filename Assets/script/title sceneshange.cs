using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // シーン管理に必要

public class TitleSceneChange : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Fire2ボタンが押されたか確認
        if (Input.GetButtonDown("Fire2"))
        {
            // コルーチンを開始
            StartCoroutine(WaitAndChangeScene());
        }
    }

    // シーン遷移を遅らせるためのコルーチン
    IEnumerator WaitAndChangeScene()
    {
        // 2秒待機
        yield return new WaitForSeconds(2f);

        // シーンを"OP"に切り替え
        SceneManager.LoadScene("OP");
    }

}
