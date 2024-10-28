using System.Collections;
using UnityEngine;

public class BoxAopen : MonoBehaviour
{
    public GameObject ue;
    public GameObject shita;
    private Animator ueAnimator;
    private Animator shitaAnimator;
    private FlagManager flagManager;
    [SerializeField] private Collider ABoxcollider;
    [SerializeField] GameObject Illustpasswordobj;
    [SerializeField] Canvas capsuleDget;
    private bool playerInsideCollider = false;

    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect; // 音声クリップをシリアライズ

    [SerializeField] private MonoBehaviour playerScript;
    private bool controlsDisabled = false;
    private bool canvasEnabled = false;
    private bool itemgetpanelLogged = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        capsuleDget.gameObject.SetActive(false);
        if (ue != null) ueAnimator = ue.GetComponent<Animator>();
        if (shita != null) shitaAnimator = shita.GetComponent<Animator>();
    }

    void Update()
    {
        if (playerInsideCollider)
        {
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.IllustPasswordclear) &&
                !FlagManager.Instance.GetFlag(FlagManager.FlagType.Aboxopen))
            {
                DisablePlayerControls();
                OnClickAbox();
            }
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.IllustPasswordclear) && !canvasEnabled)
            {
                // すでにコルーチンが実行されていない場合のみ開始
                StartCoroutine(EnableCanvasAfterDelay());
                canvasEnabled = true;  // コルーチンが一度だけ呼ばれるように設定
            }


            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.IllustPasswordclear) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.Aboxopen) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.BoxACamera) &&
                playerInsideCollider &&
                Input.GetButtonDown("Fire2"))
            {
                capsuleDget.gameObject.SetActive(false);
                FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, false);
                playerScript.enabled = true;
            }
        }

        if (controlsDisabled)
        {
            if (Input.GetButtonDown("Fire1"))
            {
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
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Aboxopen))
        {
            Illustpasswordobj.SetActive(false);
            StartCoroutine(PlayAnimationsSimultaneously());
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Aboxopen, true);
            controlsDisabled = true;
        }
    }

    private IEnumerator PlayAnimationsSimultaneously()
    {
        if (ueAnimator != null) ueAnimator.SetTrigger("Aboxopen");
        if (shitaAnimator != null) shitaAnimator.SetTrigger("Aboxopen");
        yield return null;
    }

    private IEnumerator EnableCanvasAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        capsuleDget.gameObject.SetActive(true);
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, true);

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
