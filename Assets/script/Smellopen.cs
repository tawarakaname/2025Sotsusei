using System.Collections;
using UnityEngine;

public class Smellopen : MonoBehaviour
{
    [SerializeField] private GameObject Blueori;
    [SerializeField] private Collider SmellgimickCollider;
    [SerializeField] private Canvas bluekeyGet;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect; // 効果音
    [SerializeField] private MonoBehaviour playerScript;
    [SerializeField] private float animatedTime;

    private Animator blueoriAnimator;
    private bool controlsDisabled = false;
    private bool canvasEnabled = false;
    private bool itemGetPanelLogged = false;
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
        // FlagType.SmellPasswordclearが立ったら
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.SmellPasswordclear) && blueoriOpenCoroutine == null)
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
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, true);
        yield return new WaitForSeconds(1f);

        bluekeyGet.gameObject.SetActive(true);

        if (!itemGetPanelLogged)
        {
            itemGetPanelLogged = true;
            Debug.Log("正解のSEを流します");
        }

        audioSource.PlayOneShot(soundEffect); // 効果音を再生
    }

    private void DisablePlayerControls()
    {
        if (playerScript != null)
            playerScript.enabled = false;

        controlsDisabled = true;
    }
}