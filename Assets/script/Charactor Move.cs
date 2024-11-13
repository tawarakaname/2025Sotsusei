using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharactorMove : MonoBehaviour
{
    //void Start()
    //{
    //    DontDestroyOnLoad(gameObject);
    //}

    void OnCollisionEnter(Collision collision)
    {
        // フラグの状態を取得
        bool isAdooropenFlagOn = FlagManager.Instance.GetFlag(FlagManager.FlagType.Adooropen);

        // warpAtoBに衝突した場合、フラグが有効ならシーン遷移
        if (collision.gameObject.name == "warpAtoB")
        {
            if (isAdooropenFlagOn) // フラグがオンの場合のみ処理実行
            {
                SceneManager.LoadScene("B");
                this.transform.position = new Vector3(0, 0, 8); // シーン遷移後の新しい位置を設定
                FlagManager.Instance.SetFlag(FlagManager.FlagType.comebackA, false);
            }
        }
        if (collision.gameObject.name == "warpBtoA")
        {
             SceneManager.LoadScene("A");
             this.transform.position = new Vector3(0, 0, 8); // シーン遷移後の新しい位置を設定
             FlagManager.Instance.SetFlag(FlagManager.FlagType.comebackA, true);


        }
        // warpBtoCはそのまま動作
        if (collision.gameObject.name == "warpBtoC")
        {
            SceneManager.LoadScene("C");
            this.transform.position = new Vector3(0, 10, 8);
        }
    }
}
