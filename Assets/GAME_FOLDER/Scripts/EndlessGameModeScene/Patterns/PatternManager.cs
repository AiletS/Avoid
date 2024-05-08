using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PatternManager : MonoBehaviour
{
    static public PatternManager instance;

    [SerializeField] GameObject[] Patterns;
    [SerializeField] GameObject[] TrialPatterns;
    [SerializeField] private PatternData patternData;

    private int num_Pattern;
    private int[] isplaying_Pattern;
    private int num_playingPattern;
    private bool is_generating = false;

    public bool is_Gameover = false;

    private float now_time = 0f;

    SaveData data;

    private void Awake()
    {
        instance = this;
        num_Pattern = Patterns.Length;
        isplaying_Pattern = new int[num_Pattern];
        for (int i = 0; i < num_Pattern; i++) isplaying_Pattern[i] = 0;
        num_playingPattern = 0;
        is_generating = false;
        patternData.level = 0;
    }

    private void Start()
    {
        data = GameManager.instance.data;
    }

    private void Update()
    {
        now_time += Time.deltaTime;
        if(now_time >= patternData.LevelTimes[patternData.level])
        {
            level_up();
        }
    }

    private void FixedUpdate()
    {
        if (is_Gameover) return;
        check_numPattern();
    }

    private void check_numPattern()
    {
        if (is_generating) return;
        if (num_playingPattern >= patternData.level) return;
        is_generating = true;
        int id = Random.Range(0, num_Pattern);
        while (isplaying_Pattern[id] == 1) id = (id + 1) % num_Pattern;
        DOVirtual.DelayedCall(0.5f, () => { generate_Pattern(id); is_generating = false; });
    }

    private void generate_Pattern(int id)
    {
        num_playingPattern++;
        isplaying_Pattern[id] = 1;
        UIManager.instance.add_pattern(id);

        var sequence = DOTween.Sequence();
        sequence
            .Append(DOVirtual.DelayedCall(0, () =>
            {
                if (data.TrialList[id] == 1) TrialPatterns[id].SetActive(true);
                else Patterns[id].SetActive(true);
            }))
            .Append(DOVirtual.DelayedCall(patternData.PatternTimes[id], () =>
            {
                remove_Pattern(id);
            }));
        sequence
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play();
    }

    public void remove_Pattern(int id)
    {
        if (data.TrialList[id] == 1) TrialPatterns[id].SetActive(false);
        else Patterns[id].SetActive(false);
        num_playingPattern--;
        isplaying_Pattern[id] = 0;
        UIManager.instance.remove_pattern(id);
    }

    public void Gameover()
    {
        is_Gameover = true;
        for(int i = 0; i < num_Pattern; i++) if(isplaying_Pattern[i] == 1)
            {
                remove_Pattern(i);
            }
    }

    // パターン数UI関連
    [SerializeField] private TextMeshProUGUI pattern_numText;
    private void level_up()
    {
        patternData.level++;
        now_time = 0;

        RectTransform pos = pattern_numText.GetComponent<RectTransform>();
        string text = patternData.level.ToString() + "  Patterns";
        var sequence = DOTween.Sequence();

        sequence
            .Append(pattern_numText.DOFade(0f, 0f))
            .Join(pattern_numText.DOText(text, 0f, false))
            .Join(pos.DOAnchorPosX(200f, 0f))
            .Append(pos.DOAnchorPosX(0f, 0.3f))
            .Join(pattern_numText.DOFade(1f, 0.3f))
            .AppendInterval(1f)
            .Append(pos.DOAnchorPosX(-200f, 0.3f))
            .Join(pattern_numText.DOFade(0f, 0.3f));

        sequence
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play();
    }
}
