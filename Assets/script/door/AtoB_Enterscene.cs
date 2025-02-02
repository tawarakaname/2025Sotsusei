using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AtoB_Enterscene : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;    // プレイヤー
    [SerializeField] private Vector3 playerPosition;    // プレイヤー移動先の位置


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

        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Bstartsceneok))
        {
            return; // Updateメソッドを早期終了
        }
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackB))
        {
            return; // Updateメソッドを早期終了
        }

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Bstartsceneok) &&
            (FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackAtoB) &&
            (!FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackB))))
        {
            EnterEvent();
        }

    }


    private void EnterEvent()
    {
        if (playerObject != null)
        {
            playerObject.transform.position = playerPosition;
        }

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

}
