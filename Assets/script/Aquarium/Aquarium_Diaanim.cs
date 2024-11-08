using System.Collections;
using UnityEngine;

public class Aquarium_diaanim : MonoBehaviour
{
    public GameObject Diacup;  // コップのオブジェクト (アニメーションがついているオブジェクト)
    private Animator DiacupAnimator;  // 上のオブジェクトのアニメーター
    private FlagManager flagManager;
    [SerializeField] private Collider Aquarium0collider;
    //[SerializeField] GameObject Colorpasswordobj;
    [SerializeField] Canvas Diacup2get; //itemgetcanvasのことだよー！！！！！！
    private bool playerInsideCollider = false;
    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect; // 音声クリップをシリアライズ

    // Playerクラスの参照（プレイヤーの操作を無効化するため）
    [SerializeField] private MonoBehaviour playerScript;  // Playerクラスをアタッチ
    private bool controlsDisabled = false;  // 操作無効フラグ
    private bool canvasEnabled = false;  // canvasDgetが有効になったかを追跡するフラグ
    private bool itemgetpanelLogged = false;
    [SerializeField] private float animatedTime;
    private Coroutine Aquarium0waterCoroutine;

    void Start()
    {
        Diacup2get.gameObject.SetActive(false);
        // ueとshitaのAnimatorコンポーネントを取得
        if (Diacup != null) DiacupAnimator = Diacup.GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();

    }


    void Update()
    {
        // プレイヤーがコライダー内にいる場合のみ処理を行う
        if (playerInsideCollider)
        {
            //  が true かつAquarium0water がまだない場合
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.diacup) &&
                 Aquarium0waterCoroutine == null)
            {
                // プレイヤー操作と Fire1, Fire2 ボタンの入力を無効化
                DisablePlayerControls();
                OnClickBbox();
                Aquarium0waterCoroutine = StartCoroutine(BoxBAnimCompleted());
            }

            //  Diacupが true で、まだ canvas が有効化されていない場合のみ処理を行う
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.diacup) && !canvasEnabled)
            {
                StartCoroutine(EnableCanvasAfterDelay());
                canvasEnabled = true;  // コルーチンが一度だけ呼ばれるように設定
            }

            // 条件をチェック
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.diacup) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.Aquariumcamera0) &&
                playerInsideCollider &&
                Input.GetButtonDown("Fire2"))
            {
                // Fire2が押されたらCanvasをfalseに
                Diacup2get.gameObject.SetActive(false);
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

    private IEnumerator BoxBAnimCompleted()
    {
        yield return new WaitForSeconds(animatedTime);
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Diacup2get, true);
    }

    public void OnClickBbox()
    {
        // Diacup2getがまだ開かれていない場合のみアニメーションを再生
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Diacup2get))
        {
            // Fire1, Fire2 入力を無効にするフラグを立てる
            controlsDisabled = true;
            if (DiacupAnimator != null) DiacupAnimator.SetTrigger("Diacup2get");
        }
    }
    private IEnumerator EnableCanvasAfterDelay()
    {
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, true);
        // 1.7秒待機
        yield return new WaitForSeconds(1f);
        Diacup2get.gameObject.SetActive(true);

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