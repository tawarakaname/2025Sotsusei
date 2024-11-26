using System.Collections;
using UnityEngine;

public class ThreePasopen : MonoBehaviour
{
    public GameObject Threepashuta;  // アニメーション対象のオブジェクト
    private Animator ThreepashutaAnimator; // Animatorコンポーネント
    public Collider BTBgimmickcollider;
    private bool playerInsideCollider = false; // プレイヤーがコライダー内にいるか
    [SerializeField] private float animatedTime; // アニメーション時間

    private FlagManager flagManager; // フラグマネージャーのインスタンス
    private bool animationPlayed = false; // アニメーションが再生済みかを追跡

    void Start()
    {

        flagManager = FlagManager.Instance; // FlagManagerのインスタンスをキャッシュ
        // Animatorコンポーネントを取得
        if (Threepashuta != null)
            ThreepashutaAnimator = Threepashuta.GetComponent<Animator>();
    }

    void Update()
    {
        // FlagManagerの条件に従いスクリプトを無効化する
        if (!flagManager.GetFlagByType(Item.Type.BTB))
        {
            return;
        }
        // プレイヤーがコライダー内にいて、フラグが立っており、まだアニメーションを再生していない場合のみ処理
        if (playerInsideCollider && FlagManager.Instance.GetFlagByType(Item.Type.BTB) && !animationPlayed)
        {
            PlayThreepashutaAnimation();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = true;
            Debug.Log("in");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = false;
        }
    }

    private void PlayThreepashutaAnimation()
    {
        // アニメーションを再生し、フラグを更新
        if (ThreepashutaAnimator != null)
        {
            ThreepashutaAnimator.SetTrigger("huta2open");
            animationPlayed = true; // アニメーション再生済みを記録
            StartCoroutine(SetFlagAfterAnimation());
        }
    }

    private IEnumerator SetFlagAfterAnimation()
    {
        // アニメーションが完了するまで待機
        yield return new WaitForSeconds(animatedTime);

        // フラグを設定
        FlagManager.Instance.SetFlag(FlagManager.FlagType.huta2open, true);
    }
}
