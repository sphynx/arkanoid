using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameLogic : MonoBehaviour
{
    public static event Action OnGameOver;
    public static event Action<int> OnLevelChange;

    [SerializeField]
    IntVar lives;

    [SerializeField]
    IntVar score;

    [SerializeField]
    IntVar level;

    [SerializeField]
    Transform levelContainer;

    [SerializeField]
    GameObject[] levelPrefabs;

    private void OnEnable()
    {
        Pad.OnLostLife += HandleOnLostLife;
        Bricks.OnAllBricksDestroyed += HandleNextLevel;
    }

    private void OnDisable()
    {
        Pad.OnLostLife -= HandleOnLostLife;
        Bricks.OnAllBricksDestroyed -= HandleNextLevel;
    }

    private void Start()
    {
        lives.Value = 3;
        score.Value = 0;
        level.Value = 1;

        LoadLevel();
    }

    void HandleOnLostLife()
    {
        lives.Value -= 1;
        if (lives.Value == 0)
        {
            OnGameOver?.Invoke();
            GameOver();
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    void HandleNextLevel()
    {
        level.Value += 1;
        OnLevelChange?.Invoke(level.Value);
        LoadLevel();
    }

    public void LoadLevel()
    {
        Debug.Log($"Loading level: {level.Value}");

        int levelNo = level.Value;

        if (levelNo >= 1 && levelNo <= levelPrefabs.Length)
        {
            // Destroy existing level, if any.
            for (int i = 0; i < levelContainer.childCount; i++)
            {
                Destroy(levelContainer.GetChild(i).gameObject);
            }

            // Load a new one. Note that level numbers are 1-based.
            GameObject levelPrefab = levelPrefabs[levelNo - 1];
            Instantiate(levelPrefab, levelContainer);
        }
    }
}
