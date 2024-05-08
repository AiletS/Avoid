using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class OpeningScene_UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] MenuTexts;
    [SerializeField] TextMeshProUGUI ModeExpl;
    [SerializeField] Image TextBox;
    [SerializeField] private AudioSource audiosource;
    [SerializeField] private AudioClip audioclip;

    Color text_color = new Color(0f, 188f / 255f, 1f, 1f);

    private int index = 0;
    private bool is_select = false;
    SaveData data;

    private void Awake()
    {
        index = 0;
        change_menu(0);
    }

    private void Start()
    {
        data = GetComponent<SaveLoadManager>().data;
        audiosource.volume = data.SEVolume;
    }

    private void Update()
    {
        // メニューの方向キーでの移動と選択
        if (Input.GetKeyDown(KeyCode.DownArrow) && index < MenuTexts.Length) change_menu(1);
        else if (Input.GetKeyDown(KeyCode.UpArrow) && index > 0) change_menu(-1);
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) select_menu_anim();
    }

    private void change_menu(int ud) // メニュー移動アニメーション
    {
        if (is_select == true) return;

        // index 番号調節
        index += ud;
        if (index < 0) index = 0;
        if (index >= MenuTexts.Length) index = MenuTexts.Length - 1;

        // アニメーション
        var sequence = DOTween.Sequence();
        for (int i = 0; i < MenuTexts.Length; i++)
        {
            int d = index - i;
            Vector2 pos = new Vector2(100f, d * 150f);
            float alpha = (255f - Mathf.Abs(d) * 80) / 255f;
            sequence.Join(MenuTexts[i].GetComponent<RectTransform>().DOAnchorPos(pos, 0.2f)
                                .SetEase(Ease.Linear));
            if (d == 0) sequence.Join(MenuTexts[i].DOColor(text_color, 0.2f));
            else sequence.Join(MenuTexts[i].DOColor(new Color(1f, 1f, 1f, alpha), 0.2f));
        }

        sequence.Play();
    }

    private void select_menu_anim() // メニュー選択アニメーション
    {
        is_select = true;
        var sequence = DOTween.Sequence();
        sequence
            .Append(TextBox.DOFillAmount(1f, 0.25f).SetEase(Ease.OutCubic))
            .Join(MenuTexts[index].DOColor(Color.white, 0.25f))
            .AppendInterval(0.3f)
            .AppendInterval(0.1f);
        //.Append(DOVirtual.DelayedCall(0.5f, () => { load_scene(); }));

        for (int i = 0; i < MenuTexts.Length; i++)
            sequence.Join(MenuTexts[i].GetComponent<RectTransform>().DOAnchorPosX(-800f, 0.2f));
        sequence.Join(TextBox.GetComponent<RectTransform>().DOAnchorPosX(-800f, 0.2f));
        sequence.Join(ModeExpl.GetComponent<RectTransform>().DOAnchorPosX(900f, 0.2f));
        sequence.Append(DOVirtual.DelayedCall(0f, () => { select_menu(); }));

        audiosource.PlayOneShot(audioclip);
        sequence.Play();
    }

    private void select_menu() // メニュー選択条件分岐 (TODO : やる)
    {
        if (index == 0) SceneManager.LoadScene("OpeningScene_EndlessTrial");
        else if(index == 1)
        {
            SceneManager.LoadScene("OpeningScene_Settings");
        }
        else if(index == 2)
        {
            SceneManager.LoadScene("OpeningScene_Profile");
        }
        else if(index == 3)
        {
            SceneManager.LoadScene("OpeningScene_Patterns");
        }
        else if(index == 4)
        {
            SceneManager.LoadScene("OpeningScene_Credit");
        }
        else 
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
        }
    }
}
