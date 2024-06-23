using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance => _instance;

    public Action<int> OnLifeValueChange;
    public Action<int> OnScoreValueChange;

    private int _lives;
    private int _score;

    public int lives
    {
        get => _lives;
        set
        {
            if (value <= 0)
            {
                GameOver();
                return;
            }
            if (value < _lives) Respawn();
            if (value > maxLives) value = maxLives;
            _lives = value;

            OnLifeValueChange?.Invoke(_lives);

            Debug.Log($"Lives have been set to {_lives}");
        }
    }

    public int score
    {
        get => _score;
        set
        {
            _score = value;
            OnScoreValueChange?.Invoke(_score);
            Debug.Log($"Score has been set to {_score}");
        }
    }

    [SerializeField] private int maxLives = 5;
    [SerializeField] private PlayerController playerPrefab;

    [HideInInspector] public PlayerController PlayerInstance => _playerInstance;
    PlayerController _playerInstance = null;
    Transform currentCheckpoint;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    private void Start()
    {
        if (maxLives <= 0)
        {
            maxLives = 5;
        }
        lives = maxLives;
        score = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "Game")
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(1);
            }
        }

        if (lives == 0)
        {
            SceneManager.LoadScene(1);
        }
    }

    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    private void GameOver()
    {
        Debug.Log("GameOver goes here");
        SceneManager.LoadScene("GameOver");
    }

    private void Respawn()
    {
        _playerInstance.transform.position = currentCheckpoint.position;
    }

    public void SpawnPlayer(Transform spawnLocation)
    {
        _playerInstance = Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);
        currentCheckpoint = spawnLocation;
    }

    public void UpdateCheckpoint(Transform updatedCheckpoint)
    {
        currentCheckpoint = updatedCheckpoint;
    }

    public void AddScore(int amount)
    {
        score += amount;
    }
}
