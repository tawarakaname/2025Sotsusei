using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aquarium_allanim : MonoBehaviour
{

    public GameObject Allcup;  // コップのオブジェクト (アニメーションがついているオブジェクト)
    public GameObject wateranimAll;  // コップのオブジェクト (アニメーションがついているオブジェクト)
    private Animator AllcupAnimator;  // 上のオブジェクトのアニメーター
    private Animator wateranimAllAnimator;  // 上のオブジェクトのアニメーター
    private FlagManager flagManager;
    [SerializeField] private Collider Aquarium1collider;
    //[SerializeField] GameObject Colorpasswordobj;
    [SerializeField] Canvas Allcupwater; //itemgetcanvasのことだよー！！！！！！
    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect; // 音声クリップをシリアライズ
    [SerializeField] private GameObject Allcup2; // 最初に非表示のオブジェクト
    [SerializeField] private GameObject setcup; // 最初に表示のオブジェクト

    [SerializeField] private GameObject itemgeteffect;

    // Playerクラスの参照（プレイヤーの操作を無効化するため）
    [SerializeField] private MonoBehaviour playerScript;  // Playerクラスをアタッチ
    private bool controlsDisabled = false;  // 操作無効フラグ
    private bool canvasEnabled = false;  // canvasDgetが有効になったかを追跡するフラグ
    private bool itemgetpanelLogged = false;
    [SerializeField] private float animatedTime;
    private Coroutine Aquarium1waterCoroutine;

    void Start()
    {
        Allcupwater.gameObject.SetActive(false);
        // ueとshitaのAnimatorコンポーネントを取得
        if (Allcup != null) AllcupAnimator = Allcup.GetComponent<Animator>();
        if (wateranimAll!= null) wateranimAllAnimator = wateranimAll.GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
        itemgeteffect.gameObject.SetActive(false);


    }


    void Update()
    {
        // Aquariumcamera0 フラグが true の場合に処理を開始
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Aquariumcamera1))
        {
            return; // 早期リターン
        }
        // Aquariumcamera0 フラグが true の場合に処理を開始
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Bdooropen))
        {
            return; // 早期リターン
        }
        // キャッシュするフラグ
        bool isAllcupFlagActive = FlagManager.Instance.GetFlagByType(Item.Type.allcup);
        bool isAllcup2getFlagActive = FlagManager.Instance.GetFlag(FlagManager.FlagType.Allcupwater);
        bool isCameraZoomFlagActive = FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj);

        // diacup フラグが true かつ Aquarium0water コルーチンがまだ開始されていない場合
        if (isAllcupFlagActive && Aquarium1waterCoroutine == null)
        {
            // プレイヤー操作と Fire1, Fire2 ボタンの入力を無効化
            DisablePlayerControls();
            StartCoroutine(OnClickBbox());
            Aquarium1waterCoroutine = StartCoroutine(BoxBAnimCompleted());

            // canvas がまだ有効化されていない場合のみコルーチンを実行
            if (!canvasEnabled)
            {
                StartCoroutine(EnableCanvasAfterDelay());
                canvasEnabled = true;
            }
        }

        // Fire2が押されたときの処理、条件を全て満たす場合にのみ実行
        if (isAllcupFlagActive && isAllcup2getFlagActive && isCameraZoomFlagActive && Input.GetButtonDown("Fire2"))
        {
            // Canvasを非表示にし、フラグを更新
            Allcupwater.gameObject.SetActive(false);
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, false);

            itemgeteffect.gameObject.SetActive(false);
            playerScript.enabled = true;
        }

        // Fire1 の入力を無効化する処理
        if (controlsDisabled && Input.GetButtonDown("Fire1"))
        {
            return; // Fire1 入力を無視
        }
    }



    private IEnumerator BoxBAnimCompleted()
    {
        yield return new WaitForSeconds(animatedTime);
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Allcupwater, true);
    }

    private IEnumerator OnClickBbox()
    {
        // Diacup2getがまだ開かれていない場合のみアニメーションを再生
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Allcupwater))
        {
            // Fire1, Fire2 入力を無効にするフラグを立てる
            controlsDisabled = true;
            yield return StartCoroutine(PlayAnimationsSimultaneously());
        }
        else
        {
            // 何もしない場合でも、yield return を使って空の処理を返す必要があります
            yield break;
        }
    }

    private IEnumerator PlayAnimationsSimultaneously()
    {
        if (AllcupAnimator != null)
        {
            AllcupAnimator.SetTrigger("faucet0");
            yield return new WaitForSeconds(0.5f); // 0.5秒待機
        }

        if (wateranimAllAnimator != null)
        {
            wateranimAllAnimator.SetTrigger("water&cup");
            yield return new WaitForSeconds(0.5f); // さらに0.5秒待機（必要に応じて）
        }
    }

    private IEnumerator EnableCanvasAfterDelay()
    {
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, true);
        // 1.7秒待機
        yield return new WaitForSeconds(1.1f);
        itemgeteffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Allcupwater.gameObject.SetActive(true);
        Allcup2.SetActive(true);
        setcup.SetActive(false);

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
