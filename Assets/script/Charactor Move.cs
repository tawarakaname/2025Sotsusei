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

        // warpAtoBに衝突した場合、フラグが有効ならシーン遷移
        if (collision.gameObject.name == "warpAtoB")
        {
            bool isAdooropenFlagOn = FlagManager.Instance.GetFlag(FlagManager.FlagType.Adooropen);
            if (isAdooropenFlagOn)
            {
                transitionManager.LoadScene("B");
                this.transform.position = new Vector3(0, 0, 8);
                FlagManager.Instance.SetFlag(FlagManager.FlagType.comebackA, false);
            }
        }
        if (collision.gameObject.name == "warpBtoA")
        {
            transitionManager.LoadScene("A");
             this.transform.position = new Vector3(0, 0, 8); // シーン遷移後の新しい位置を設定
             FlagManager.Instance.SetFlag(FlagManager.FlagType.comebackA, true);


        }
        // warpBtoCはそのまま動作
        if (collision.gameObject.name == "warpBtoC")
        {
            transitionManager.LoadScene("C");
            this.transform.position = new Vector3(0, 10, 8);
            FlagManager.Instance.SetFlag(FlagManager.FlagType.comebackB, false);
        }

        if (collision.gameObject.name == "warpCtoB")
        {
            transitionManager.LoadScene("B");
            this.transform.position = new Vector3(0, 0, 8); // シーン遷移後の新しい位置を設定
            FlagManager.Instance.SetFlag(FlagManager.FlagType.comebackB, true);


        }
    }
}
