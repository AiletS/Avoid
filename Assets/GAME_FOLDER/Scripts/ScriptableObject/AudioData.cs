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
        // �����^�C���ł̏������ݗp�ɒl���R�s�[����
    }
}
