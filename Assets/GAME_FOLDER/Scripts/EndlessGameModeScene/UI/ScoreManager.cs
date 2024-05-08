using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private PatternData patternData;
    public float score = 0;
    private float score_mul = 1f;
    private float score_mul_time = 60f;

    SaveData data;

    private void Start()
    {
        data = GameManager.instance.data;
        scoreText.text = "0";
        DOVirtual.Float(1f, data.score_multiply, score_mul_time,
            (value) => { score_mul = value; }).SetEase(Ease.InQuart);
    }

    private void FixedUpdate()
    {
        score += (float)patternData.level 
            * Time.fixedDeltaTime * 1.5f * score_mul;
        scoreText.text = ((int)score).ToString("D8");
    }
}
