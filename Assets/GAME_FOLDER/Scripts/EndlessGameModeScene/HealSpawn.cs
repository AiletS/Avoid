using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealSpawn : MonoBehaviour
{
    [SerializeField] float spawn_time = 20f;
    [SerializeField] GameObject heal_obj;

    private float time = 0;

    SaveData data;

    private void Start()
    {
        data = GameManager.instance.data;
        TRIAL trial = (TRIAL)Enum.Parse(typeof(TRIAL), "NoHeal");
        int trial_id = (int)trial;
        if (data.TrialList[trial_id] == 1) spawn_time = 40f;
    }


    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        if(time >= spawn_time)
        {
            float x = UnityEngine.Random.Range(-8f, 8f);
            float y = UnityEngine.Random.Range(-4f, 4f);
            GameObject h = Instantiate(heal_obj);
            h.transform.position = new Vector2(x, y);
            time = 0;
        }
    }
}
