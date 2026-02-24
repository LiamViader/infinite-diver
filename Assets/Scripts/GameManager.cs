using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PlayerControls _controls;

    public event Action OnGameStart;
    public event Action OnGameOver;
    public event Action OnDifficultyIncreased;

    private bool _isGameActive;

    private InputAction _startGameAction;

    private int _distanceTraveled = 0;
    private int _personalBest = 0;

    private int _nextDifficultyThreshold = 100;




    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        _personalBest = PlayerPrefs.GetInt("PersonalBest", 0);
        InitializeInput();
    }

    private void Start()
    {
        UIManager.Instance.SetPBText(_personalBest);
    }


    private void InitializeInput()
    {
        _controls = new PlayerControls();
        _startGameAction = _controls.GameControls.StartGame;
        _startGameAction.Enable();

        _startGameAction.performed += context => StartGame();
    }

    private void StartGame()
    {
        _nextDifficultyThreshold = 100;
        _isGameActive = true;
        _startGameAction.Disable();
        UIManager.Instance.HideStartTexts();
        UIManager.Instance.ShowGameplayTexts();
        OnGameStart?.Invoke();
    }


    public void EndGame()
    {
        _isGameActive = false;
        UIManager.Instance.ShowStartTexts();
        UIManager.Instance.HideGameplayTexts();
        UIManager.Instance.SetLastRecordText(_distanceTraveled);
        if (_personalBest < _distanceTraveled)
        {
            _personalBest = _distanceTraveled;
            PlayerPrefs.SetInt("PersonalBest", _personalBest); 
            PlayerPrefs.Save();
        }
        UIManager.Instance.SetPBText(_personalBest);
        OnGameOver?.Invoke();
        StartCoroutine(EnableStartAfterDelay(0.5f));
    }

    public bool IsGameActive()
    {
        return _isGameActive;
    }

    public void UpdateDistanceTraveled(float distance)
    {
        int newDistance = Mathf.FloorToInt(distance);

        if (newDistance >= _nextDifficultyThreshold)
        {
            OnDifficultyIncreased?.Invoke();
            _nextDifficultyThreshold += 100;
        }
        _distanceTraveled = newDistance;
        UIManager.Instance.UpdateGameplayDistanceText(_distanceTraveled);
    }

    
    

    private IEnumerator EnableStartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _startGameAction.Enable();
    }

    private void OnDisable()
    {
        _startGameAction.Disable();
    }

    private void OnEnable()
    {
        _startGameAction.Enable();
    }
}