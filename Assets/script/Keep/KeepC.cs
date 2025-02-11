using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepC : MonoBehaviour
{
    [Header("Belt1move")]
    [SerializeField] private GameObject[] DeleteObjects; // 対象のGameObjectをInspectorで設定
    [SerializeField] private GameObject[] GenerateObjects; // 対象のGameObjectをInspectorで設定
    [SerializeField] private Collider[] GenerateColliders; // 有効化するコライダー（ここを追加）

    [Header("Belt1move Y Position Change")]
    [SerializeField] private GameObject targetObject; // Y座標を変更する対象
    [SerializeField] private float newYPosition; // 新しいY座標の値

    [Header("leverdown")]
    [SerializeField] private GameObject[] DeleteObjects1; // 対象のGameObjectをInspectorで設定
    [SerializeField] private GameObject[] GenerateObjects1; // 対象のGameObjectをInspectorで設定

    [Header("toolboxopen")]
    [SerializeField] private GameObject[] DeleteObjects2; // 複数形にリネーム
    [SerializeField] private GameObject[] GenerateObjects2; // 複数形にリネーム

    [Header("ironopen")]
    [SerializeField] private GameObject[] DeleteObjects3; // 複数形にリネーム
    [SerializeField] private GameObject[] GenerateObjects3; // 複数形にリネーム

    [Header("Belt2move")]
    [SerializeField] private GameObject[] DeleteObjects4; // 複数形にリネーム
    [SerializeField] private GameObject[] GenerateObjects4; // 複数形にリネーム

    //[Header("mix")]
    //[SerializeField] private GameObject[] DeleteObjects5; // 複数形にリネーム
    //[SerializeField] private GameObject[] GenerateObjects5; // 複数形にリネーム

    //[Header("watercan")]
    //[SerializeField] private GameObject[] DeleteObjects6; // 複数形にリネーム
    //[SerializeField] private GameObject[] GenerateObjects6; // 複数形にリネーム

    void Start()
    {
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Belt1move))
        {
            SetActiveForObjects(GenerateObjects, true);
            SetActiveForObjects(DeleteObjects, false);
            EnableColliders(GenerateColliders, true);  // ここでコライダーを有効化
            ChangeYPosition(targetObject, newYPosition); // Y座標変更処理
        }

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Leverdown))
        {
            SetActiveForObjects(GenerateObjects1, true);
            SetActiveForObjects(DeleteObjects1, false);
        }

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.toolboxopen))
        {
            SetActiveForObjects(GenerateObjects2, true);
            SetActiveForObjects(DeleteObjects2, false);
        }

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.playironplate))
        {
            SetActiveForObjects(GenerateObjects3, true);
            SetActiveForObjects(DeleteObjects3, false);
        }

        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Belt2move))
        {
            SetActiveForObjects(GenerateObjects4, true);
            SetActiveForObjects(DeleteObjects4, false);
        }

        //if (FlagManager.Instance.GetFlagByType(Item.Type.watercan))
        //{
        //    SetActiveForObjects(GenerateObjects6, true);
        //    SetActiveForObjects(DeleteObjects6, false);
        //}
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

    private void EnableColliders(Collider[] colliders, bool state)
    {
        foreach (Collider col in colliders)
        {
            col.enabled = state;
        }
    }

    private void ChangeYPosition(GameObject obj, float newY)
    {
        if (obj != null)
        {
            Vector3 position = obj.transform.position;
            position.y = newY;
            obj.transform.position = position;
        }
    }
}