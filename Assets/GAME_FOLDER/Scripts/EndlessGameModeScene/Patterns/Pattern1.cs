using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pattern1 : MonoBehaviour
{
    [SerializeField] GameObject[] pats;
    LineRenderer[] line;

    [SerializeField] float rotate_angle = 15f;
    [SerializeField] float rotate_time = 1f;
    [SerializeField] float base_scale = 1.5f;
    [SerializeField] float move_speed = 20f;
    [SerializeField] float gen_time = 2f;

    private void Awake()
    {
        line = new LineRenderer[4];
        for(int i = 0; i < 4; i++)
        {
            line[i] = pats[i].GetComponent<LineRenderer>();
            line[i].startWidth = 0.02f;
            line[i].endWidth = 0.02f;
            //line[i].startColor = Color.red;
            //line[i].endColor = Color.red;
            pats[i].transform.localScale = Vector3.one * base_scale;
        }
        init();
    }

    private void OnEnable()
    {
        init();
    }

    private void init()
    {
        for (int i = 0; i < 4; i++)
        {
            pats[i].transform.localScale = Vector3.one * base_scale;
            pats[i].GetComponent<BoxCollider2D>().enabled = false;
        }
        float x = Random.Range(-8f, 8f);
        float y = Random.Range(-4f, 4f);
        for (int i = 0; i < 4; i++) pats[i].transform.position = new Vector2(x, y);

        Animate_1();
    }

    private void Update()
    {
        make_line();
    }

    private void make_line()
    {
        for(int i = 0; i < 4; i++)
        {
            line[i].SetPosition(0, pats[i].transform.position);
            Vector3 pos = pats[i].transform.position + pats[i].transform.up * 20f;
            line[i].SetPosition(1, pos);
        }
    }

    private void Animate_1()
    {
        var sequence = DOTween.Sequence();

        for(int i = 0; i < 4; i++)
        {
            sequence
                .Join(pats[i].transform.DORotate(new Vector3(0f, 0f, -rotate_angle), rotate_time)
                            .SetRelative())
                .Join(pats[i].transform.DOScale(Vector3.one, rotate_time));
        }

        sequence
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play()
            .OnComplete(Animate_2);
    }

    private void Animate_2()
    {
        for (int i = 0; i < 4; i++)
        {
            pats[i].GetComponent<BoxCollider2D>().enabled = true;
        }

        var sequence = DOTween.Sequence();
        for(int i = 0; i < 4; i++)
        {
            sequence
                .Join(pats[i].GetComponent<Rigidbody2D>().DOMove(pats[i].transform.up * move_speed, 1f)
                            .SetRelative()
                            .SetEase(Ease.Linear));
        }

        sequence
            .Append(DOVirtual.DelayedCall(gen_time, () => { init(); }));

        sequence
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play();
    }
}
