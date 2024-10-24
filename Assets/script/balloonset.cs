using System.Collections;
using UnityEngine;

public class BalloonSet : MonoBehaviour
{
    public Renderer quadRenderer; // QuadのRendererコンポーネント
    public Texture[] balloonTextures; // 切り替える3枚のテクスチャ（スプライトをテクスチャに変換しておく）
    public float frameDuration = 0.5f; // 1枚の画像を表示する時間（秒）

    [SerializeField] GameObject TextBox; // TextBoxへの参照を公開
    [SerializeField] private GameObject DTextBox;
    [SerializeField] TextManager textManager; // TextManagerへの参照を公開
    [SerializeField] Collider standCollider;

    private string currentKeyword; // 現在のコライダーに対応するキーワード

    public Transform animationPosition; // アニメーションを表示する場所

    private bool hasPlayed = false;
    private int currentFrame = 0;

    private void OnTriggerEnter(Collider other)
    {
        // balloon フラグが false の場合にのみ処理を実行
        if (!FlagManager.Instance.GetFlagByType(Item.Type.balloon))
        {
            // プレイヤーがコライダーに接触した場合
            if (other.CompareTag("Player"))
            {
                if (standCollider.bounds.Intersects(other.bounds))
                {
                    currentKeyword = "BalloonStand";
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // balloon フラグが false の場合にのみ処理を実行
        if (!FlagManager.Instance.GetFlagByType(Item.Type.balloon))
        {
            // プレイヤーがコライダーから出た場合、キーワードをリセット
            if (other.CompareTag("Player"))
            {
                currentKeyword = null;
                TextBox.SetActive(false); // コライダーを出た時にTextBoxを非表示にする
            }
        }
    }

    private void Update()
    {
        // balloon フラグが true ならテクスチャアニメーションを再生
        if (FlagManager.Instance.GetFlagByType(Item.Type.balloon) && !hasPlayed)
        {
            StartCoroutine(PlayBalloonTextureAnimation());
            hasPlayed = true; // 一度だけ再生するようにフラグを立てる
        }
        // balloon フラグが false の場合のみ、コライダーとFire2に関連した動作を行う
        else if (!FlagManager.Instance.GetFlagByType(Item.Type.balloon))
        {
            // IllustPasswordclear フラグが false の場合のみ Fire2 ボタンと TextBox の処理を行う
            if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.ColorPasswordclear))
            {
                // Fire2ボタンが押され、かつ currentKeyword が null でない場合
                if (Input.GetButtonDown("Fire2") && currentKeyword != null && !FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
                {
                    OnClickstandThis();
                }
                else if (Input.GetButtonDown("Fire2") && currentKeyword != null && FlagManager.Instance.GetFlag(FlagManager.FlagType.Textbox))
                {
                    textManager.DisplayCurrentLine();
                }
                
            }
        }
    }


    public void OnClickstandThis()
    {
        textManager.DisplayTextForKeyword(currentKeyword);
    }


    // テクスチャを順番に切り替えるコルーチン
    IEnumerator PlayBalloonTextureAnimation()
    {
        // テクスチャの表示場所を指定
        if (animationPosition != null)
        {
            quadRenderer.transform.position = animationPosition.position;
        }

        while (currentFrame < balloonTextures.Length)
        {
            // 現在のフレームのテクスチャを設定
            quadRenderer.material.mainTexture = balloonTextures[currentFrame];

            // frameDuration秒待つ
            yield return new WaitForSeconds(frameDuration);

            // 次のフレームに進む
            currentFrame++;
        }

        // テクスチャが正常に再生されたらAkeygetフラグをtrueに設定
        FlagManager.Instance.SetFlag(FlagManager.FlagType.Akeyget, true);
    }
}
