using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class Watercanset : MonoBehaviour
{
    [SerializeField] private MonoBehaviour playerScript;
    [SerializeField] private GameObject targetCamera;
    [SerializeField] private Canvas Bkeyget;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect; // 音声クリップをシリアライズ
    [SerializeField] private GameObject Bkey; // 最初に非表示のオブジェクト

    private bool soundPlaying = false;

    private FlagManager flagManager;
    public PlayableDirector director;

    private bool animationCompleted = false;

    void Start()
    {
        flagManager = FlagManager.Instance;
        audioSource = GetComponent<AudioSource>();

        if (director != null)
        {
            director.stopped += OnPlayableDirectorStopped;
        }

        Bkeyget.gameObject.SetActive(false);
    }

    void Update()
    {
        if (flagManager.GetFlagByType(Item.Type.watercan) &&
            !flagManager.GetFlag(FlagManager.FlagType.potopen))
        {
            Openpot();
        }

        if (animationCompleted)
        {
            if (targetCamera != null)
            {
                targetCamera.SetActive(false);
            }
            animationCompleted = false;

            // 0.7秒の遅延を追加
            StartCoroutine(ShowItemGetPanelWithDelay(1f));
        }

        if (Bkeyget.gameObject.activeSelf &&
            Input.GetButtonDown("Fire2") &&
            flagManager.GetFlag(FlagManager.FlagType.Itemgetpanel))
        {
            Bkeyget.gameObject.SetActive(false);
            flagManager.SetFlag(FlagManager.FlagType.Itemgetpanel, false);
            if (playerScript != null) playerScript.enabled = true;
        }
    }

    private void Openpot()
    {
        if (!flagManager.GetFlag(FlagManager.FlagType.potopen) &&
            !flagManager.GetFlag(FlagManager.FlagType.Nowanim))
        {
            flagManager.SetFlag(FlagManager.FlagType.Nowanim, true);
            director.Play();
            DisablePlayerControls();
        }
    }

    private void DisablePlayerControls()
    {
        if (playerScript != null)
        {
            playerScript.enabled = false;
        }
    }

    private void EnablePlayerControls()
    {
        if (playerScript != null)
        {
            playerScript.enabled = true;
        }
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        animationCompleted = true;
        flagManager.SetFlag(FlagManager.FlagType.potopen, true);
        flagManager.SetFlag(FlagManager.FlagType.Nowanim, false);
        EnablePlayerControls();
    }

    private void OnDestroy()
    {
        if (director != null)
        {
            director.stopped -= OnPlayableDirectorStopped;
        }
    }

    private IEnumerator ShowItemGetPanelWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Bkey.SetActive(true);
        Itemgetpanel();
    }

    private void Itemgetpanel()
    {
        if (!soundPlaying)
        {
            Bkeyget.gameObject.SetActive(true);
            flagManager.SetFlag(FlagManager.FlagType.Itemgetpanel, true);

            audioSource.PlayOneShot(soundEffect); // soundEffect（音声データ）を再生
        }
    }
}
