using System.Collections;
using UnityEngine;

public class Aquarium_diaanim : MonoBehaviour
{
    public GameObject Diacup;  // コップのオブジェクト (アニメーションがついているオブジェクト)
    public GameObject wateranimDia;  // コップのオブジェクト (アニメーションがついているオブジェクト)
    private Animator DiacupAnimator;  // 上のオブジェクトのアニメーター
    private Animator wateranimDiaAnimator;  // 上のオブジェクトのアニメーター
    private FlagManager flagManager;
    [SerializeField] private Collider Aquarium0collider;
    //[SerializeField] GameObject Colorpasswordobj;
    [SerializeField] Canvas Diacup2get; //itemgetcanvasのことだよー！！！！！！
    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect; // 音声クリップをシリアライズ
    [SerializeField] private GameObject Diacup2; // 最初に非表示のオブジェクト
    
    [SerializeField] private GameObject setcup; // 最初に表示のオブジェクト

    // Playerクラスの参照（プレイヤーの操作を無効化するため）
    [SerializeField] private MonoBehaviour playerScript;  // Playerクラスをアタッチ
    private bool controlsDisabled = false;  // 操作無効フラグ
    private bool canvasEnabled = false;  // canvasDgetが有効になったかを追跡するフラグ
    private bool itemgetpanelLogged = false;
    [SerializeField] private float animatedTime;
    private Coroutine Aquarium0waterCoroutine;

    [SerializeField] private GameObject itemgeteffect;

    void Start()
    {
        Diacup2get.gameObject.SetActive(false);
        // ueとshitaのAnimatorコンポーネントを取得
        if (Diacup != null) DiacupAnimator = Diacup.GetComponent<Animator>();
        if (wateranimDia != null) wateranimDiaAnimator = wateranimDia.GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
        itemgeteffect.gameObject.SetActive(false);

    }


    void Update()
    {
        // Aquariumcamera0 フラグが true の場合に処理を開始
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Aquariumcamera0))
        {
            return; // 早期リターン
        }

        // キャッシュするフラグ
        bool isDiacupFlagActive = FlagManager.Instance.GetFlagByType(Item.Type.diacup);
        bool isDiacup2getFlagActive = FlagManager.Instance.GetFlag(FlagManager.FlagType.Diacup2get);
        bool isCameraZoomFlagActive = FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj);


        // diacup フラグが true かつ Aquarium0water コルーチンがまだ開始されていない場合
        if (isDiacupFlagActive && Aquarium0waterCoroutine == null)
        {
            // プレイヤー操作と Fire1, Fire2 ボタンの入力を無効化
            DisablePlayerControls();
            StartCoroutine(OnClickBbox());
            Aquarium0waterCoroutine = StartCoroutine(BoxBAnimCompleted());

            // canvas がまだ有効化されていない場合のみコルーチンを実行
            if (!canvasEnabled)
            {
                StartCoroutine(EnableCanvasAfterDelay());
                canvasEnabled = true;
            }
        }

        // Fire2が押されたときの処理、条件を全て満たす場合にのみ実行
        if (isDiacupFlagActive && isDiacup2getFlagActive && isCameraZoomFlagActive && Input.GetButtonDown("Fire2"))
        {
            // Canvasを非表示にし、フラグを更新
            Diacup2get.gameObject.SetActive(false);
            itemgeteffect.gameObject.SetActive(false);
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, false);
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
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Diacup2get, true);
    }

    private IEnumerator OnClickBbox()
    {
        // Diacup2getがまだ開かれていない場合のみアニメーションを再生
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Diacup2get))
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
        if (DiacupAnimator != null)
        {
            DiacupAnimator.SetTrigger("faucet0");
            yield return new WaitForSeconds(0.5f); // 0.5秒待機
        }

        if (wateranimDiaAnimator != null)
        {
            wateranimDiaAnimator.SetTrigger("water&cup");
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
        Diacup2get.gameObject.SetActive(true);
        Diacup2.SetActive(true);
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

//diacup…item　を使った時のフラグ
//Diacup2get…diacup2を手に入れた時のフラグ