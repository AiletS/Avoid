using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;

public class OpeningScene_EndlessTrial_UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ModeExpl;

    [SerializeField] Image EndlessModeText;

    [SerializeField] Image[] TrialImages;
    [SerializeField] SpriteData spriteData;
    [SerializeField] GameObject TrialImages_obj;
    [SerializeField] TextMeshProUGUI ScoreMultiplyText;

    // 効果音
    [SerializeField] private AudioSource audiosource;
    [SerializeField] private AudioClip audioclip;

    // カーソル
    [SerializeField] private Image cursor;
    private RectTransform cursorpos;
    private int x = 0, y = 0;
    private int base_x = 100, base_y = -100;

    [SerializeField] private PatternData patternData;
    SaveData data;

    private void Start()
    {
        init_data();

        var sequence = DOTween.Sequence();
        sequence.Append(TrialImages_obj.GetComponent<RectTransform>().DOAnchorPosX(0f, 0.2f));
        sequence.Join(EndlessModeText.GetComponent<RectTransform>().DOAnchorPosX(0f, 0.2f));

        sequence.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) load_scene("OpeningScene_Main");
        if (Input.GetKeyDown(KeyCode.RightArrow)) move_cursor(1, 0);
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) move_cursor(-1, 0);
        else if (Input.GetKeyDown(KeyCode.UpArrow)) move_cursor(0, 1);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) move_cursor(0, -1);
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            int id = -y * 4 + x;
            if (id < 12) change_tiral_int(id);
            else
            {
                audiosource.PlayOneShot(audioclip);
                load_scene("EndlessModeScene");
            }
        }
    }

    private void init_data() // SaveDataから復元
    {
        // SaveData読み込み
        data = GetComponent<SaveLoadManager>().data;
        // 復元
        for (int i = 0; i < TrialImages.Length; i++)
        {
            if (data.TrialList[i] != 0 && data.TrialList[i] != 1) data.TrialList[i] = 0;
            TrialImages[i].GetComponent<Image>().sprite
                    = spriteData.TrialSprites[i * 2 + data.TrialList[i]];
        }
        string smtext = "スコア倍率 : " + data.score_multiply.ToString("N2");
        ScoreMultiplyText.DOText(smtext, 0.3f);

        // カーソル
        cursorpos = cursor.GetComponent<RectTransform>();
        DOVirtual.Float(0f, 1f, 1f, (value) =>
        {
            cursor.fillAmount = value;
        }).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject, LinkBehaviour.KillOnDisable).Play();

        // オーディオ
        audiosource.volume = data.SEVolume;
    }

    public void load_scene(string scene_name)
    {
        GetComponent<SaveLoadManager>().Save(data);
        if (scene_name == "EndlessModeScene")
        {
            SceneManager.LoadScene(scene_name);
        }
        else if(scene_name == "OpeningScene_Main")
        {
            var sequence = DOTween.Sequence();
            sequence.Append(TrialImages_obj.GetComponent<RectTransform>().DOAnchorPosX(-1000f, 0.2f));
            sequence.Join(EndlessModeText.GetComponent<RectTransform>().DOAnchorPosX(800f, 0.2f));
            sequence.Append(DOVirtual.DelayedCall(0.3f, () => { SceneManager.LoadScene(scene_name); }));

            sequence.Play();
        }
    }
    private void change_tiral_int(int id)
    {
        audiosource.PlayOneShot(audioclip);
        data.TrialList[id] ^= 1;
        data.score_multiply = 1f;
        for (int i = 0; i < patternData.PatternScoreMul.Length; i++)
        {
            if (data.TrialList[i] == 1) data.score_multiply *= patternData.PatternScoreMul[i];
        }

        string smtext = "スコア倍率 : " + data.score_multiply.ToString("N2");
        ScoreMultiplyText.DOText(smtext, 0.3f);
        TrialImages[id].GetComponent<Image>().sprite = spriteData.TrialSprites[id * 2 + data.TrialList[id]];
    }

    private void move_cursor(int dx, int dy)
    {
        x += dx; y += dy;
        x = Mathf.Clamp(x, 0, 3);
        y = Mathf.Clamp(y, -3, 0);
        cursorpos.DOAnchorPos(new Vector2(base_x + x * 200f, base_y + y * 200f), 0f);
        cursorpos.localScale = new Vector2(1.5f, 1.5f);
        if(y == -3)
        {
            cursorpos.DOAnchorPos(new Vector2(1500,-900), 0f);
            cursorpos.localScale = new Vector2(9f, 1.5f);
        }
    }
}
