using UnityEngine;
using UnityEngine.UI;

public class PasswordFlask : MonoBehaviour
{
    [Header("正解の色")]
    [SerializeField] private ColorState[] _correctColors;
    private bool _isCorrect;
    private enum InputState
    {
        Beaker,
        Frasco
    }
    private InputState _currentState;
    
    private enum ColorState
    {
        None,
        Yellow,
        Blue,
        Green
    }
    [SerializeField] private GameObject jouro; // 最初に非表示のオブジェクト
    [SerializeField] private GameObject _beakerSelected;
    [SerializeField] private GameObject[] _beakers;
    private int _currentBeakerNum;
    
    [SerializeField] private GameObject _flaskSelected;
    [SerializeField] private Image[] _flasks;
    private int _currentFlaskNum;
    private ColorState[] _flaskColors;
    
    [SerializeField] private GameObject _frascoPanel;
    private bool _playerInsideCollider;
    private bool _playerSolving;
    // 入力処理の待機時間
    private const float InputPadding = 0.2f;
    private float _currentInputPadding;

    private void Start()
    {
        _currentState = InputState.Beaker;
        _flaskColors = new ColorState[_flasks.Length];
    }
    
    private void Update()
    {
        SolvingHandler();
        PanelHandler();
    }
    
    private void SolvingHandler()
    {
        if (!_playerSolving || _isCorrect) return;
        if (Input.GetButtonDown("Fire2")) SolvingInput();
        SolvingSelected();
    }
    
    // オブジェクトに対する〇ボタンの処理
    private void SolvingInput()
    {
        switch (_currentState)
        {
            case InputState.Beaker:
                _currentState = InputState.Frasco;
                break;
            case InputState.Frasco:
                _currentState = InputState.Beaker;
                _flaskColors[_currentFlaskNum] = GetNewFlaskColor(_currentBeakerNum, _currentFlaskNum, _flaskColors);
                ChangeColor(_flasks[_currentFlaskNum], _flaskColors[_currentFlaskNum]);
                CheckCorrect();
                break;
        }
    }
    
    // 正解判定
    private void CheckCorrect()
    {
        for (var i = 0; i < _flaskColors.Length; i++)
        {
            if (_correctColors[i] == _flaskColors[i]) continue;
            // 不正解の処理
            _isCorrect = false;
            Debug.Log("ミス！！");
            return;
        }
        // 正解の処理
        _isCorrect = true;
        FlagManager.Instance.SetFlag(FlagManager.FlagType.MixPasswordclear,true);
        jouro.SetActive(true);
        Debug.Log("やるじゃん。");
    }
    
    // 対象の選択処理
    private void SolvingSelected()
    {
        if (_currentInputPadding > 0)
        {
            _currentInputPadding -= Time.deltaTime;
            return;
        }
        _currentInputPadding = InputPadding;
        var input = Input.GetAxisRaw("Horizontal Stick-L");
        if (input == 0) return;
        input = input < 0 ? -1 : 1;
        
        switch (_currentState)
        {
            case InputState.Beaker:
                _currentBeakerNum = ClampSelectedValue(_currentBeakerNum + (int)input, _beakers.Length);
                _beakerSelected.transform.position = _beakers[_currentBeakerNum].transform.position;
                break;
            case InputState.Frasco:
                _currentFlaskNum = ClampSelectedValue(_currentFlaskNum + (int)input, _flasks.Length);
                var pos = _flaskSelected.transform.position;
                pos.x = _flasks[_currentFlaskNum].transform.position.x;
                _flaskSelected.transform.position = pos;
                break;
        }
    }
    
    // 仮置き
    private void PanelHandler()
    {
        if (!_playerInsideCollider) return;
        if (!_playerSolving) if (Input.GetButtonDown("Fire2")) ChangeFrascoPanel(true);
        if (Input.GetButtonDown("Fire1")) ChangeFrascoPanel(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) _playerInsideCollider = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) _playerInsideCollider = false;
    }
    
    // 仮置き
    private void ChangeFrascoPanel(bool isActive)
    {
        _frascoPanel.SetActive(isActive);
        _playerSolving = isActive;
    }

    private int ClampSelectedValue(int value, int max)
    {
        if (value < 0) value = max - 1;
        if (value == max) value = 0;
        return value;
    }
    
    private ColorState GetNewFlaskColor(int currentBeakerNum, int currentFlaskNum, ColorState[] flaskColors)
    {
        var newColor = currentBeakerNum == 0 ? ColorState.Yellow : ColorState.Blue;
        switch (flaskColors[currentFlaskNum])
        {
            case ColorState.Yellow when newColor == ColorState.Blue:
            case ColorState.Blue when newColor == ColorState.Yellow:
                newColor = ColorState.Green;
                break;
        }
        return newColor;
    }
    
    // 仮置き
    private void ChangeColor(Image target, ColorState newColor)
    {
        switch (newColor)
        {
            case ColorState.Yellow:
                target.color = Color.yellow;
                break;
            case ColorState.Blue:
                target.color = Color.blue;
                break;
            case ColorState.Green:
                target.color = Color.green;
                break;
        }
    }
    
}

