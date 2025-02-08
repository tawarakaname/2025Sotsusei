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

        // Canvas や OptionCanvas をリセット
        var itemCanvas = FindObjectOfType<ItemCanvas>();
        if (itemCanvas != null)
        {
            itemCanvas.ResetItemCanvas(); // Canvas と OptionCanvas を非表示にする
        }

        // ZukanScript にアクセスせず、フラグをリセットするだけ
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Zukan, false);


        Debug.Log("進行状況をリセットしました。");
    }

}
