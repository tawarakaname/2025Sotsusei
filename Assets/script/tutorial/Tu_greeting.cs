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

        if (director != null)
        {
            director.stopped += OnPlayableDirectorStopped;
        }

        if (!flagManager.GetFlag(FlagManager.FlagType.Greeting))
        {
            Greeting();
        }
    }


    private void Greeting()
    {
        if (!flagManager.GetFlag(FlagManager.FlagType.Greeting) &&
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

    private void OnDestroy()
    {
        if (director != null)
        {
            director.stopped -= OnPlayableDirectorStopped;
        }
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        flagManager.SetFlag(FlagManager.FlagType.Greeting, true);
        flagManager.SetFlag(FlagManager.FlagType.Nowanim, false);
        EnablePlayerControls();

        if (targetCamera != null)
        {
            targetCamera.SetActive(false);
        }
    }

}
