using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBopen : MonoBehaviour
{
    public GameObject Bue;  // 上のオブジェクト (アニメーションがついているオブジェクト)
    public GameObject Bshita;  // 下のオブジェクト (アニメーションがついているオブジェクト)
    private Animator BueAnimator;  // 上のオブジェクトのアニメーター
    private Animator BshitaAnimator;  // 下のオブジェクトのアニメーター
    private FlagManager flagManager;
    [SerializeField] private Collider BBoxcollider;
    [SerializeField] GameObject Colorpasswordobj;
    [SerializeField] Canvas balloonget;
    private bool playerInsideCollider = false;
    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect; // 音声クリップをシリアライズ

    // Playerクラスの参照（プレイヤーの操作を無効化するため）
    [SerializeField] private MonoBehaviour playerScript;  // Playerクラスをアタッチ
    private bool controlsDisabled = false;  // 操作無効フラグ
    private bool canvasEnabled = false;  // canvasDgetが有効になったかを追跡するフラグ

    void Start()
    {
        balloonget.gameObject.SetActive(false);
        // ueとshitaのAnimatorコンポーネントを取得
        if (Bue != null)
        {
            BueAnimator = Bue.GetComponent<Animator>();
        }
        if (Bshita != null)
        {
            BshitaAnimator = Bshita.GetComponent<Animator>();
        }

        audioSource = GetComponent<AudioSource>();

    }


    void Update()
    {
        // プレイヤーがコライダー内にいる場合のみ処理を行う
        if (playerInsideCollider)
        {
            // ColorPasswordclear が true かつ Aboxopen がまだ開かれていない場合
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.ColorPasswordclear) &&
                !FlagManager.Instance.GetFlag(FlagManager.FlagType.Bboxopen))
            {
                // プレイヤー操作と Fire1, Fire2 ボタンの入力を無効化
                DisablePlayerControls();

                // アニメーションを再生
                OnClickBbox();
            }

            //  ColorPasswordclear が true で、まだ canvas が有効化されていない場合のみ処理を行う
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.ColorPasswordclear) && !canvasEnabled)
            {
                StartCoroutine(EnableCanvasAfterDelay());
            }

            // 条件をチェック
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.ColorPasswordclear) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.Bboxopen) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.BoxBCamera) &&
                playerInsideCollider &&
                Input.GetButtonDown("Fire2"))
            {
                // Fire2が押されたらCanvasをfalseに
                balloonget.gameObject.SetActive(false);
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

    public void OnClickBbox()
    {
        // Bboxopen がまだ開かれていない場合のみアニメーションを再生
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Bboxopen))
        {
            Colorpasswordobj.SetActive(false);

            // 上と下のアニメーションをできるだけ同時に再生する
            StartCoroutine(PlayAnimationsSimultaneously());

            // Bboxopen フラグを true にして再生を一度だけにする
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Bboxopen, true);

            // Fire1, Fire2 入力を無効にするフラグを立てる
            controlsDisabled = true;
        }
    }

    private IEnumerator PlayAnimationsSimultaneously()
    {
        // 両方のAnimatorに同時にトリガーを設定する
        if (BueAnimator != null)
        {
            BueAnimator.SetTrigger("Bboxopen");
        }
        if (BshitaAnimator != null)
        {
            BshitaAnimator.SetTrigger("Bboxopen");
        }

        // 次のフレームまで待機して処理を続行
        yield return null;
        yield return null;
        yield return null;// これで1フレーム待機し、同時に再生されるようにする
    }

    private IEnumerator EnableCanvasAfterDelay()
    {
        // 1秒待機
        yield return new WaitForSeconds(1.7f);

        // balloongetをtrueにする
        if (!canvasEnabled)  // まだcanvasが有効化されていない場合のみ実行
        {
            balloonget.gameObject.SetActive(true);
            audioSource.PlayOneShot(soundEffect); // 音声クリップを再生
            Debug.Log("正解のSEを流します");


            FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, true);

            // canvas が有効化されたことを記録
            canvasEnabled = true; // ここでフラグを設定
        }

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
