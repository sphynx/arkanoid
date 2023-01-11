using UnityEngine;

public enum BonusType
{
    WIDE_PAD = 1,
    MULTIPLE_BALLS = 2,
    EXTRA_LIFE = 3,
    GUN = 4,
}

[CreateAssetMenu]
public class BonusSpawner : ScriptableObject
{
    [System.Serializable]
    public class Bonus
    {
        public BonusType type;
        public float probability;
        public GameObject prefab;
    }

    public Bonus[] Bonuses;

    public Bonus GenerateRandomBonus()
    {
        float r = Random.Range(0f, 100f);
        float total = 0f;

        foreach (var bonus in Bonuses)
        {
            total += bonus.probability;
            if (r <= total)
                return bonus;
        }

        return null;
    }

    public GameObject SpawnBonus(Vector3 where)
    {
        Bonus bonus = GenerateRandomBonus();
        GameObject result = null;

        if (bonus != null)
        {
            result = Instantiate(bonus.prefab, where, Quaternion.identity);
        }

        return result;
    }
}
