using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text scoreText;

    [SerializeField]
    TMP_Text levelText;

    [SerializeField]
    GameObject heartPrefab;

    [SerializeField]
    IntVar score;

    [SerializeField]
    IntVar lives;

    List<GameObject> hearts;
    Animator levelTextAnimator;

    void Start()
    {
        scoreText.text = "";
        hearts = new List<GameObject>();
        levelTextAnimator = levelText.GetComponent<Animator>();
    }

    void Update()
    {
        SetScore(score.Value);
        SetLives(lives.Value);
    }

    private void OnEnable()
    {
        GameLogic.OnLevelChange += SetLevel;
    }

    private void OnDisable()
    {
        GameLogic.OnLevelChange -= SetLevel;
    }

    void SetLives(int lives)
    {
        if (lives < hearts.Count)
        {
            for (int i = hearts.Count - 1; i >= lives && i >= 0; i--)
            {
                Destroy(hearts[i]);
                hearts.RemoveAt(i);
            }
        }
        else if (lives > hearts.Count)
        {
            for (int i = 0; i < lives; i++)
            {
                Vector2 position = new Vector2(-16.5f + 1.1f * i, -9.2f);
                GameObject newHeart = Instantiate(heartPrefab, position, Quaternion.identity);
                hearts.Add(newHeart);
            }
        }
    }

    void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }

    void SetLevel(int level)
    {
        levelText.text = $"Level {level}";
        levelTextAnimator.Play("LevelTitleAnim", -1, 0f);
    }
}
