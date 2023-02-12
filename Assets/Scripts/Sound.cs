using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField]
    AudioSource bonusSound;

    [SerializeField]
    AudioSource padHitsBallSound;

    public void PlayPadHitsBall()
    {
        padHitsBallSound.Play();
    }

    public void PlayBonusPickup()
    {
        bonusSound.Play();
    }
}
