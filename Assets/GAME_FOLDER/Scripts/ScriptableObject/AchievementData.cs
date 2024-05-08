using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AchievementData", menuName = "GAMEDATA/AchievementData")]
public class AchievementData : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private Sprite[] _AchImage;
    [NonSerialized] public Sprite[] AchImage;

    [SerializeField] public Material Material_Yes;
    [SerializeField] public Material Material_No;

    public void OnBeforeSerialize() { }
    public void OnAfterDeserialize()
    {
        // �����^�C���ł̏������ݗp�ɒl���R�s�[����
        AchImage = _AchImage;
    }
}
