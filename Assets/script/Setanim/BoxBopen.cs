using System.Collections;
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
    private bool itemgetpanelLogged = false;
    [SerializeField] private float animatedTime;
    private Coroutine boxBOpenCoroutine;

    [SerializeField] private GameObject itemgeteffect;

    void Start()
    {
        balloonget.gameObject.SetActive(false);
        // ueとshitaのAnimatorコンポーネントを取得
        if (Bue != null) BueAnimator = Bue.GetComponent<Animator>();
        if (Bshita != null) BshitaAnimator = Bshita.GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
        itemgeteffect.gameObject.SetActive(false);

    }


    void Update()
    {
        // プレイヤーがコライダー内にいる場合のみ処理を行う
        if (playerInsideCollider)
        {
            // ColorPasswordclear が true かつ Aboxopen がまだ開かれていない場合
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.ColorPasswordclear) &&
                 boxBOpenCoroutine == null)
            {
                // プレイヤー操作と Fire1, Fire2 ボタンの入力を無効化
                DisablePlayerControls();
                OnClickBbox();
                boxBOpenCoroutine = StartCoroutine(BoxBAnimCompleted());
            }

            //  ColorPasswordclear が true で、まだ canvas が有効化されていない場合のみ処理を行う
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.ColorPasswordclear) && !canvasEnabled)
            {
                StartCoroutine(EnableCanvasAfterDelay());
                canvasEnabled = true;  // コルーチンが一度だけ呼ばれるように設定
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

    private IEnumerator BoxBAnimCompleted()
    {
        yield return new WaitForSeconds(animatedTime);
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Bboxopen, true);
    }

    public void OnClickBbox()
    {
        // Bboxopen がまだ開かれていない場合のみアニメーションを再生
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Bboxopen))
        {
            Colorpasswordobj.SetActive(false);
            // 上と下のアニメーションをできるだけ同時に再生する
            StartCoroutine(PlayAnimationsSimultaneously());
            // Fire1, Fire2 入力を無効にするフラグを立てる
            controlsDisabled = true;
        }
    }

    private IEnumerator PlayAnimationsSimultaneously()
    {
        if (BueAnimator != null) BueAnimator.SetTrigger("Bboxopen");
        if (BshitaAnimator != null) BshitaAnimator.SetTrigger("Bboxopen");

        // 次のフレームまで待機して処理を続行
        yield return null;
        yield return null;
    }

    private IEnumerator EnableCanvasAfterDelay()
    {
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, true);
        // 1.7秒待機
        yield return new WaitForSeconds(1.2f);
        itemgeteffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        balloonget.gameObject.SetActive(true);

        if (!itemgetpanelLogged)
        {
            itemgetpanelLogged = true;
        }
        audioSource.PlayOneShot(soundEffect); // 音声クリップを再生
        Debug.Log("正解のSEを流します");

    }


    private void DisablePlayerControls()
    {
        if (playerScript != null) playerScript.enabled = false;
        controlsDisabled = true;
    }
}
