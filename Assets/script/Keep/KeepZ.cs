using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepZ : MonoBehaviour
{
    [Header("Result_A")]
    [SerializeField] private GameObject[] DeleteObjects; // 複数形にリネーム
    [SerializeField] private GameObject[] GenerateObjects; // 複数形にリネーム

    [Header("Result_B")]
    [SerializeField] private GameObject[] DeleteObjects1; // 複数形にリネーム
    [SerializeField] private GameObject[] GenerateObjects1; // 複数形にリネーム

    [Header("Result_C")]
    [SerializeField] private GameObject[] DeleteObjects2; // 複数形にリネーム
    [SerializeField] private GameObject[] GenerateObjects2; // 複数形にリネーム

    void Start()
    {
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Z, true);

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Result_A))
        {
            SetActiveForObjects(DeleteObjects, false);
            SetActiveForObjects(GenerateObjects, true);
        }

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Result_B))
        {
            SetActiveForObjects(DeleteObjects1, false);
            SetActiveForObjects(GenerateObjects1, true);
        }

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Result_C))
        {
            SetActiveForObjects(DeleteObjects2, false);
            SetActiveForObjects(GenerateObjects2, true);
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
