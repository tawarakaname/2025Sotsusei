using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepA : MonoBehaviour
{
    [Header("Result_A")]
    [SerializeField] private GameObject[] DeleteObjects; // 複数形にリネーム
    [SerializeField] private GameObject[] GenerateObjects; // 複数形にリネーム

    [Header("青い箱")]
    [SerializeField] private GameObject[] DeleteObjects1; // 対象のGameObjectをInspectorで設定
    [SerializeField] private GameObject[] GenerateObjects1; // 対象のGameObjectをInspectorで設定

    void Start()
    {
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Result_A))
        {
            SetActiveForObjects(DeleteObjects, false);
            SetActiveForObjects(GenerateObjects, true);
        }

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.playBlueBox))
        {
            SetActiveForObjects(GenerateObjects1, true);
            SetActiveForObjects(DeleteObjects1, false);
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
