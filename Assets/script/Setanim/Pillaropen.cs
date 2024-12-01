using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class Pillaropen : MonoBehaviour
{
    [SerializeField] private Collider pillarcollider;
    [SerializeField] GameObject Monitorpasswordobj;
    [SerializeField] GameObject Mixpasswordobj;
    [SerializeField] Canvas jouroget;
    private bool playerInsideCollider = false;

    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect; // 音声クリップをシリアライズ

    [SerializeField] private MonoBehaviour playerScript;
    private bool controlsDisabled = false;
    private bool canvasEnabled = false;
    private bool itemgetpanelLogged = false;

    [SerializeField] private GameObject targetCamera;

    // PlayableDirectorの追加
    [SerializeField] private PlayableDirector director;

    // 独自のフラグを追加
    private bool playerDisabledOnce = false;
    private bool playerEnabledOnce = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        jouroget.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerInsideCollider)
        {
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.MonitorPasswordclear))
            {
                Monitorpasswordobj.SetActive(false);
                Mixpasswordobj.SetActive(true);
            }
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.MixPasswordclear) && !playerDisabledOnce)
            {
                DisablePlayerControls();
                OnClickpillar();
                playerDisabledOnce = true; // フラグを設定
            }
            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.MixPasswordclear) && !canvasEnabled)
            {
                StartCoroutine(EnableCanvasAfterDelay());
                canvasEnabled = true; // コルーチンが一度だけ呼ばれるように設定
            }

            if (FlagManager.Instance.GetFlag(FlagManager.FlagType.MixPasswordclear) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.Pillaropen) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.CameraZoomObj) &&
                FlagManager.Instance.GetFlag(FlagManager.FlagType.JugCamera) &&
                playerInsideCollider &&
                Input.GetButtonDown("Fire2"))
            {
                jouroget.gameObject.SetActive(false);
                FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, false);
                targetCamera.SetActive(false);
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

    public void OnClickpillar()
    {
        // 既にフラグが設定されている場合、処理をスキップ
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Pillaropen))
        {
            StartCoroutine(PlayAnimationsSimultaneously());
            controlsDisabled = true;
        }
    }

    private IEnumerator PlayAnimationsSimultaneously()
    {
        if (director != null)
        {
            director.Play(); // PlayableDirectorで再生
        }

        yield return new WaitForSeconds((float)director.duration); // PlayableDirectorの再生時間を待機

        // フラグを設定（ここで一度だけ実行される）
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Pillaropen))
        {
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Pillaropen, true);
        }

        // プレイヤー操作を再び有効化
        if (playerScript != null && !playerEnabledOnce)
        {
            playerScript.enabled = true;
            playerEnabledOnce = true; // フラグを設定
        }
    }

    private IEnumerator EnableCanvasAfterDelay()
    {
        yield return new WaitForSeconds(4.5f);
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Itemgetpanel, true);
        jouroget.gameObject.SetActive(true);

        if (!itemgetpanelLogged)
        {
            itemgetpanelLogged = true;
        }
        audioSource.PlayOneShot(soundEffect); // 音声クリップを再生
        Debug.Log("正解のSEを流します");
    }

    private void DisablePlayerControls()
    {
        if (playerScript != null)
        {
            playerScript.enabled = false;
        }
        controlsDisabled = true;
    }
}
