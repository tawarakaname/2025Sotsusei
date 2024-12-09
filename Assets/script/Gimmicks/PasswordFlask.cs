using UnityEngine;
using System;


[System.Serializable]
public class FlaskColorSprites
{
    public Sprite yellowSprite;
    public Sprite blueSprite;
    public Sprite greenSprite;
}

public class PasswordFlask : MonoBehaviour
{
    private enum InputState { Beaker, Frasco }
    private enum ColorState { None, Yellow, Blue, Green }

    [Header("正解の色")]
    [SerializeField] private ColorState[] _correctColors;

    [Header("オブジェクト設定")]
    [SerializeField] private GameObject _beakerSelected;
    [SerializeField] private GameObject[] _beakers;
    [SerializeField] private GameObject _flaskSelected;
    [SerializeField] private GameObject[] _flasks;
    [SerializeField] private GameObject[] _bgPanels;

    [Header("各フラスコの色スプライト")]
    [SerializeField] private FlaskColorSprites[] _flaskColorSprites;

    [Header("選択時のスプライト")]
    [SerializeField] private Sprite yellowFlaskSelectedSprite;
    [SerializeField] private Sprite blueFlaskSelectedSprite;

    private InputState _currentState = InputState.Beaker;
    private ColorState[] _flaskColors;
    private GameObject _currentBgPanel;
    private bool _isCorrect;
    private bool _playerInsideCollider;
    private bool _playerSolving;
    private int _currentBeakerNum;
    private int _currentFlaskNum;

    private const float InputPadding = 0.2f;
    private float _currentInputPadding;

    private void Start()
    {
        _flaskColors = new ColorState[_flasks.Length];
        DeactivateAllPanels();
    }

    private void Update()
    {
        // フラグがすべてtrueでなければ、処理を行わない
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.MonitorPasswordclear) ||
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.monitorpanelopen) ||
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.JugCamera))
        {
            return; // 処理をスキップ
        }

        // フラグがすべてtrueの場合のみ動作
        if (!_playerSolving)
        {
            // フラグがtrueになったタイミングでプレイヤーが解いている状態にする
            _playerSolving = true;
        }

        if (_playerSolving) HandleSolving();
        HandlePanelInteraction();
    }


    private void HandleSolving()
    {
        if (_isCorrect || Input.GetButtonDown("Fire1")) ChangeFrascoPanel(false);
        if (Input.GetButtonDown("Fire2")) ToggleInputState();
        UpdateSelection();
    }

    private void ToggleInputState()
    {
        if (_currentState == InputState.Beaker)
        {
            _currentState = InputState.Frasco;
        }
        else
        {
            _currentState = InputState.Beaker;
            UpdateFlaskColor();
        }
    }

    private void UpdateSelection()
    {
        if (_currentInputPadding > 0)
        {
            _currentInputPadding -= Time.deltaTime;
            return;
        }

        _currentInputPadding = InputPadding;
        float input = Input.GetAxisRaw("Horizontal Stick-L");
        if (Mathf.Abs(input) < 0.2f) return;

        input = input < 0 ? -1 : 1;

        switch (_currentState)
        {
            case InputState.Beaker:
                _currentBeakerNum = ClampIndex(_currentBeakerNum + (int)input, _beakers.Length);
                _beakerSelected.transform.position = _beakers[_currentBeakerNum].transform.position;
                UpdateSelectedSprite();
                break;

            case InputState.Frasco:
                _currentFlaskNum = ClampIndex(_currentFlaskNum + (int)input, _flasks.Length);

                // **`_flaskSelected`を現在のフラスコ位置に更新**
                Vector3 newPosition = _flaskSelected.transform.position;
                newPosition.x = _flasks[_currentFlaskNum].transform.position.x;
                _flaskSelected.transform.position = newPosition;

                UpdateBgPanel();
                break;
        }
    }


    private void UpdateFlaskColor()
    {
        _flaskColors[_currentFlaskNum] = GetNewColor(_currentBeakerNum, _flaskColors[_currentFlaskNum]);
        ChangeColor(_flasks[_currentFlaskNum], _flaskColors[_currentFlaskNum]);
        CheckCorrect();
    }

    private void CheckCorrect()
    {
        _isCorrect = true;
        for (int i = 0; i < _flaskColors.Length; i++)
        {
            if (_flaskColors[i] != _correctColors[i])
            {
                _isCorrect = false;
                Debug.Log("ミス！！");
                return;
            }
        }
        FlagManager.Instance.SetFlag(FlagManager.FlagType.MixPasswordclear, true);
        Debug.Log("やるじゃん。");
    }

    private void UpdateBgPanel()
    {
        if (_currentBgPanel != null) _currentBgPanel.SetActive(false);
        _currentBgPanel = _bgPanels[_currentFlaskNum];
        _currentBgPanel.SetActive(true);
    }

    private void UpdateSelectedSprite()
    {
        var spriteRenderer = _flaskSelected.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) return;
        spriteRenderer.sprite = _currentBeakerNum == 0 ? yellowFlaskSelectedSprite : blueFlaskSelectedSprite;
    }

    private void HandlePanelInteraction()
    {
        if (!_playerInsideCollider) return;
        if (!_playerSolving && Input.GetButtonDown("Fire2")) ChangeFrascoPanel(true);
    }

    private void ChangeFrascoPanel(bool isActive)
    {
       
        _playerSolving = isActive;
        if (!isActive) DeactivateAllPanels();
    }

    private void DeactivateAllPanels()
    {
        foreach (var panel in _bgPanels) panel.SetActive(false);
        _currentBgPanel = null;
    }

    private int ClampIndex(int value, int max)
    {
        return (value + max) % max;
    }

    private ColorState GetNewColor(int beakerIndex, ColorState currentColor)
    {
        var newColor = beakerIndex == 0 ? ColorState.Yellow : ColorState.Blue;
        return currentColor switch
        {
            ColorState.Yellow when newColor == ColorState.Blue => ColorState.Green,
            ColorState.Blue when newColor == ColorState.Yellow => ColorState.Green,
            _ => newColor
        };
    }

    private void ChangeColor(GameObject flask, ColorState color)
    {
        var spriteRenderer = flask.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) return;

        var sprites = _flaskColorSprites[Array.IndexOf(_flasks, flask)];
        spriteRenderer.sprite = color switch
        {
            ColorState.Yellow => sprites.yellowSprite,
            ColorState.Blue => sprites.blueSprite,
            ColorState.Green => sprites.greenSprite,
            _ => null
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) _playerInsideCollider = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) _playerInsideCollider = false;
    }
}
