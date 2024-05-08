using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SkillTrialUIImage : MonoBehaviour
{
    [SerializeField] string TrialName;
    [SerializeField] Sprite[] TrialSprites;

    SaveData data;

    private void Start()
    {
        data = GameManager.instance.data;
        TRIAL trial = (TRIAL)Enum.Parse(typeof(TRIAL), TrialName);
        int id = (int)trial;
        GetComponent<Image>().sprite = TrialSprites[data.TrialList[id]];
    }
}
