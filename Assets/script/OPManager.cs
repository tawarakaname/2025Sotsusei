
using UnityEngine.SceneManagement;
using UnityEngine;

public class OPManager : MonoBehaviour
{
    private void OnEnableOP()
    {
        SceneManager.sceneLoaded += OnSceneLoadedOP;
    }

    private void OnDisableOP() // OnDestroyよりOnDisableのほうが確実
    {
        SceneManager.sceneLoaded -= OnSceneLoadedOP;
    }

    private void OnSceneLoadedOP(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "OP")
        {
            ResetProgressOP();
        }
    }

    private void ResetProgressOP()
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
