using UnityEngine;

[System.Serializable]
public class RunStatistics
{
    public int enemiesKilled;
    public int encountersSurvived;
    public int totalDamageTaken;
    public int damageAbsorbed;
    public float timeSpentInBattle;
    public int perfectFights;
    public int highestSingleTurnDamage;

    // Call this when starting a new run!
    public void Reset()
    {
        enemiesKilled = 0;
        encountersSurvived = 0;
        totalDamageTaken = 0;
        damageAbsorbed = 0;
        timeSpentInBattle = 0f;
        perfectFights = 0;
        highestSingleTurnDamage = 0;
    }

    // for umbraco to convert to json and save to file
    public string ToJson()
    {
        return JsonUtility.ToJson(this, true);
    }
}