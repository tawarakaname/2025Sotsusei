using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GasBannerAOpen : MonoBehaviour
{
    [SerializeField] private List<GameObject> gasBannerDoors;
    [SerializeField] private MonoBehaviour playerScript;
    [SerializeField] private List<Collider> gasCameraColliders;
    [SerializeField] private GameObject targetCamera;

    private bool controlsDisabled = false;
    private FlagManager flagManager;
    public PlayableDirector director;

    private bool animationCompleted = false;
    private bool playerControlDisabledOnce = false;  // プレイヤー操作無効化の一度限り実行フラグ

    void Start()
    {
        flagManager = FlagManager.Instance;

        foreach (var collider in gasCameraColliders)
        {
            if (collider != null)
            {
                collider.enabled = false;
            }
        }

        if (director != null)
        {
            director.stopped += OnPlayableDirectorStopped;
        }
    }

    void Update()
    {
        if (flagManager.GetFlag(FlagManager.FlagType.DialPasswordclear) &&
            !flagManager.GetFlag(FlagManager.FlagType.Gasbaneropen) &&
            !playerControlDisabledOnce) // 一度だけ無効化を実行
        {
            DisablePlayerControls();
            OpenGasBanner();
            playerControlDisabledOnce = true; // 一度実行後フラグを立てる
        }

        if (controlsDisabled && (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")))
        {
            return;
        }

        if (animationCompleted)
        {
            if (targetCamera != null)
            {
                targetCamera.SetActive(false);
            }
            animationCompleted = false;
        }
    }

    private void OpenGasBanner()
    {
        if (!flagManager.GetFlag(FlagManager.FlagType.Gasbaneropen) &&
            !flagManager.GetFlag(FlagManager.FlagType.Nowanim))
        {
            director.Play();

            flagManager.SetFlag(FlagManager.FlagType.Nowanim, true);

            foreach (var collider in gasCameraColliders)
            {
                if (collider != null)
                {
                    collider.enabled = true;
                }
            }
        }
    }

    private void DisablePlayerControls()
    {
        if (playerScript != null)
        {
            playerScript.enabled = false;
            controlsDisabled = true;
            Debug.Log("プレイヤー操作無効化");
        }
    }

    private void EnablePlayerControls()
    {
        if (playerScript != null)
        {
            playerScript.enabled = true;
            controlsDisabled = false;
            Debug.Log("プレイヤー操作有効化");
        }
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        animationCompleted = true;
        flagManager.SetFlag(FlagManager.FlagType.Gasbaneropen, true);

        flagManager.SetFlag(FlagManager.FlagType.Nowanim, false);

        EnablePlayerControls();  // アニメーション終了後に一度だけプレイヤー操作を有効化
    }

    private void OnDestroy()
    {
        if (director != null)
        {
            director.stopped -= OnPlayableDirectorStopped;
        }
    }
}
