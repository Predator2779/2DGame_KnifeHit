using System;
using UnityEngine;
using UnityEngine.UI;

public class ManagerScript : MonoBehaviour
{
    #region Переменные

    [Header("GameManager")]
    /// <summary>
    /// Публичные переменные.
    /// </summary>
    [Range(1, 6)] public int _difficulty = 1;
    public ThrowScript _throwScript;

    /// <summary>
    /// Сериализованные приватные переменные.
    /// </summary>
    [SerializeField] private TargetScript _targetScript;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Slider _knifeSkinSlider;
    [SerializeField] private Slider _targetSkinSlider;
    [SerializeField] private GameObject _bonus;
    [SerializeField] private GameObject _game;
    [SerializeField] private GameObject _backButton;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _skinPanel;
    [SerializeField] private bool _playing = false;
    [SerializeField] private bool _spawnBonus = false;

    /// <summary>
    /// Приватные переменные.
    /// </summary>
    private GameObject _cloneGame;
    private Text _debug;
    private bool _timerMenu = false;
    private float _scaleTarget = 1;
    private float _timeMenu;
    private int _currentScores;
    private int _totalScores;

    #region Переменные TargetScript.

    [Header("TargetScript")]
    public int _hits = 0;
    public int _healthTarget = 1;
    public float _timeRestore = 0.06f;
    public float _displaceScale = 0.07f;
    public float _displaceYPos = 0.18f;
    public bool _isDestroy = false;
    public Sprite _targetSkin;
    [Range(-10.0f, 10.0f)] public float _targetSpeedRotation = 1.0f;

    #endregion

    #region Переменные ThrowScript.

    [Header("ThrowScript")]
    public GameObject _knife;
    public Sprite _knifeSkin;
    public bool _fire = true;
    public bool _holdThrow = false;
    public bool _isTwisted = false;
    public float _holdTime = 0;

    #endregion

    #endregion

    #region Методы

    /// <summary>
    /// Start.
    /// </summary>
    private void Start() 
    {
        DefaultValues();
    }

    /// <summary>
    /// Update.
    /// </summary>
    private void Update()
    {
        Playing();
        Difficulty();
        TimerMenuOn();
        ChangeSkin();
    }

    /// <summary>
    /// Начало игры.
    /// </summary>
    private void InstantiateGame()
    {
        _playing = true;

        _cloneGame = Instantiate(_game);

        _throwScript = _cloneGame.transform.Find("ThrowKhife").gameObject.GetComponent<ThrowScript>();
        _targetScript = _cloneGame.transform.Find("Target").gameObject.GetComponent<TargetScript>();

        _throwScript._knifeSkin = _knifeSkin;
        _targetScript._targetSkin = _targetSkin;

        var scale = ScaleTarget(_targetScript.transform);
        _targetScript.transform.localScale = new Vector2(scale, scale);

        SpawnBonus();
    }

    /// <summary>
    /// Победа в игре.
    /// </summary>
    private void Win()
    {
        if (_targetScript._hits >= _healthTarget)
        {
            _targetScript._isDestroy = true;
            _throwScript.gameObject.SetActive(false);

            LevelUp();
            _timerMenu = true;
            _playing = false;
        }
    }

    /// <summary>
    /// Переключатель "В игре".
    /// </summary>
    private void Playing()
    {
        if (_playing)
        {
            Win();
            DebugLog();

            _backButton.SetActive(true);
        }
    }

    /// <summary>
    /// Таймер включения победного меню.
    /// </summary>
    private void TimerMenuOn()
    {
        if (_timerMenu)
        {
            _timeMenu -= Time.deltaTime;

            if (_timeMenu <= 0)
            {
                _winPanel.SetActive(true);
                _timerMenu = false;
            }
        }
    }

    /// <summary>
    /// Кнопка в меню.
    /// </summary>
    public void ToMenu()
    {
        Destroy(_cloneGame);

        _menuPanel.gameObject.SetActive(true);
        _winPanel.gameObject.SetActive(false);
        _backButton.SetActive(false);
        _playing = false;

        SaveScores();
        PrintsScores();
    }

    /// <summary>
    /// Переиграть уровень.
    /// </summary>
    public void Replay()
    {
        Destroy(_cloneGame);

        _difficulty--;

        _menuPanel.gameObject.SetActive(false);
        _winPanel.gameObject.SetActive(false);
        InstantiateGame();
        DefaultValues();
    }

    /// <summary>
    /// Загрузить уровень.
    /// </summary>
    public void LoadLevel()
    {
        Destroy(_cloneGame);

        SaveScores();
        _menuPanel.gameObject.SetActive(false);
        _winPanel.gameObject.SetActive(false);
        InstantiateGame();
        DefaultValues();
    }

    /// <summary>
    /// Установка значений по умолчанию.
    /// </summary>
    public void DefaultValues()
    {
        _currentScores = 0;
        _healthTarget = (int)_difficulty * 3;
        _timeMenu = 2.0f;
    }

    /// <summary>
    /// Debug.
    /// </summary>
    private void DebugLog()
    {
        _debug = GameObject.Find("DebugText").GetComponent<Text>();

        _debug.text =
            $"_throw_time: {_holdTime}\n" +
            $"_difficulty: {_difficulty}\n" +
            $"_health_target: {_healthTarget - _targetScript._hits}";

        _scoreText.text = $"Score: {_currentScores}";
    }

    /// <summary>
    /// Изменение сложности игры.
    /// </summary>
    public void Difficulty()
    {
        if (_settingsPanel.gameObject.activeSelf)
        {
            var slider = GameObject.Find("DifficultySlider").GetComponent<Slider>();
            _difficulty = (int)slider.value;
            _healthTarget = (int)_difficulty * 3;
            var sliderText = slider.transform.Find("DifficultyText").gameObject.GetComponent<Text>();

            sliderText.text = _difficulty.ToString();
        }
    }

    /// <summary>
    /// Спаун бонуса.
    /// </summary>
    private void SpawnBonus()
    {
        if (_spawnBonus)
        {
            var bonusPos = new Vector2(_targetScript.transform.position.x, _targetScript.transform.position.y + _targetScript.transform.localScale.y + _bonus.transform.localScale.y);

            Instantiate(_bonus, bonusPos, Quaternion.identity);
        }
    }

    /// <summary>
    /// Смена скина.
    /// </summary>
    private void ChangeSkin()
    {
        if (_skinPanel.gameObject.activeSelf)
        {
            _knifeSkin = _knifeSkinSlider.GetComponent<ChangeSkin>()._skin.sprite;
            _targetSkin = _targetSkinSlider.GetComponent<ChangeSkin>()._skin.sprite;
        }
    }

    /// <summary>
    /// Размер мишени, в зависимости от сложности.
    /// </summary>
    /// <param name="target">Мишень</param>
    /// <returns></returns>
    private float ScaleTarget(Transform target)
    {
        _scaleTarget = target.localScale.x - (0.12f * _difficulty);
        return _scaleTarget;
    }

    /// <summary>
    /// Увеличение уровня сложности.
    /// </summary>
    public void LevelUp() => _difficulty++;

    /// <summary>
    /// Метод увеличения количества текущих очков.
    /// </summary>
    /// <param name="koef">коэффициент увеличения</param>
    public void IncreaseScore(float koef)
    {
        if (!_targetScript.gameObject.GetComponent<TargetScript>()._isDestroy)
        {
            _currentScores += (int)koef * 5;
        }
    }

    /// <summary>
    /// Сохранение игровых очков.
    /// </summary>
    private void SaveScores() => _totalScores += _currentScores;

    /// <summary>
    /// Вывод на экран всех собранных очков.
    /// </summary>
    private void PrintsScores()
    {
        _scoreText.text = $"Score: {_totalScores}";
    }

    #endregion
}
