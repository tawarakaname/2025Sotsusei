using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safezone : MonoBehaviour
{
    [SerializeField] private Collider safezoneCollider; // コライダーAを指定する
    [SerializeField] private Transform safezoneCenter;  // ゾーン内の中心位置（移動先の方向を決定）

    private TextManager textManager; // TextManagerへの参照
    private string currentKeyword; // 現在のコライダーに対応するキーワード

    private void Start()
    {
        textManager = GameObject.FindWithTag("TextManager").GetComponent<TextManager>();
    }

    private void OnTriggerExit(Collider other)
    {
        // "Player"タグを持つオブジェクトのみ処理する
        if (other.CompareTag("Player"))
        {
            // プレイヤーがSafezone外に出ようとした場合、中心に向かって5ユニット移動させる
            MovePlayerTowardsCenter(other.transform);

            currentKeyword = "Notsafezone";

            if (currentKeyword != null && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
            {
                OnClickSafezoneThis();
            }
        }
    }

    private void Update()
    {
        // Belt1moveフラグがtrueの場合、このスクリプトを無効化
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Belt1move))
        {
            this.enabled = false;
            return;
        }

        // Fire2入力がある場合にフラグとキーワードをチェック
        if (Input.GetButtonDown("Fire2") && currentKeyword != null && FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
        {
            textManager.DisplayCurrentLine();
        }
    }

    public void OnClickSafezoneThis()
    {
        textManager.DisplayTextForKeyword(currentKeyword);
    }

    // プレイヤーをSafezone中心に向かって5ユニット移動させる関数
    private void MovePlayerTowardsCenter(Transform player)
    {
        Vector3 direction = safezoneCenter.position - player.position;  // 中心に向かう方向ベクトル
        direction.y = 0;  // 垂直方向の移動を無効化（y軸方向の移動が不要であれば）

        if (direction.magnitude > 0) // 中心に向かう方向が存在する場合
        {
            direction.Normalize();  // 単位ベクトルにする
            player.position += direction * 0.5f;  // 中心に向かって5ユニット移動
        }
    }
}
