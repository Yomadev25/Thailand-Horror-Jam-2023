using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Setting")]
    [SerializeField]
    private int _targetCandyCount;
    [SerializeField]
    private int _currentCandyCount;
    [Space]
    [SerializeField]
    private GameObject[] _candies; 
    [SerializeField]
    private Transform[] _candySpawnPoint;
    [SerializeField]
    private float _hintDuration;
    [SerializeField]
    private UnityEvent onStart;
    [SerializeField]
    private UnityEvent onCollectedAllCandy;

    [Header("Title HUD")]
    [SerializeField]
    private CanvasGroup _titleHud;

    [Header("Gameplay HUD")]
    [SerializeField]
    private GameObject _gameplayHud;
    [SerializeField]
    private TMP_Text _candyText;
    [SerializeField]
    private TMP_Text _objectiveText;
    [SerializeField]
    private Image _maskedImage;
    [SerializeField]
    private Image _maskedFill;
    [SerializeField]
    private Image _distractImage;
    [SerializeField]
    private Transform _waypointRoot;
    [SerializeField]
    private GameObject _waypointTemplate;
    [SerializeField]
    private GameObject _eButton;

    [Header("Complete HUD")]
    [SerializeField]
    private CanvasGroup _completeHud;
    [SerializeField]
    private TMP_Text _timeText;   

    [Header("References")]
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private UnityEvent _onGameover;

    [Header("Time Recorder")]
    private DateTime _sessionStartTime;

    bool isStart;

    private void Awake()
    {
        if (instance == null) instance = this;       
    }

    private void Start()
    {
        TransitionManager.Instance.SceneFadeOut();
        InitCandy();
        UpdateCandyText();       
    }

    private void Update()
    {
        if (!isStart && Input.anyKeyDown && !Input.GetMouseButtonDown(0))
        {
            GameStart();
        }

        if (isStart && Input.GetKeyDown(KeyCode.F5))
        {
            Gameover();
        }
    }

    private void InitCandy()
    {
        for (int i = 0; i < _targetCandyCount; i++)
        {
            GameObject candy = _candies[Random.Range(0, _candies.Length)];
            Transform spawnPoint = _candySpawnPoint[Random.Range(0, _candySpawnPoint.Length)];

            while (true)
            {
                if (spawnPoint.childCount <= 0)
                {
                    break;
                }

                spawnPoint = _candySpawnPoint[Random.Range(0, _candySpawnPoint.Length)];
            }

            Instantiate(candy, spawnPoint.position, Quaternion.identity, spawnPoint).SetActive(true);
        }

        _objectiveText.text = "Find all candies";
    }

    private void GameStart()
    {
        SetCursor(true);
        _titleHud.LeanAlpha(0, 0.5f).setOnComplete(() => _titleHud.gameObject.SetActive(false));
        _player.SetActive(true);
        _gameplayHud.SetActive(true);
        _sessionStartTime = DateTime.Now;

        onStart?.Invoke();

        isStart = true;
    }

    private void SetCursor(bool isLock)
    {
        Cursor.lockState = isLock? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = isLock ? false : true;
    }

    public void GetCandy()
    {
        _currentCandyCount++;

        if (_currentCandyCount >= _targetCandyCount)
        {
            _objectiveText.text = "Find a way out";
            _objectiveText.transform.LeanScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f).setEaseOutSine().setOnComplete(() =>
            {
                _objectiveText.transform.LeanScale(Vector3.one, 0.2f).setEaseInSine();
            });

            onCollectedAllCandy?.Invoke();
        }

        UpdateCandyText();
    }

    public void GameComplete()
    {
        if (_currentCandyCount >= _targetCandyCount)
        {
            TimeSpan sessionDuration = DateTime.Now - _sessionStartTime;

            SetCursor(false);
            _player.SetActive(false);

            _timeText.text = sessionDuration.TotalMinutes.ToString("0.00");
            _completeHud.gameObject.SetActive(true);

            _completeHud.LeanAlpha(1, 1);
        }
        else
        {
            Debug.Log("Return to get all candy");
        }
    }

    public void Gameover()
    {
        _onGameover?.Invoke();
        SetCursor(false);

        TransitionManager.Instance.SceneFadeIn(2, () =>
        {
            SceneManager.LoadScene(0);
        });
    }

    #region HUD MANAGEMENT
    private void UpdateCandyText()
    {
        _candyText.text = _currentCandyCount.ToString() + "/" + _targetCandyCount.ToString();
    }

    public void UpdateMaskedSkillImage(bool isActive)
    {
        _maskedImage.color = isActive ? Color.white : Color.gray;

        if (isActive)
        {
            _maskedImage.transform.LeanScale(new Vector3(1.2f, 1.2f, 1.2f), 0.1f).setEaseOutSine().setOnComplete(() =>
            {
                _maskedImage.transform.LeanScale(Vector3.one, 0.1f).setEaseInSine();
            });
        }
    }

    public void UpdateMaskedFill(float value)
    {
        _maskedFill.fillAmount = value;
    }

    public void UpdateDistractSkillImage(bool isActive)
    {
        _distractImage.color = isActive ? Color.white : Color.gray;
    }

    public void ActiveCandyWaypoint()
    {
        GameObject[] candy = GameObject.FindGameObjectsWithTag("Candy");
        List<Image> waypoints = new List<Image>();

        for (int i = 0; i < candy.Length; i++)
        {
            GameObject GO = Instantiate(_waypointTemplate, _waypointRoot);
            GO.GetComponent<Waypoint>().target = candy[i].transform;
            waypoints.Add(GO.GetComponent<Image>());

            GO.SetActive(true);
        }

        LeanTween.value(1 ,0, _hintDuration).setOnUpdate((x) =>
        {
            foreach (Image waypoint in waypoints)
            {
                waypoint.color = new Color(1, 1, 1, x);
            }
        }).setOnComplete(() =>
        {
            foreach (Image waypoint in waypoints)
            {
                Destroy(waypoint.gameObject);
            }

            waypoints.Clear();
        });
    }

    public void ActiveEButton(bool isActive)
    {
        if (isActive)
        {
            _eButton.SetActive(true);
        }
        else
        {
            _eButton.SetActive(false);
        }
    }
    #endregion
}
