// セーブデータクラス

[System.Serializable]
public class SaveData 
{
    public const int TrialLen = 30;
    public int[] TrialList = new int[TrialLen];
    public float score_multiply = 1f;

    public long PlayCount = 0;
    public long HighScore = 0;
    public int[] Highscore_TrialList = new int[TrialLen];

    public const int AchievementLen = 30;
    public int[] AchievementList = new int[AchievementLen];

    public float BGMVolume = 1f;
    public float SEVolume = 1f;
}
