using UnityEngine;
using UnityEngine.Tilemaps;

public class Sound : MonoBehaviour
{
    [SerializeField]
    AudioSource bonusSound;

    [SerializeField]
    AudioSource padHitsBallSound;

    [SerializeField]
    AudioSource brickHitSound;

    private void OnEnable()
    {
        Bricks.OnBrickHit += PlayBrickHitSound;
        Pad.OnBonusPickup += PlayBonusPickup;
        Pad.OnPadHitsBall += PlayPadHitsBall;
    }

    private void OnDisable()
    {
        Bricks.OnBrickHit -= PlayBrickHitSound;
        Pad.OnBonusPickup -= PlayBonusPickup;
        Pad.OnPadHitsBall -= PlayPadHitsBall;
    }

    public void PlayPadHitsBall()
    {
        padHitsBallSound.Play();
    }

    public void PlayBonusPickup(string _bonus)
    {
        bonusSound.Play();
    }

    public void PlayBrickHitSound(TileBase _tile)
    {
        brickHitSound.Play();
    }
}
