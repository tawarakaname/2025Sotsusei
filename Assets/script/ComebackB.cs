using UnityEngine;
using UnityEngine.Playables;

public class ComebackB : MonoBehaviour
{
    //やっぱりこっちがcomeback２にしました

    [SerializeField] private GameObject targetObject;   // 位置を移動させたいオブジェクト
    [SerializeField] private Vector3 targetPosition;    // 移動先の位置
    [SerializeField] private PlayableDirector director; // 再生させるDirector
    [SerializeField] private GameObject targetCamera;   // アニメーション後に無効化するカメラ
    [SerializeField] private Collider triggerCollider;
    [SerializeField] private GameObject playerObject;    // プレイヤー
    [SerializeField] private Vector3 playerPosition;    //　プレイヤー移動先の位置


    private bool hasPlayedDirector = false; // Directorが再生されたかを追跡
    private bool isDoorOpened = false;      // ドアが開いたかどうかのフラグ
    private bool hasMovedTarget = false;    // オブジェクトを移動したかどうかを追跡
    private FlagManager flagManager;

    void Start()
    {
        flagManager = FlagManager.Instance;

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Bdooropen) && FlagManager.Instance.GetFlag(FlagManager.FlagType.Leverdown))
        {// コライダーを無効化
            if (triggerCollider != null)
            {
                triggerCollider.enabled = true;
            }
        }

        
        // comebackA フラグが true になったらオブジェクトの位置を移動
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackB) && !isDoorOpened)
        {
            MoveTargetObject();
        }

        // Adooropen と comebackA フラグが true かつ、初回再生時のみDirectorを再生
        if (ShouldPlayDirector())
        {
            // PlayableDirectorの停止イベントにハンドラーを登録
            if (director != null)
            {
                director.stopped += OnPlayableDirectorStopped;
            }

            director.Play();
            hasPlayedDirector = true; // 再生済みと記録
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Nowanim, true); // Nowanim フラグを true に設定

        }
    }
 

    // 指定した位置にオブジェクトを移動させるメソッド
    private void MoveTargetObject()
    {
        if (!hasMovedTarget && targetObject != null)
        {
            targetObject.transform.position = targetPosition;
            hasMovedTarget = true; // 移動済みと記録
            Debug.Log("2回目の移動完了");
        }
        if (playerObject != null)
        {
            playerObject.transform.position = playerPosition;
        }
    }

    // Director再生条件をチェックするメソッド
    private bool ShouldPlayDirector()
    {
        return FlagManager.Instance.GetFlag(FlagManager.FlagType.Bdooropen)
               && FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackB)
               && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Nowanim)
               && !FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackBanim)
               && FlagManager.Instance.GetFlag(FlagManager.FlagType.Leverdown)
               && !hasPlayedDirector;
    }

    // タイムラインが停止したときに呼ばれる
    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        // Nowanim フラグを false に設定
        if (flagManager != null)
        {
            flagManager.SetFlag(FlagManager.FlagType.Nowanim, false);
            FlagManager.Instance.SetFlag(FlagManager.FlagType.comebackBanim, true);
        }

        // targetCamera を無効化
        if (targetCamera != null)
        {
            targetCamera.SetActive(false);
        }

        isDoorOpened = true; // ドアが開いたことを記録
    }
}
