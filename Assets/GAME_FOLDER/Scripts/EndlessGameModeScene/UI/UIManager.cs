using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UIManager : MonoBehaviour
{
    static public UIManager instance;

    // hpバー関連
    [SerializeField] Image HPBar_Left;
    [SerializeField] Image HPBar_Right;
    float hp_animateTime = 1f;
    int now_hp = 100;

    // パターンUI関連
    [SerializeField] private SpriteData spriteData;
    [SerializeField] Image sample_patternUI;
    [SerializeField] Canvas canvas;
    List<Image> living_patterns;
    private float move_time = 0.5f;

    // ポーズ関連
    [SerializeField] Image PauseBG;
    bool is_pause;

    SaveData data;

    private void Awake()
    {
        instance = this;
        living_patterns = new List<Image>();
        is_pause = false;
    }

    private void Start()
    {
        data = GameManager.instance.data;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PatternManager.instance.is_Gameover == false)
        {
            pause();
        }
    }

    public void get_Damage(int damage)
    {
        int next_hp = now_hp - damage;
        next_hp = Mathf.Max(next_hp, 0);
        next_hp = Mathf.Min(next_hp, 100);
        now_hp = next_hp;
        var sequence = DOTween.Sequence();
        sequence
            .Append(HPBar_Left.DOFillAmount((float)next_hp / 100, hp_animateTime))
            .Join(HPBar_Right.DOFillAmount((float)next_hp / 100, hp_animateTime));
        if(next_hp > 50)
        {
            sequence.Join(HPBar_Left.DOColor(Color.green, hp_animateTime));
            sequence.Join(HPBar_Right.DOColor(Color.green, hp_animateTime));
        }
        else if (next_hp <= 50 && next_hp >= 20)
        {
            sequence.Join(HPBar_Left.DOColor(Color.yellow, hp_animateTime));
            sequence.Join(HPBar_Right.DOColor(Color.yellow, hp_animateTime));
        }
        else if(next_hp < 20)
        {
            sequence.Join(HPBar_Left.DOColor(Color.red, hp_animateTime));
            sequence.Join(HPBar_Right.DOColor(Color.red, hp_animateTime));
        }

        sequence
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play();

        if(now_hp == 0)
        {
            GetComponent<GameoverManager>().Gameover();
        }
    }

    public void add_pattern(int id)
    {
        Image img = Instantiate(sample_patternUI, canvas.transform);

        string t = "Pattern" + id.ToString();
        TRIAL trial = (TRIAL)Enum.Parse(typeof(TRIAL), t);
        int trial_id = (int)trial;

        img.sprite = spriteData.TrialSprites[id * 2 + data.TrialList[trial_id]];
        Vector3 pos = img.GetComponent<RectTransform>().anchoredPosition;
        pos.x = 50 * living_patterns.Count + 400;
        img.GetComponent<RectTransform>().anchoredPosition = pos;
        living_patterns.Add(img);
        move_patternUI();
    }

    public void remove_pattern(int id)
    {
        int pattern_num = living_patterns.Count;
        string t = "Pattern" + id.ToString();
        TRIAL trial = (TRIAL)Enum.Parse(typeof(TRIAL), t);
        int trial_id = (int)trial;
        for (int i = 0; i < pattern_num; i++)
        {
            if(living_patterns[i].sprite == spriteData.TrialSprites[id * 2 + data.TrialList[trial_id]])
            {
                Destroy(living_patterns[i]);
                living_patterns.RemoveAt(i);
                break;
            }
        }
        move_patternUI();
    }

    private void move_patternUI()
    {
        int pattern_num = living_patterns.Count;
        if (pattern_num == 0) return;
        float left = 50;
        float space = 100;

        var sequence = DOTween.Sequence();

        for(int i = 0; i < pattern_num; i++)
        {
            float pos = -left * (pattern_num - 1) + space * i;
            sequence
                .Join(living_patterns[i].GetComponent<RectTransform>().DOAnchorPosX(pos, move_time));
        }

        sequence
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play();
    }

    private void pause()
    {
        if(is_pause == false)
        {
            is_pause = true;
            PauseBG.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            is_pause = false;
            PauseBG.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
