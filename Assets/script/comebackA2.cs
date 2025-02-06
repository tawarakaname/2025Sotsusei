using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class comebackA2 : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;   // 位置を移動させたいオブジェクト
    [SerializeField] private Vector3 targetPosition;    // 移動先の位置
    [SerializeField] private Collider triggerCollider;


    public PlayableDirector director;
    private FlagManager flagManager;                          // フラグマネージャー


    //private bool isDoorOpened = false; // ドアが開いたかどうかのフラグ


    void Start()
    {
        flagManager = FlagManager.Instance;
        // PlayableDirectorの停止イベントにハンドラーを登録
        if (director != null)
        {
            director.stopped += OnPlayableDirectorStopped;
        }
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackAanim))
        {
            return; // Updateメソッドを早期終了
        }

        if (FlagManager.Instance.GetFlagByType(Item.Type.BTB) && (FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackA)))
        {
            MoveTargetObject();
            if (triggerCollider != null)
            {
                Debug.Log("trueだや");
                triggerCollider.enabled = true;
            }
        }


        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackAanim))
        {
            // comebackAフラグがtrueになった場合にオブジェクトを移動
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackA))
            {
                EnterEvent();
            }
        }
    }
    

    private void EnterEvent()
    {
        // タイムラインの再生
        if (director != null)
        {
            director.Play();
        }

        flagManager.SetFlag(FlagManager.FlagType.Nowanim, true);
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        flagManager.SetFlag(FlagManager.FlagType.Nowanim, false);

    }


    // 指定した位置にオブジェクトを移動させるメソッド
    private void MoveTargetObject()
    {
        if (targetObject != null)
        {
            targetObject.transform.position = targetPosition;
            Debug.Log("2かいめ");
        }

    }
  

}
