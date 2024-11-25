using System.Collections;
using UnityEngine;

public class Smellopen : MonoBehaviour
{
    [SerializeField] private GameObject Blueori;
    [SerializeField] private Canvas bluekeyGet;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect; // 効果音
    [SerializeField] private MonoBehaviour playerScript;
    [SerializeField] private float animatedTime;
    [SerializeField] private Collider OdogurCollider;  // トリガーコライダー追加

    private Animator blueoriAnimator;
    private bool controlsDisabled = false;
    private bool canvasEnabled = false;
    private bool itemGetPanelLogged = false;
    private bool playerInsideCollider = false;  // プレイヤーがコライダー内にいるかどうかのフラグ
    private Coroutine blueoriOpenCoroutine;

    void Start()
    {
        // 初期設定
        audioSource = GetComponent<AudioSource>();
        bluekeyGet.gameObject.SetActive(false);

        if (Blueori != null)
            blueoriAnimator = Blueori.GetComponent<Animator>();
    }

    void Update()
    {
        // プレイヤーがコライダー内にいる場合のみ動作
        if (!playerInsideCollider)
            return; // コライダー内にいない場合、コードは何もしない

        // FlagType.SmellPasswordclearが立ったら
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.SmellPasswordclear) &&
           !FlagManager.Instance.GetFlag(FlagManager.FlagType.Blueoriopen) &&
           FlagManager.Instance.GetFlag(FlagManager.FlagType.OdoguCamera) &&
             blueoriOpenCoroutine == null)
        {
            DisablePlayerControls();
            OnClickBlueori();
            blueoriOpenCoroutine = StartCoroutine(BlueoriAnimCompleted());
        }

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.SmellPasswordclear) && !canvasEnabled)
        {
            StartCoroutine(EnableCanvasAfterDelay());
            canvasEnabled = true;  // コルーチンが一度だけ呼ばれるように設定
        }

        // Fire2ボタン入力でパネルとフラグのリセット
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.SmellPasswordclear) &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.Blueoriopen) &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.OdoguCamera) &&
            Input.GetButtonDown("Fire2"))

        {
            bluekeyGet.gameObject.SetActive(false);
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, false);
            playerScript.enabled = true;

            // コードの無効化とコライダー削除
            DisableAndCleanup();
            Debug.Log("owari");
        }

        // Fire1ボタンの無効化
        if (controlsDisabled && Input.GetButtonDown("Fire1"))
        {
            return;
        }
    }

    private IEnumerator BlueoriAnimCompleted()
    {
        yield return new WaitForSeconds(animatedTime);
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Blueoriopen, true);
    }

    public void OnClickBlueori()
    {
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Blueoriopen))
        {
            StartCoroutine(PlayAnimationsSimultaneously());
            controlsDisabled = true;
        }
    }

    private IEnumerator PlayAnimationsSimultaneously()
    {
        if (blueoriAnimator != null)
            blueoriAnimator.SetTrigger("Blueoriopen");

        yield return null;
    }

    private IEnumerator EnableCanvasAfterDelay()
    {
        // すでにアイテム取得パネルが表示されている場合は何もしない
        if (itemGetPanelLogged)
        {
            yield break; // 既に表示済みの場合、コルーチンを終了
        }
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Blueoriopen))
        {
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, true);
            yield return new WaitForSeconds(1f);

            bluekeyGet.gameObject.SetActive(true);

            if (!itemGetPanelLogged)
            {
                itemGetPanelLogged = true;
                Debug.Log("正解のSEを流します");
                audioSource.PlayOneShot(soundEffect); // 効果音を再生
            }
        }
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Blueoriopen, true);
       
    }

    private void DisablePlayerControls()
    {
        if (playerScript != null)
            playerScript.enabled = false;

        controlsDisabled = true;
    }

    // プレイヤーがOdogurColliderに入ったとき
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = true;
            Debug.Log("in");
        }
    }

    // プレイヤーがOdogurColliderから出たとき
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = false;
            Debug.Log("out");
        }
    }

    // スクリプトを無効化し、コライダーを削除
    private void DisableAndCleanup()
    {
        // コライダーを削除
        if (OdogurCollider != null)
        {
            Destroy(OdogurCollider);
        }

        // スクリプトを無効化
        this.enabled = false;

        Debug.Log("Smellopen script and collider disabled.");
    }
}
