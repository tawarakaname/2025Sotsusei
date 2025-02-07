using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepZ : MonoBehaviour
{
    [SerializeField] private GameObject[] DeleteObjects; // 複数形にリネーム
    [SerializeField] private GameObject[] GenerateObjects; // 複数形にリネーム

    void Start()
    {
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Z, true);

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Result_A))
        {
            SetActiveForObjects(DeleteObjects, false);
            SetActiveForObjects(GenerateObjects, true);
        }
    }

    private void SetActiveForObjects(GameObject[] objects, bool state)
    {
        if (objects == null) return;

        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                obj.SetActive(state);
            }
        }
    }
}
