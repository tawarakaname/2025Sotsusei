using UnityEngine;
using UnityEngine.UI;

public class ZukanScript : MonoBehaviour
{
    [SerializeField] private Image contentImage; // 表示用のImage
    [Header("未登録")]
    [SerializeField] private Sprite[] sprites; // 未登録時のSpriteのリスト
    [Header("登録")]
    [SerializeField] private Sprite[] clearsprites; // 登録時のSpriteのリスト
    private int currentIndex = 0; // 現在選択されているSpriteのインデックス
    private float nextMoveTime = 0f; // 次の移動可能な時間
    private float moveCooldown = 0.2f; // 入力クールダウン時間
    [SerializeField] private AudioSource audioSource;
    private FlagManager flagManager; // フラグマネージャーのインスタンス

    void Start()
    {
        contentImage.gameObject.SetActive(false);

        flagManager = FlagManager.Instance; // FlagManagerのインスタンスをキャッシュ
        if (flagManager == null)
        {
            return;
        }

        UpdateContentDisplay(); // 初期状態で選択されたSpriteを表示
        audioSource = GetComponent<AudioSource>();
    }

    public void Jump()
    {
        UpdateContentDisplay(); // 現在の選択に基づいて内容を表示
    }

    private void Update()
    {
        // ①optionフラグがtrueでない場合は実行しない
        if (!flagManager.GetFlag(FlagManager.FlagType.Option))
        {
            return;
        }
        // ②Zukanフラグがtrueで、contentImageが非表示の場合に再表示
        if (flagManager.GetFlag(FlagManager.FlagType.Zukan) && !contentImage.gameObject.activeSelf)
        {
            contentImage.gameObject.SetActive(true);
            UpdateContentDisplay(); // 内容を更新
        }
        HandleHorizontalInput(); // 入力を監視

        // ③optionフラグとZukanフラグがどちらもtrueのときFire1を監視
        if (flagManager.GetFlag(FlagManager.FlagType.Zukan) && Input.GetButtonDown("Fire1"))
        {
            HandleFire1Input(); // Fire1の入力処理
        }
    }

    private void HandleHorizontalInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal Stick-L");

        if (Time.time >= nextMoveTime && Mathf.Abs(horizontalInput) > 0.1f)
        {
            if (horizontalInput > 0)
            {
                MoveSelection(1); // 右に移動
                PlayAudio(); // 音声再生
            }
            else if (horizontalInput < 0)
            {
                MoveSelection(-1); // 左に移動
                PlayAudio(); // 音声再生
            }
            nextMoveTime = Time.time + moveCooldown; // 次の移動時間を設定
        }
    }

    private void MoveSelection(int direction)
    {
        currentIndex += direction;

        // インデックスをループさせる
        Sprite[] activeSprites = GetActiveSprites();
        if (currentIndex >= activeSprites.Length)
        {
            currentIndex = 0; // 最初に戻る
        }
        else if (currentIndex < 0)
        {
            currentIndex = activeSprites.Length - 1; // 最後に戻る
        }

        UpdateContentDisplay(); // 表示を更新
    }

    private void UpdateContentDisplay()
    {
        if (flagManager == null)
            return;

        // フラグ状態を取得
        bool isARegistered = flagManager.GetFlag(FlagManager.FlagType.Adooropen);
        bool isBRegistered = flagManager.GetFlag(FlagManager.FlagType.Bdooropen);
        bool isCRegistered = flagManager.GetFlag(FlagManager.FlagType.Cdooropen);

        // 初期状態では未登録スプライト
        Sprite selectedSprite = sprites.Length > currentIndex ? sprites[currentIndex] : null;

        // フラグの状態に応じてクリアスプライトを適用
        if ((isARegistered && currentIndex <= 4) ||
            (isBRegistered && currentIndex <= 10) ||
            (isCRegistered && currentIndex <= 14))
        {
            selectedSprite = clearsprites.Length > currentIndex ? clearsprites[currentIndex] : selectedSprite;
        }

        // 選択されたスプライトを Image に適用
        if (contentImage != null && selectedSprite != null)
        {
            contentImage.sprite = selectedSprite;
        }
    }



    private Sprite[] GetActiveSprites()
    {
        if (flagManager == null)
            return sprites;

        // FlagManager の状態に基づいて使用するSprite配列を返す
        return flagManager.GetFlag(FlagManager.FlagType.Motosen) ? clearsprites : sprites;
    }

    private void PlayAudio()
    {
        if (audioSource != null)
        {
            audioSource.Play(); // 音声再生
        }
    }

    private void HandleFire1Input()
    {
        // ③Fire1が入力された場合の処理
        contentImage.gameObject.SetActive(false);
        flagManager.SetFlag(FlagManager.FlagType.Zukan, false);
    }
}