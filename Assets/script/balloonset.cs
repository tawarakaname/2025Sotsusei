using System.Collections;
using UnityEngine;

public class BalloonSet : MonoBehaviour
{
    public Renderer quadRenderer; // QuadのRendererコンポーネント
    public Texture[] balloonTextures; // 切り替える3枚のテクスチャ（スプライトをテクスチャに変換しておく）
    public float frameDuration = 0.5f; // 1枚の画像を表示する時間（秒）

    public Transform animationPosition; // アニメーションを表示する場所

    private bool hasPlayed = false;
    private int currentFrame = 0;

    void Update()
    {
        // balloon フラグが true になったかどうかを確認
        if (FlagManager.Instance.GetFlagByType(Item.Type.balloon) && !hasPlayed)
        {
            StartCoroutine(PlayBalloonTextureAnimation());
            hasPlayed = true; // 一度だけ再生するようにフラグを立てる
        }
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
