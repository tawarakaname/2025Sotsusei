using System.Collections;
using UnityEngine;

public class Telop_A : MonoBehaviour
{
    [SerializeField] private MonoBehaviour playerScript;  // Playerスクリプトの参照
    public GameObject Telop_Aanim;                        // アニメーションがついているオブジェクト
    private Animator Telop_AAnimator;                     // アニメーター
    [SerializeField] private float delayAfterAnim = 1f;   // アニメーション後の遅延時間
    [SerializeField] private string triggerName = "Telop_A"; // アニメーション用のトリガー名

    void Start()
    {
        // Adooropenフラグがtrueの場合
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Adooropen))
        {
            // アニメーションオブジェクトを非表示にする
            if (Telop_Aanim != null)
            {
                Telop_Aanim.SetActive(false);
            }

            // このスクリプトを無効化
            this.enabled = false;
            return;
        }

        // スクリプトを無効化
        playerScript.enabled = false;

        // アニメーターを取得
        if (Telop_Aanim != null) Telop_AAnimator = Telop_Aanim.GetComponent<Animator>();

        // アニメーション再生の監視を開始
        StartCoroutine(HandleAnimationAndDelay());
    }

    private IEnumerator HandleAnimationAndDelay()
    {
        // アニメーション再生を開始（トリガーを設定）
        if (Telop_AAnimator != null && !string.IsNullOrEmpty(triggerName))
        {
            Telop_AAnimator.SetTrigger(triggerName);
        }

        // アニメーション開始フラグをセット
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Telop, true);

        // アニメーションが終了するまで待機
        while (Telop_AAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f || Telop_AAnimator.IsInTransition(0))
        {
            yield return null;
        }

        // 指定された秒数待機
        yield return new WaitForSeconds(delayAfterAnim);

        // フラグをリセット
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Telop, false);

        // スクリプトを有効化
        playerScript.enabled = true;
    }
}
