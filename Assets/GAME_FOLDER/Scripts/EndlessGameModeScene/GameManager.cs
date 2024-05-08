using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;


public class GameManager : MonoBehaviour
{
    // インスタンス
    static public GameManager instance;

    // データ

    public SaveData data;
    public int level;
    [SerializeField] PatternData patternData;

    // オープニング
    [SerializeField] TextMeshProUGUI cd_text;
    [SerializeField] Image[] hpbar;

    // bgm
    [SerializeField] public AudioSource audiosource;
    [SerializeField] public AudioClip bgm;


    // ===========================================================

    private void Awake()
    {
        DG.Tweening.DOTween.SetTweensCapacity(tweenersCapacity: 800, sequencesCapacity: 200);
        instance = this;
        level = 0;
    }

    private void Start()
    {
        data = GetComponent<SaveLoadManager>().data;
        data.PlayCount++;
        patternData.damage = 33;
        if (data.TrialList[11] == 1) patternData.damage = 99;
        DOVirtual.DelayedCall(0.5f, () => { countdown_opening(); })
            .SetLink(gameObject, LinkBehaviour.KillOnDisable).Play();
        audiosource.volume = data.BGMVolume;
        audiosource.Play();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("EndlessModeScene");
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("OpeningScene_Main");
        }
    }

    public void save_score(long score)
    {
        // ハイスコアの更新
        if (score > data.HighScore)
        {
            data.HighScore = score;
            data.Highscore_TrialList = data.TrialList;
        }

        // 実績データの更新
        int trial_count = 0;
        for(int i = 0; i < 24; i++)
        {
            trial_count += data.TrialList[i / 2];
            if (i % 2 == 0 && data.TrialList[i / 2] == 1 && score >= 4000) data.AchievementList[i] = 1;
            else if(i % 2 == 1 && data.TrialList[i / 2] == 1 && score >= 8000) data.AchievementList[i] = 1;
        }
        if (trial_count == 24 && score >= 50000) data.AchievementList[24] = 1;
        

        GetComponent<SaveLoadManager>().Save(data);
    }

    private void countdown_opening()
    {
        var sequence = DOTween.Sequence();

        // hpバー
        var sequence_2 = DOTween.Sequence();
        sequence_2.Append(hpbar[0].DOColor(Color.red, 0f));
        sequence_2.Append(hpbar[1].DOColor(Color.red, 0f));
        sequence_2.Append(DOVirtual.Float(0f, 1f, 3f, (value) =>
        {
            hpbar[0].GetComponent<Image>().fillAmount = value;
            hpbar[1].GetComponent<Image>().fillAmount = value;
        }));
        sequence_2.Join(hpbar[0].DOColor(Color.green, 3f));
        sequence_2.Join(hpbar[1].DOColor(Color.green, 3f));

        // カウントダウン
        RectTransform pos = cd_text.GetComponent<RectTransform>();
        string s;
        for(int i = 3; i >= 1; i--)
        {
            s = "" + i;
            sequence
                .Append(cd_text.DOFade(0f, 0f))
                .Join(cd_text.DOText(s, 0f, false))
                .Join(pos.DOAnchorPosX(200f, 0f))
                .Append(pos.DOAnchorPosX(0f, 0.3f))
                .Join(cd_text.DOFade(1f, 0.3f))
                .AppendInterval(0.4f)
                .Append(pos.DOAnchorPosX(-200f, 0.3f))
                .Join(cd_text.DOFade(0f, 0.3f));
        }

        // 実行
        sequence
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play();
        sequence_2
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play();
    }
}
