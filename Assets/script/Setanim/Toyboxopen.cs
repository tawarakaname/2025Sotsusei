using System.Collections;
using UnityEngine;

public class Toyboxopen : MonoBehaviour
{
    public GameObject Toyanim;  // 上のオブジェクト (アニメーションがついているオブジェクト)
    private Animator ToyueAnimator;  // 上のオブジェクトのアニメーター
    [SerializeField] private Collider ToyBoxcollider;
    [SerializeField] Canvas butteryAget;
    private bool playerInsideCollider = false;
    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect; // 音声クリップをシリアライズ

    [SerializeField] private GameObject BatteryA; // 最初に非表示のオブジェクト

    // Playerクラスの参照（プレイヤーの操作を無効化するため）
    [SerializeField] private MonoBehaviour playerScript;  // Playerクラスをアタッチ
    private bool controlsDisabled = false;  // 操作無効フラグ
    private bool canvasEnabled = false;  // canvasDgetが有効になったかを追跡するフラグ
    private bool itemgetpanelLogged = false;
    [SerializeField] private float animatedTime;
    [SerializeField] private GameObject itemgeteffect;
    private Coroutine ToyOpenCoroutine;

    void Start()
    {
        butteryAget.gameObject.SetActive(false);
        // ueとshitaのAnimatorコンポーネントを取得
        if (Toyanim != null) ToyueAnimator = Toyanim.GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
        itemgeteffect.gameObject.SetActive(false);

    }


    void Update()
    {
        // プレイヤーがコライダー内にいる場合のみ処理を行う
        if (playerInsideCollider)
        {
            if (playerInsideCollider &&
   　　　　　　   FlagManager.Instance.GetFlag(FlagManager.FlagType.ToyPasswordclear) &&
   　　　　　　   ToyOpenCoroutine == null)
           　　 {
             　　   DisablePlayerControls();
             　　   OnClickToybox();
             　　   ToyOpenCoroutine = StartCoroutine(ToyAnimCompleted());
          　　  }


            //  ColorPasswordclear が true で、まだ canvas が有効化されていない場合のみ処理を行う
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.ToyPasswordclear) && !canvasEnabled)
            {
                BatteryA.SetActive(true);
                StartCoroutine(EnableCanvasAfterDelay());
                canvasEnabled = true;  // コルーチンが一度だけ呼ばれるように設定
            }

            // 条件をチェック
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.ToyPasswordclear) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.Toyboxopen) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.ToyboxCamera) &&
                playerInsideCollider &&
                Input.GetButtonDown("Fire2"))
            {
                // Fire2が押されたらCanvasをfalseに
                butteryAget.gameObject.SetActive(false);
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

    private IEnumerator ToyAnimCompleted()
    {
        yield return new WaitForSeconds(animatedTime);
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Toyboxopen, true);
    }

    public void OnClickToybox()
    {
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Toyboxopen))
        {
            StartCoroutine(PlayAnimationsSimultaneously());
            controlsDisabled = true;
        }

    }

    private IEnumerator PlayAnimationsSimultaneously()
    {
        if (ToyueAnimator != null) ToyueAnimator.SetTrigger("toyboxopen");
        yield return null;
      
    }

    private IEnumerator EnableCanvasAfterDelay()
    {
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, true);
        // 1.7秒待機
        yield return new WaitForSeconds(1f);
        itemgeteffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        butteryAget.gameObject.SetActive(true);

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
