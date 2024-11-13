using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ComebackA : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;   // 位置を移動させたいオブジェクト
    [SerializeField] private Vector3 targetPosition;    // 移動先の位置
    [SerializeField] private PlayableDirector director; // 再生させるDirector

    private bool hasPlayedDirector = false; // Directorが再生されたかを追跡

    // Update is called once per frame
    void Update()
    {
        // comebackA フラグが true になったらオブジェクトの位置を移動
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackA))
        {
            MoveTargetObject();
        }

        // Adooropen と comebackA フラグが true かつ、初回再生時のみDirectorを再生
        if (ShouldPlayDirector())
        {
            director.Play();
            hasPlayedDirector = true; // 再生済みと記録
            FlagManager.Instance.SetFlag(FlagManager.FlagType.comebackAanim, true);
            FlagManager.Instance.SetFlag(FlagManager.FlagType.Nowanim, true); // Nowanim フラグを true に設定
        }
    }

    // Director再生条件をチェックするメソッド
    private bool ShouldPlayDirector()
    {
        return FlagManager.Instance.GetFlag(FlagManager.FlagType.Adooropen)
               && FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackA)
               && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Nowanim)
               && !FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackAanim)
               && !hasPlayedDirector;
    }

    // 指定した位置にオブジェクトを移動させるメソッド
    private void MoveTargetObject()
    {
        if (targetObject != null)
        {
            targetObject.transform.position = targetPosition;
        }
    }
}

