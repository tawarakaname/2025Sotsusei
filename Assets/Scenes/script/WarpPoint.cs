using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPoint : MonoBehaviour
{

    public Vector3 teleportDestination;
    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーがトリガーゾーンに入ったかどうかを確認
        if (other.CompareTag("Player"))
        {
            // プレイヤーの位置をワープ先に設定
            other.transform.position = teleportDestination;
        }
    }
}
