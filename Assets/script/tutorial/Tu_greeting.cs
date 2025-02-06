using UnityEngine;
using UnityEngine.Playables;

public class Tu_greeting : MonoBehaviour
{
    [SerializeField] private MonoBehaviour playerScript;
    [SerializeField] private GameObject targetCamera;
    private FlagManager flagManager;
    public PlayableDirector director;



    // Start is called before the first frame update
    void Start()
    {
        flagManager = FlagManager.Instance;

        if (flagManager.GetFlag(FlagManager.FlagType.Greeting))
        {
            DisableScript();
            return;
        }

        if (director != null)
        {
            director.stopped += OnPlayableDirectorStopped;
        }

        if (!flagManager.GetFlag(FlagManager.FlagType.Greeting))
        {
            Greeting();
        }

        if (flagManager.GetFlag(FlagManager.FlagType.Be_Aclear))
        {
            DisableScript();
            return;
        }
    }


    private void Greeting()
    {
        if (!flagManager.GetFlag(FlagManager.FlagType.Greeting) &&
            !flagManager.GetFlag(FlagManager.FlagType.Nowanim))
        {
            flagManager.SetFlag(FlagManager.FlagType.Nowanim, true);
            Debug.Log("greeting");
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

    private void OnDestroy()
    {
        if (director != null)
        {
            director.stopped -= OnPlayableDirectorStopped;
        }
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        flagManager.SetFlag(FlagManager.FlagType.Nowanim, false);
        EnablePlayerControls();

        if (targetCamera != null)
        {
            targetCamera.SetActive(false);
        }


        flagManager.SetFlag(FlagManager.FlagType.Greeting, true);
    }

    private void DisableScript()
    {
        // イベントの登録解除
        if (director != null)
        {
            director.stopped -= OnPlayableDirectorStopped;

            // タイムラインを強制停止
            director.Stop();
        }

        // スクリプトを無効化
        this.enabled = false;
    }
}
