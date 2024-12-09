using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monitoropen : MonoBehaviour
{
    [SerializeField] private GameObject monitorpasswordobj;
    [SerializeField] private GameObject mixpasswordobj;

    public GameObject monitorpanel;  // アニメーション対象のオブジェクト
    private Animator monitorpanelAnimator; // Animatorコンポーネント
    public Collider Pllarcollider;
    private bool playerInsideCollider = false; // プレイヤーがコライダー内にいるか
    [SerializeField] private float monitorpanelAnimatedTime; // monitorpanel アニメーション時間

    private FlagManager flagManager; // フラグマネージャーのインスタンス
    private bool animationPlayed = false; // アニメーションが再生済みかを追跡
    [SerializeField] private MonoBehaviour playerScript; // プレイヤー操作スクリプトを参照
    [SerializeField] private float animationStartDelay = 0.5f; // アニメーション開始の遅延時間

    void Start()
    {
        flagManager = FlagManager.Instance; // FlagManagerのインスタンスをキャッシュ
        if (monitorpanel != null)
            monitorpanelAnimator = monitorpanel.GetComponent<Animator>();
    }

    void Update()
    {
        if (!flagManager.GetFlag(FlagManager.FlagType.MonitorPasswordclear))
        {
            return;
        }
        // プレイヤーがコライダー内にいて、フラグが立っており、まだアニメーションを再生していない場合のみ処理
        if (playerInsideCollider && flagManager.GetFlag(FlagManager.FlagType.MonitorPasswordclear) && !animationPlayed)
        {
            StartCoroutine(StartAnimationWithDelay());
        }
    }

    private IEnumerator StartAnimationWithDelay()
    {
        // 遅延を追加
        yield return new WaitForSeconds(animationStartDelay);
        monitorpasswordobj.gameObject.SetActive(false);
        PlaymonitorpanelAnimation();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = false;
        }
    }

    private void PlaymonitorpanelAnimation()
    {
        // monitorpanel のアニメーションを再生し、その後 Threepashuta を再生
        if (monitorpanelAnimator != null)
        {
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Setanim, true);
            DisablePlayerControls(); // プレイヤー操作を無効化
            monitorpanelAnimator.SetTrigger("monitoropen");
            animationPlayed = true; // アニメーション再生済みを記録
            StartCoroutine(SetFlagAfterAnimation()); // アニメーション終了後にコルーチンを開始
        }
    }

    private IEnumerator SetFlagAfterAnimation()
    {
        // Threepashuta アニメーションが完了するまで待機
        yield return new WaitForSeconds(monitorpanelAnimatedTime);

        // フラグを設定
        FlagManager.Instance.SetFlag(FlagManager.FlagType.monitorpanelopen, true);
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Setanim, false);

        mixpasswordobj.gameObject.SetActive(true);
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

