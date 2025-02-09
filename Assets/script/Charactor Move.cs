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
        SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
        GameObject warpObject = collision.gameObject; // ワープオブジェクトの参照
        string warpName = warpObject.name;

        // warpAtoBに衝突した場合、フラグが有効ならシーン遷移
        if (collision.gameObject.name == "warpAtoB")
        {
            bool isAdooropenFlagOn = FlagManager.Instance.GetFlag(FlagManager.FlagType.Adooropen);
            if (isAdooropenFlagOn)
            {
                transitionManager.LoadScene("B");
                this.transform.position = new Vector3(-5.7f, 0, 8);
                FlagManager.Instance.SetFlag(FlagManager.FlagType.comebackA, false);
                FlagManager.Instance.SetFlag(FlagManager.FlagType.comebackAtoB, true);
            }
        }
        if (collision.gameObject.name == "warpBtoA")
        {
            transitionManager.LoadScene("A");
             this.transform.position = new Vector3(0f, 0.84f, -3.4f); // シーン遷移後の新しい位置を設定
             FlagManager.Instance.SetFlag(FlagManager.FlagType.comebackA, true);
             FlagManager.Instance.SetFlag(FlagManager.FlagType.comebackAtoB, false);
            


        }
        // warpBtoCはそのまま動作
        if (collision.gameObject.name == "warpBtoC")
        {
            transitionManager.LoadScene("C");
            this.transform.position = new Vector3(0, 10, 8);
            FlagManager.Instance.SetFlag(FlagManager.FlagType.comebackB, false);
            FlagManager.Instance.SetFlag(FlagManager.FlagType.comebackBtoC,true);
        }

        if (collision.gameObject.name == "warpCtoB")
        {
            transitionManager.LoadScene("B");
            this.transform.position = new Vector3(0, 0, 8); // シーン遷移後の新しい位置を設定
            FlagManager.Instance.SetFlag(FlagManager.FlagType.comebackB, true);
            FlagManager.Instance.SetFlag(FlagManager.FlagType.comebackBtoC, false);


        }
        if (collision.gameObject.name == "warpZtoA")
        {
            transitionManager.LoadScene("A");
            this.transform.position = new Vector3(0, 0, 8); // シーン遷移後の新しい位置を設定
            warpObject.SetActive(false); // ワープオブジェクトを無効化
        }
        if (collision.gameObject.name == "warpZtoB")
        {
            transitionManager.LoadScene("B");
            this.transform.position = new Vector3(0, 0, 8); // シーン遷移後の新しい位置を設定
            warpObject.SetActive(false); // ワープオブジェクトを無効化

        }
        if (collision.gameObject.name == "warpZtoC")
        {
            transitionManager.LoadScene("C");
            this.transform.position = new Vector3(0, 0, 8); // シーン遷移後の新しい位置を設定
            warpObject.SetActive(false); // ワープオブジェクトを無効化

        }
    }
}
