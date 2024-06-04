using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance => _instance;

    private int _lives;
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

            Debug.Log($"Lives have been set to {_lives}");
        }
    }

    [SerializeField] private int maxLives = 5;
    [SerializeField] private PlayerController playerPrefab;

    [HideInInspector] public PlayerController PlayerInstance => _playerInstance;
    PlayerController _playerInstance = null;
    Transform currentCheckpoint;



    // Start is called before the first frame update
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "Game")
        { 
                SceneManager.LoadScene(0);
        }
        else
            SceneManager.LoadScene(1);
        }

        if (lives == 0)
        {
            SceneManager.LoadScene(2);
        }
            
    }

    private void GameOver()
    {
        Debug.Log("GameOver goes here");
        SceneManager.LoadScene(0);
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
}
