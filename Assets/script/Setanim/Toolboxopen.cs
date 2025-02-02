using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolboxopen : MonoBehaviour
{
    public GameObject Toolanim;  // 上のオブジェクト (アニメーションがついているオブジェクト)
    private Animator ToolAnimator;  // 上のオブジェクトのアニメーター
    [SerializeField] private Collider ToolBoxcollider;
    [SerializeField] Canvas Dryberget;
    private bool playerInsideCollider = false;
    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect; // 音声クリップをシリアライズ

    [SerializeField] private GameObject Dryber; // 最初に非表示のオブジェクト

    // Playerクラスの参照（プレイヤーの操作を無効化するため）
    [SerializeField] private MonoBehaviour playerScript;  // Playerクラスをアタッチ
    private bool controlsDisabled = false;  // 操作無効フラグ
    private bool canvasEnabled = false;  // canvasDgetが有効になったかを追跡するフラグ
    private bool itemgetpanelLogged = false;
    [SerializeField] private float animatedTime;
    [SerializeField] private GameObject itemgeteffect;
    private Coroutine ToolOpenCoroutine;

    void Start()
    {
        Dryberget.gameObject.SetActive(false);
        // ueとshitaのAnimatorコンポーネントを取得
        if (Toolanim != null) ToolAnimator = Toolanim.GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
        itemgeteffect.gameObject.SetActive(false);

    }


    void Update()
    {
        // プレイヤーがコライダー内にいる場合のみ処理を行う
        if (playerInsideCollider)
        {
            if (playerInsideCollider &&
      FlagManager.Instance.GetFlag(FlagManager.FlagType.toolPasswordclear) &&
      ToolOpenCoroutine == null)
            {
                DisablePlayerControls();
                OnClickToolbox();
                ToolOpenCoroutine = StartCoroutine(ToolAnimCompleted());
            }


            //  ColorPasswordclear が true で、まだ canvas が有効化されていない場合のみ処理を行う
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.toolPasswordclear) && !canvasEnabled)
            {
                Dryber.SetActive(true);
                StartCoroutine(EnableCanvasAfterDelay());
                canvasEnabled = true;  // コルーチンが一度だけ呼ばれるように設定
            }

            // 条件をチェック
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.toolPasswordclear) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.toolboxopen) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.ToolCamera) &&
                playerInsideCollider &&
                Input.GetButtonDown("Fire2"))
            {
                // Fire2が押されたらCanvasをfalseに
                Dryberget.gameObject.SetActive(false);
                itemgeteffect.gameObject.SetActive(false);
                FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, false);
                playerScript.enabled = true;
            }
        }

        // Fire1 と Fire2 の入力を無効化する
        if (controlsDisabled)
        {
            // Fire1, Fire2 の入力を無効化
            if (Input.GetButtonDown("Fire1"))
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

    private IEnumerator ToolAnimCompleted()
    {
        yield return new WaitForSeconds(animatedTime);
        FlagManager.Instance.SetFlag(FlagManager.FlagType.toolboxopen, true);
    }

    public void OnClickToolbox()
    {
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.toolboxopen))
        {
            StartCoroutine(PlayAnimationsSimultaneously());
            controlsDisabled = true;
        }

    }

    private IEnumerator PlayAnimationsSimultaneously()
    {
        if (ToolAnimator != null) ToolAnimator.SetTrigger("toolboxopen");
        yield return null;

    }

    private IEnumerator EnableCanvasAfterDelay()
    {
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, true);
        // 1.7秒待機
        yield return new WaitForSeconds(1f);
        itemgeteffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        Dryberget.gameObject.SetActive(true);

        if (!itemgetpanelLogged)
        {
            itemgetpanelLogged = true;
        }
        audioSource.PlayOneShot(soundEffect); // 音声クリップを再生

    }


    private void DisablePlayerControls()
    {
        if (playerScript != null) playerScript.enabled = false;
        controlsDisabled = true;
    }
}
