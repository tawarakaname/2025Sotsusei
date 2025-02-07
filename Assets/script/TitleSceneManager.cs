using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() // OnDestroyよりOnDisableのほうが確実
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "title")
        {
            ResetProgress();
        }
    }

    private void ResetProgress()
    {
        if (FlagManager.Instance != null)
        {
            FlagManager.Instance.ResetFlags();
        }
        // アイテムリセット
        Itembox.instance?.ResetItembox();

        PickupObj.ResetPickedItems();
        Debug.Log("進行状況をリセットしました。");


    }
}
