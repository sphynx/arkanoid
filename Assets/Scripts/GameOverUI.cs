using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private IntVar score;

    [SerializeField]
    private IntVar hiScore;

    [SerializeField]
    IntVar lives;

    [SerializeField]
    IntVar startLives;

    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField]
    private TMP_Text hiScoreText;

    void Start()
    {
        if (score.Value > hiScore.Value)
            hiScore.Value = score.Value;

        scoreText.text = $"Your score: <color=red>{score.Value}</color>";
        hiScoreText.text = $"High score: {hiScore.Value}";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            lives.Value = startLives.Value;
            SceneManager.LoadScene("Level1");
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
