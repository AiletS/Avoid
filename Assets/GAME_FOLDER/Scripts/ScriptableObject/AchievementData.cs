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
        // ランタイムでの書き込み用に値をコピーする
        AchImage = _AchImage;
    }
}
