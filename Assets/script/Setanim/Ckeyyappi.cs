using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class Ckeyyappi : MonoBehaviour
{
    [SerializeField] private MonoBehaviour playerScript;
    [SerializeField] private GameObject targetCamera;
    [SerializeField] private Canvas Ckeyfullget;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect; // 音声クリップをシリアライズ
    [SerializeField] private GameObject Ckeyfull; // 最初に非表示のオブジェクト
    [SerializeField] private GameObject dammyyappi; // 最初に非表示のオブジェクト
    //[SerializeField] private GameObject itemgeteffect;

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

        Ckeyfullget.gameObject.SetActive(false);
        //itemgeteffect.gameObject.SetActive(false);
    }

    void Update()
    {

        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.yappiclear))
        {
            return;
        }

        if (flagManager.GetFlagByType(Item.Type.Dkey1) &&
           (flagManager.GetFlagByType(Item.Type.Dkey2) &&
            !flagManager.GetFlag(FlagManager.FlagType.getckeyfull)))
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
            StartCoroutine(ShowItemGetPanelWithDelay(0.1f));
        }

        if (Ckeyfullget.gameObject.activeSelf &&
            Input.GetButtonDown("Fire2") &&
            flagManager.GetFlag(FlagManager.FlagType.Itemgetpanel))
        {
            Ckeyfull.gameObject.SetActive(true);
            Ckeyfullget.gameObject.SetActive(false);
            flagManager.SetFlag(FlagManager.FlagType.Itemgetpanel, false);
            //itemgeteffect.gameObject.SetActive(false);
            if (playerScript != null) playerScript.enabled = true;
            // フラグ Ckeygivefase を true に設定
            flagManager.SetFlag(FlagManager.FlagType.Ckeygivefase, false);
            dammyyappi.gameObject.SetActive(false);
        }
    }

    private void Openpot()
    {
        if (!flagManager.GetFlag(FlagManager.FlagType.getckeyfull) &&
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
        flagManager.SetFlag(FlagManager.FlagType.getckeyfull, true);
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
        //yield return new WaitForSeconds(0.2f);
        //itemgeteffect.gameObject.SetActive(true);
        // yield return new WaitForSeconds(0.5f);
        Ckeyfull.SetActive(true);
        Itemgetpanel();
    }

    private void Itemgetpanel()
    {
        if (!soundPlaying)
        {
            Ckeyfullget.gameObject.SetActive(true);
            flagManager.SetFlag(FlagManager.FlagType.Itemgetpanel, true);

            audioSource.PlayOneShot(soundEffect); // soundEffect（音声データ）を再生
        }
    }
}
