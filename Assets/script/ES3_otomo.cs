using UnityEngine.Playables;
using UnityEngine;

public class ES3_otomo : MonoBehaviour
{
    [SerializeField] private MonoBehaviour playerScript;      // プレイヤーのスクリプト（操作を無効化するため）
    [SerializeField] private GameObject targetCamera;         // アニメーション後に無効化するカメラ

    private FlagManager flagManager;                          // フラグマネージャー
    public PlayableDirector director;

    void Start()
    {
        flagManager = FlagManager.Instance;

        if (director != null)
        {
            director.stopped += OnPlayableDirectorStopped;
        }
    }


    private void Update()
    {
        if (!flagManager.GetFlag(FlagManager.FlagType.otomokakuho) &&
            flagManager.GetFlag(FlagManager.FlagType.Belt2move))
        {
            otomokakuho();
        }
        else
        {
            return;
        }

    }

    private void otomokakuho()
    {
         director.Play();

         DisablePlayerControls();

         flagManager.SetFlag(FlagManager.FlagType.Nowanim, true);

       
    }

    private void DisablePlayerControls()
    {
        if (playerScript != null)
        {
            playerScript.enabled = false;  // プレイヤーのスクリプトを無効化
        }

    }

    private void EnablePlayerControls()
    {
        if (playerScript != null)
        {
            playerScript.enabled = true;  // プレイヤーのスクリプトを有効化
        }
    }

    // タイムラインが停止したときに呼ばれる
    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        // カメラを非表示
        if (targetCamera != null)
        {
            targetCamera.SetActive(false);
        }

        // フラグを更新
        flagManager.SetFlag(FlagManager.FlagType.Nowanim, false);
        flagManager.SetFlag(FlagManager.FlagType.otomokakuho, true);

        // プレイヤー操作を再び有効化
        EnablePlayerControls();
      
    }

    
}
