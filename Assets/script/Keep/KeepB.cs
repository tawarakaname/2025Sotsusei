using UnityEngine;

public class KeepB : MonoBehaviour
{
    [Header("SmellPassword")]
    [SerializeField] private GameObject[] DeleteObjects; // 対象のGameObjectをInspectorで設定
    [SerializeField] private GameObject[] GenerateObjects; // 対象のGameObjectをInspectorで設定

    [Header("BTB")]
    [SerializeField] private GameObject[] DeleteObjects1; // 対象のGameObjectをInspectorで設定
    [SerializeField] private GameObject[] GenerateObjects1; // 対象のGameObjectをInspectorで設定

    [Header("ThreePassword")]
    [SerializeField] private GameObject[] DeleteObjects2; // 複数形にリネーム
    [SerializeField] private GameObject[] GenerateObjects2; // 複数形にリネーム

    [Header("redbutton")]
    [SerializeField] private GameObject[] DeleteObjects3; // 複数形にリネーム
    [SerializeField] private GameObject[] GenerateObjects3; // 複数形にリネーム

    [Header("monitor")]
    [SerializeField] private GameObject[] DeleteObjects4; // 複数形にリネーム
    [SerializeField] private GameObject[] GenerateObjects4; // 複数形にリネーム

    [Header("mix")]
    [SerializeField] private GameObject[] DeleteObjects5; // 複数形にリネーム
    [SerializeField] private GameObject[] GenerateObjects5; // 複数形にリネーム

    [Header("watercan")]
    [SerializeField] private GameObject[] DeleteObjects6; // 複数形にリネーム
    [SerializeField] private GameObject[] GenerateObjects6; // 複数形にリネーム

    void Start()
    {
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.SmellPasswordclear))
        {
            SetActiveForObjects(GenerateObjects, true);
            SetActiveForObjects(DeleteObjects, false);
        }

        if (FlagManager.Instance.GetFlagByType(Item.Type.BTB))
        {
            SetActiveForObjects(GenerateObjects1, true);
            SetActiveForObjects(DeleteObjects1, false);
        }

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.PushRedButton))
        {
            SetActiveForObjects(GenerateObjects3, true);
            SetActiveForObjects(DeleteObjects3, false);
        }

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.MonitorPasswordclear))
        {
            SetActiveForObjects(GenerateObjects4, true);
            SetActiveForObjects(DeleteObjects4, false);
        }

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.MixPasswordclear))
        {
            SetActiveForObjects(GenerateObjects5, true);
            SetActiveForObjects(DeleteObjects5, false);
        }

        if (FlagManager.Instance.GetFlagByType(Item.Type.watercan))
        {
            SetActiveForObjects(GenerateObjects6, true);
            SetActiveForObjects(DeleteObjects6, false);
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