using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
// unityroom スコアランキング専用
using unityroom.Api;

public class GameoverManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameover_scoreText;
    [SerializeField] private Image gameover_bg;
    [SerializeField] private GameObject RestartText;
    [SerializeField] private TextMeshProUGUI Highscore_Text;
    [SerializeField] private GameObject UICanvas;
    [SerializeField] private Image[] Trials;
    [SerializeField] private SpriteData spriteData;
    [SerializeField] private LineRenderer[] lines;

    [SerializeField] private GameObject gameover_nonactive;


    public void Gameover()
    {
        // TODO : 演出適用
        PatternManager.instance.Gameover();
        scoreText.gameObject.SetActive(false);
        PlayerMove.instance.gameObject.SetActive(false);
        UICanvas.SetActive(false);

        
        // データの取得
        float score = GetComponent<ScoreManager>().score;
        for(int i = 0; i < Trials.Length; i++)
        {
            Trials[i].GetComponent<Image>().sprite
                = spriteData.TrialSprites[GameManager.instance.data.TrialList[i] + i * 2];
        }
        bool is_highscore = score > GameManager.instance.data.HighScore;

        if(is_highscore)
        {
            for(int i = 0; i < 2; i++)
            {
                lines[i].startColor = Color.yellow;
                lines[i].endColor = Color.yellow;
            }
        }

        // アニメーション
        var sequence = DOTween.Sequence();
        var t_sequence = DOTween.Sequence();
        var h_sequence = DOTween.Sequence();
        for (int i = 0; i < Trials.Length; i++)
        {
            t_sequence.Append(Trials[i].DOFade(1f, 0.2f));
        }
        h_sequence.Join(gameover_scoreText.DOColor(Color.yellow, 0.2f).SetLoops(6, LoopType.Yoyo));
        h_sequence.Join(Highscore_Text.DOColor(Color.yellow, 0.2f).SetLoops(6, LoopType.Yoyo));


        sequence
            .Append(gameover_bg.DOFade(1f, 1f))
            .Append(DOVirtual.Float(0f, 20f, 0.7f, (value) =>
            {
                lines[0].SetPosition(1, new Vector3(-10f + value, 4f, 1f));
                lines[1].SetPosition(1, new Vector3(10f - value, -4f, 1f));
            }))
            .Append(DOVirtual.DelayedCall(0, () => { gameover_nonactive.SetActive(true); }))
            .Append(DOVirtual.Float(0f, score, 3f, (value) =>
            {
                gameover_scoreText.text = ((int)value).ToString();
            }).SetEase(Ease.OutQuint))
            .Join(t_sequence)
            .Append(DOVirtual.DelayedCall(0, () => 
            {
                if (is_highscore) Highscore_Text.gameObject.SetActive(true);
            }));
        if (is_highscore) sequence.Join(h_sequence);

        sequence
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play();


        // ハイスコアの更新
        GameManager.instance.save_score((long)score);
        UnityroomApiClient.Instance.SendScore(1, score, ScoreboardWriteMode.HighScoreDesc);
    }
}
