using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;

public class OpeningScene_Profile_UIManager : MonoBehaviour
{
    // 説明
    [SerializeField] Image TextBox;
    [SerializeField] TextMeshProUGUI data_text;
    [SerializeField] TextMeshProUGUI ach_text;
    [SerializeField] Image[] Highscore_Trial;
    [SerializeField] private SpriteData spriteData;

    // 実績
    [SerializeField] private Image[] AchImage;
    [SerializeField] private TextMeshProUGUI ach_expl;

    // カーソル
    [SerializeField] private Image cursor;
    private RectTransform cursorpos;
    private int base_x = 90, base_y = -540;
    private int x = 0;
    private int y = 0;
    
    // データ
    [SerializeField] private AchievementData Achdata;

    SaveData data;
    string t_text;

    private void Start()
    {
        init_data();

        var sequence = DOTween.Sequence();
        sequence.Append(TextBox.GetComponent<RectTransform>().DOAnchorPosX(0f, 0.2f));
        sequence.Join(data_text.GetComponent<RectTransform>().DOAnchorPosX(100f, 0.2f));
        sequence.Join(ach_text.GetComponent<RectTransform>().DOAnchorPosX(100f, 0.2f));
        sequence.Append(data_text.DOText(t_text, 1f));
        sequence.Join(ach_text.DOText("実績", 1f));

        sequence.SetLink(gameObject, LinkBehaviour.KillOnDisable).Play();

        DOVirtual.Float(0f, 1f, 1f, (value) =>
        {
            cursor.fillAmount = value;
        }).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject, LinkBehaviour.KillOnDisable).Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) load_scene("OpeningScene_Main");
        if (Input.GetKeyDown(KeyCode.RightArrow)) move_cursor(1, 0);
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) move_cursor(-1, 0);
        else if (Input.GetKeyDown(KeyCode.UpArrow)) move_cursor(0, 1);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) move_cursor(0, -1);
    }

    private void init_data() // SaveDataから復元
    {
        // SaveData読み込み
        data = GetComponent<SaveLoadManager>().data;

        // テキスト設定
        t_text = "プレー回数 : " + data.PlayCount + "\n";
        t_text += "ハイスコア : " + data.HighScore + "\n";
        t_text += "ハイスコア時に使った試練\n";
        for(int i = 0; i < Highscore_Trial.Length; i++)
        {
            Highscore_Trial[i].GetComponent<Image>().sprite
                = spriteData.TrialSprites[i * 2 + data.Highscore_TrialList[i]];
        }

        // 実績
        for(int i = 0; i < AchImage.Length; i++)
        {
            AchImage[i].sprite = Achdata.AchImage[i];
            AchImage[i].material = Achdata.Material_No;
            if (data.AchievementList[i] == 1) AchImage[i].material = Achdata.Material_Yes;
        }

        // カーソル
        x = 0; y = 0;
        cursorpos = cursor.GetComponent<RectTransform>();
    }

    private void move_cursor(int dx, int dy)
    {
        x += dx; y += dy;
        x = Mathf.Clamp(x, 0, 8);
        y = Mathf.Clamp(y, -2, 0);
        cursorpos.DOAnchorPos(new Vector2(base_x + x * 150f, base_y + y * 150f), 0f);
        cursor.GetComponent<RectTransform>().localScale = new Vector2(1f, 1f);
        if(x == 8)
        {
            cursorpos.DOAnchorPos(new Vector2(base_x + x * 150f, base_y), 0f);
            cursor.GetComponent<RectTransform>().localScale = new Vector2(3.5f, 3.5f);
        }

        int id = 0;
        if (x != 8) id = -y * 8 + x;
        else id = 24;
        if (id != 24)
        {
            if (id % 2 == 0) ach_expl.DOText("図の試練を付けて4000点以上達成", 0.5f, true, ScrambleMode.All);
            else ach_expl.DOText("図の試練を付けて8000点以上達成", 0.5f, true, ScrambleMode.All);
        }
        else ach_expl.DOText("???", 0.5f, true, ScrambleMode.All);
    }

    public void load_scene(string scene_name)
    {
        if (scene_name == "OpeningScene_Main")
        {
            SceneManager.LoadScene(scene_name);
            /*
            var sequence = DOTween.Sequence();
            sequence.Append(data_text.GetComponent<RectTransform>().DOAnchorPosX(-1000f, 0.2f));
            sequence.Join(TextBox.GetComponent<RectTransform>().DOAnchorPosX(800f, 0.2f));
            for(int i = 0; i < Highscore_Trial.Length; i++)
            {
                sequence.Join(Highscore_Trial[i].GetComponent<RectTransform>().DOAnchorPosX(-2000f, 0.2f));
            }
            sequence.Append(DOVirtual.DelayedCall(0.3f, () => { SceneManager.LoadScene(scene_name); }));

            sequence
                .SetLink(gameObject, LinkBehaviour.KillOnDisable)
                .Play();
            */
        }
    }
}
