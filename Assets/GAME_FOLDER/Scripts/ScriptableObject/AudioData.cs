using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "GAMEDATA/AudioData")]
public class AudioData : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] public float BGMVolume;
    [SerializeField] public float SEVolume;

    public void OnBeforeSerialize() { }
    public void OnAfterDeserialize()
    {
        // ランタイムでの書き込み用に値をコピーする
    }
}
