using UnityEngine;
using UnityEngine.Playables;

public class ComebackB2 : MonoBehaviour
{
    //やっぱりこっちがcomeback1にしました

    [SerializeField] private GameObject targetObject;   // 位置を移動させたいオブジェクト
    [SerializeField] private Vector3 targetPosition;    // 移動先の位置
    [SerializeField] private Collider triggerCollider;
    [SerializeField] private PlayableDirector director; // 再生させるDirector
    //[SerializeField] private GameObject sandereffect;   // 位置を移動させたいオブジェクト

    private bool hasMovedTarget = false;    // オブジェクトを移動したかどうかを追跡
    private FlagManager flagManager;

    void Start()
    {
        flagManager = FlagManager.Instance;
        // PlayableDirectorの停止イベントにハンドラーを登録
        if (director != null)
        {
            director.stopped += OnPlayableDirectorStopped;
        }

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Bdooropen) && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Leverdown))
        {
            // コライダーを無効化
            if (triggerCollider != null)
            {
                triggerCollider.enabled = false;
            }
        }

        // Adooropen と comebackA フラグが true かつ、初回再生時のみDirectorを再生
        if (ShouldPlayDirector())
        {
            MoveTargetObject();
            director.Play();
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Nowanim, true); // Nowanim フラグを true に設定
        }

        // Adooropen と comebackA フラグが true かつ、初回再生時のみDirectorを再生
        if (ShouldPlayDirectorCtoB())
        {
            director.Play();
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Nowanim, true); // Nowanim フラグを true に設定
        }
    }
   

    // 指定した位置にオブジェクトを移動させるメソッド
    private void MoveTargetObject()
    {
        if (!hasMovedTarget && targetObject != null)
        {
            targetObject.transform.position = targetPosition;
        }
    }

    // Director再生条件をチェックするメソッド
    private bool ShouldPlayDirector()
    {
        return FlagManager.Instance.GetFlag(FlagManager.FlagType.Bdooropen)
               && FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackB)
               && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Nowanim)
               && !FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackBanim)
               && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Leverdown);
    }
    private bool ShouldPlayDirectorCtoB()
    {
        return FlagManager.Instance.GetFlag(FlagManager.FlagType.Bdooropen)
               && FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackB)
               && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Nowanim)
               && FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackBanim)
               && FlagManager.Instance.GetFlag(FlagManager.FlagType.Leverdown);
    }

    // タイムラインが停止したときに呼ばれる
    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        //sandereffect.SetActive(true);

        // Nowanim フラグを false に設定
        if (flagManager != null)
        {
            flagManager.SetFlag(FlagManager.FlagType.Nowanim, false);
        }

    }
}
