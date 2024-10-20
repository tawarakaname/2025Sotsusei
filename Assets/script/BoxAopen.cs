using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxAopen : MonoBehaviour
{
    public GameObject ue;  // 上のオブジェクト (アニメーションがついているオブジェクト)
    public GameObject shita;  // 下のオブジェクト (アニメーションがついているオブジェクト)
    private Animator ueAnimator;  // 上のオブジェクトのアニメーター
    private Animator shitaAnimator;  // 下のオブジェクトのアニメーター
    private FlagManager flagManager;
    [SerializeField] private Collider ABoxcollider;
    [SerializeField] GameObject Illustpasswordobj;
    [SerializeField] Canvas capsuleDget;
    private bool playerInsideCollider = false;

    // Playerクラスの参照（プレイヤーの操作を無効化するため）
    [SerializeField] private MonoBehaviour playerScript;  // Playerクラスをアタッチ
    private bool controlsDisabled = false;  // 操作無効フラグ
    private bool canvasEnabled = false;  // canvasDgetが有効になったかを追跡するフラグ

    void Start()
    {
        capsuleDget.gameObject.SetActive(false);
        // ueとshitaのAnimatorコンポーネントを取得
        if (ue != null)
        {
            ueAnimator = ue.GetComponent<Animator>();
        }
        if (shita != null)
        {
            shitaAnimator = shita.GetComponent<Animator>();
        }
    }

    void Update()
    {
        // プレイヤーがコライダー内にいる場合のみ処理を行う
        if (playerInsideCollider)
        {
            // IllustPasswordclear が true かつ Aboxopen がまだ開かれていない場合
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.IllustPasswordclear) &&
                !FlagManager.Instance.GetFlag(FlagManager.FlagType.Aboxopen))
            {
                // プレイヤー操作と Fire1, Fire2 ボタンの入力を無効化
                DisablePlayerControls();

                // アニメーションを再生
                OnClickAbox();
            }

            // IllustPasswordclear が true で、まだ canvas が有効化されていない場合のみ処理を行う
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.IllustPasswordclear) && !canvasEnabled)
            {
                StartCoroutine(EnableCanvasAfterDelay());
            }

            // 条件をチェック
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.IllustPasswordclear) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.Aboxopen) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.BoxACamera) &&
                playerInsideCollider &&
                Input.GetButtonDown("Fire2"))
            {
                // Fire2が押されたらCanvasをfalseに
                capsuleDget.gameObject.SetActive(false);
                FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, false);
                playerScript.enabled = true;
            }
        }

        // Fire1 と Fire2 の入力を無効化する
        if (controlsDisabled)
        {
            // Fire1, Fire2 の入力を無効化
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
            {
                // 何も行わない（入力を無視）
                return;
            }
        }
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

    public void OnClickAbox()
    {
        // Aboxopen がまだ開かれていない場合のみアニメーションを再生
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Aboxopen))
        {
            Illustpasswordobj.SetActive(false);

            // 上と下のアニメーションをできるだけ同時に再生する
            StartCoroutine(PlayAnimationsSimultaneously());

            // Aboxopen フラグを true にして再生を一度だけにする
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Aboxopen, true);

            // Fire1, Fire2 入力を無効にするフラグを立てる
            controlsDisabled = true;
        }
    }

    private IEnumerator PlayAnimationsSimultaneously()
    {
        // 両方のAnimatorに同時にトリガーを設定する
        if (ueAnimator != null)
        {
            ueAnimator.SetTrigger("Aboxopen");
        }
        if (shitaAnimator != null)
        {
            shitaAnimator.SetTrigger("Aboxopen");
        }

        // 次のフレームまで待機して処理を続行
        yield return null; // これで1フレーム待機し、同時に再生されるようにする
    }

    private IEnumerator EnableCanvasAfterDelay()
    {
        // 1秒待機
        yield return new WaitForSeconds(1f);

        // canvasDgetをtrueにする
        capsuleDget.gameObject.SetActive(true);
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, true);

        // canvas が有効化されたことを記録
        canvasEnabled = true;
    }

    private void DisablePlayerControls()
    {
        // Playerスクリプトを無効化（操作不可にする）
        if (playerScript != null)
        {
            playerScript.enabled = false;
        }

        // Fire1, Fire2 ボタンの入力を無効化する
        controlsDisabled = true;
    }
}
