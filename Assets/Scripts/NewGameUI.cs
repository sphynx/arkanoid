using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameUI : MonoBehaviour
{
    [SerializeField]
    IntVar lives;

    [SerializeField]
    IntVar startLives;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            lives.Value = startLives.Value;
            SceneManager.LoadScene("Level1");
        };
    }
}
