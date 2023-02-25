using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System;

public class GameLogic : MonoBehaviour
{
    public static event Action OnGameOver;
    public static event Action<int> OnLevelCleared;

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
        Bricks.OnBrickDestroyed += HandleBrokenBrick;
    }

    private void OnDisable()
    {
        Pad.OnLostLife -= HandleOnLostLife;
        Bricks.OnAllBricksDestroyed -= HandleNextLevel;
        Bricks.OnBrickDestroyed -= HandleBrokenBrick;
    }

    private void Awake()
    {
        LoadLevelMap(level.Value);
    }

    void HandleOnLostLife()
    {
        lives.Value -= 1;
        if (lives.Value == 0)
        {
            OnGameOver?.Invoke();
            StartCoroutine(nameof(GameOver));
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("GameOver");
    }

    void Won()
    {
        SceneManager.LoadScene("Won");
    }

    void HandleNextLevel()
    {
        OnLevelCleared?.Invoke(level.Value);
        StartCoroutine(nameof(LoadNewLevel));
    }

    IEnumerator LoadNewLevel()
    {
        yield return new WaitForSeconds(0.7f);

        level.Value += 1;
        SceneManager.LoadScene("Levels");
    }

    void LoadLevelMap(int levelNo)
    {
        if (levelNo >= 1 && levelNo <= levelPrefabs.Length)
        {
            // Load a level prefab. Note that level numbers are 1-based.
            GameObject levelPrefab = levelPrefabs[levelNo - 1];
            Instantiate(levelPrefab, levelContainer);
        }
        else if (levelNo > levelPrefabs.Length)
        {
            Won();
        }
    }

    void HandleBrokenBrick(TileBase _brick)
    {
        score.Value += 1;
    }
}
