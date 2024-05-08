using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PatternData", menuName = "GAMEDATA/PatternData")]
public class PatternData : ScriptableObject, ISerializationCallbackReceiver
{
    // ScriptableObject に関してはここを参照 https://elekibear.com/post/20220730_01

    // 現在のパターン数
    [SerializeField] private int _level;
    [NonSerialized] public int level;

    // パターンのダメージ
    [SerializeField] private int _damage;
    [NonSerialized] public int damage;

    // それぞれのパターンの持続時間
    [SerializeField] private int[] _PatternTimes;
    [NonSerialized] public int[] PatternTimes;

    // それぞれのレベルの持続時間
    [SerializeField] private int[] _LevelTimes;
    [NonSerialized] public int[] LevelTimes;

    // パターンのスコア倍率
    [SerializeField] private float[] _PatternScoreMul;
    [NonSerialized] public float[] PatternScoreMul;

    // ゲームオーバー判定
    [SerializeField] private bool _is_Gameover;
    [NonSerialized] public bool is_Gameover;

    public void OnBeforeSerialize() { }
    public void OnAfterDeserialize()
    {
        // ランタイムでの書き込み用に値をコピーする
        level = _level;
        damage = _damage;
        PatternTimes = _PatternTimes;
        LevelTimes = _LevelTimes;
        is_Gameover = _is_Gameover;
        PatternScoreMul = _PatternScoreMul;
    }
}
