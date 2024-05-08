using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PatternData", menuName = "GAMEDATA/PatternData")]
public class PatternData : ScriptableObject, ISerializationCallbackReceiver
{
    // ScriptableObject �Ɋւ��Ă͂������Q�� https://elekibear.com/post/20220730_01

    // ���݂̃p�^�[����
    [SerializeField] private int _level;
    [NonSerialized] public int level;

    // �p�^�[���̃_���[�W
    [SerializeField] private int _damage;
    [NonSerialized] public int damage;

    // ���ꂼ��̃p�^�[���̎�������
    [SerializeField] private int[] _PatternTimes;
    [NonSerialized] public int[] PatternTimes;

    // ���ꂼ��̃��x���̎�������
    [SerializeField] private int[] _LevelTimes;
    [NonSerialized] public int[] LevelTimes;

    // �p�^�[���̃X�R�A�{��
    [SerializeField] private float[] _PatternScoreMul;
    [NonSerialized] public float[] PatternScoreMul;

    // �Q�[���I�[�o�[����
    [SerializeField] private bool _is_Gameover;
    [NonSerialized] public bool is_Gameover;

    public void OnBeforeSerialize() { }
    public void OnAfterDeserialize()
    {
        // �����^�C���ł̏������ݗp�ɒl���R�s�[����
        level = _level;
        damage = _damage;
        PatternTimes = _PatternTimes;
        LevelTimes = _LevelTimes;
        is_Gameover = _is_Gameover;
        PatternScoreMul = _PatternScoreMul;
    }
}
