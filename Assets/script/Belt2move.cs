using UnityEngine;
using UnityEngine.Playables;

public class Belt2move : MonoBehaviour
{
    [SerializeField] private MonoBehaviour playerScript;
    [SerializeField] private GameObject targetCamera;
    [SerializeField] private GameObject ckey;
    [SerializeField] private GameObject ckeyfalse;
    private FlagManager flagManager;
    public PlayableDirector director;

    void Start()
    {
        flagManager = FlagManager.Instance;
        director.stopped += OnPlayableDirectorStopped;
    }

    void Update()
    {
        if (flagManager.GetFlagByType(Item.Type.batteryC) &&
            flagManager.GetFlagByType(Item.Type.batteryD) &&
            !flagManager.GetFlag(FlagManager.FlagType.Belt2move) &&
            flagManager.GetFlag(FlagManager.FlagType.switch2ON))
        {
            Movebelt2();
        }
    }

    private void Movebelt2()
    {
        if (!flagManager.GetFlag(FlagManager.FlagType.Belt2move) &&
            !flagManager.GetFlag(FlagManager.FlagType.Nowanim))
        {
            director.Play();
            DisablePlayerControls();
            flagManager.SetFlag(FlagManager.FlagType.Nowanim, true);
            Invoke(nameof(EnableCkey), 20f);
        }
    }

    private void EnableCkey()
    {
        ckey.SetActive(true);
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
        flagManager.SetFlag(FlagManager.FlagType.Nowanim, false);
        flagManager.SetFlag(FlagManager.FlagType.Belt2move, true);
        EnablePlayerControls();

        if (targetCamera != null)
        {
            targetCamera.SetActive(false);
        }

        ckeyfalse.SetActive(false);
    }
}
