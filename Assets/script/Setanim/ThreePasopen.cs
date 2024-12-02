using System.Collections;
using UnityEngine;

public class ThreePasopen : MonoBehaviour
{
    public GameObject Threepashuta;  // アニメーション対象のオブジェクト
    public GameObject gaslamp;  // アニメーション対象のオブジェクト
    private Animator ThreepashutaAnimator; // Animatorコンポーネント
    private Animator gaslampAnimator; // Animatorコンポーネント
    public Collider BTBgimmickcollider;
    private bool playerInsideCollider = false; // プレイヤーがコライダー内にいるか
    [SerializeField] private float gaslampAnimatedTime; // gaslamp アニメーション時間
    [SerializeField] private float threepashutaDelay = 0.5f; // gaslamp アニメーション後の遅延時間

    private FlagManager flagManager; // フラグマネージャーのインスタンス
    private bool animationPlayed = false; // アニメーションが再生済みかを追跡
    [SerializeField] private MonoBehaviour playerScript; // プレイヤー操作スクリプトを参照

    void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスをキャッシュ
        if (Threepashuta != null)
            ThreepashutaAnimator = Threepashuta.GetComponent<Animator>();
        if (gaslamp != null)
            gaslampAnimator = gaslamp.GetComponent<Animator>();
    }

    void Update()
    {
        if (!flagManager.GetFlagByType(Item.Type.BTB))
        {
            return;
        }
        // プレイヤーがコライダー内にいて、フラグが立っており、まだアニメーションを再生していない場合のみ処理
        if (playerInsideCollider && flagManager.GetFlagByType(Item.Type.BTB) && !animationPlayed)
        {
            PlayGaslampAnimation();
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

    private void PlayGaslampAnimation()
    {
        // gaslamp のアニメーションを再生し、その後 Threepashuta を再生
        if (gaslampAnimator != null)
        {
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Setanim, true);
            DisablePlayerControls(); // プレイヤー操作を無効化
            gaslampAnimator.SetTrigger("gaslamp");
            animationPlayed = true; // アニメーション再生済みを記録
            StartCoroutine(PlayThreepashutaAnimationAfterDelay());
        }
    }

    private IEnumerator PlayThreepashutaAnimationAfterDelay()
    {
        // gaslamp アニメーションが完了するまで待機
        yield return new WaitForSeconds(gaslampAnimatedTime + threepashutaDelay);

        // Threepashuta のアニメーションを再生
        if (ThreepashutaAnimator != null)
        {
            ThreepashutaAnimator.SetTrigger("huta2open");
            StartCoroutine(SetFlagAfterAnimation());
        }
    }

    private IEnumerator SetFlagAfterAnimation()
    {
        // Threepashuta アニメーションが完了するまで待機
        yield return new WaitForSeconds(gaslampAnimatedTime);

        // フラグを設定
        FlagManager.Instance.SetFlag(FlagManager.FlagType.huta2open, true);
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Setanim, false);

        EnablePlayerControls(); // プレイヤー操作を再有効化
    }

    private void DisablePlayerControls()
    {
        if (playerScript != null)
        {
            playerScript.enabled = false;
            Debug.Log("Player controls disabled");
        }
    }

    private void EnablePlayerControls()
    {
        if (playerScript != null)
        {
            playerScript.enabled = true;
            Debug.Log("Player controls enabled");
        }
    }
}
