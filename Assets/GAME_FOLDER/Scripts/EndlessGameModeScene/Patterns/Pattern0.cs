using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pattern0 : MonoBehaviour
{
    LineRenderer line;

    [SerializeField] float gen_time = 1.5f;
    [SerializeField] float rotate_angle = 360f;
    [SerializeField] float rotate_time = 2.5f;
    [SerializeField] float move_time = 1f;
    private float move_rotate = 360f;

    Color init_color, base_color;

    private void Awake()
    {
        init_color = new Color(1f, 1f, 1f, 0.5f);
        base_color = new Color(1f, 1f, 1f, 1f);
    }

    private void OnEnable()
    {
        init();
        Animate_0();
    }

    private void init()
    {
        line = GetComponent<LineRenderer>();
        line.startWidth = 0.02f;
        line.endWidth = 0.02f;
        //line.startColor = Color.red;
        //line.endColor = Color.red;
        GetComponent<SpriteRenderer>().color = init_color;
        float x = Random.Range(-8f, 8f);
        float y = Random.Range(-4f, 4f);
        this.transform.position = new Vector2(x, y);
        this.transform.DORotate(new Vector3(0f, 0f, Random.Range(0f, 360f)), 0f).Play();
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void make_line(float dist)
    {
        line.SetPosition(0, this.transform.position);
        Vector3 pos = this.transform.position + (PlayerMove.instance.transform.position - this.transform.position) * dist;
        line.SetPosition(1, pos);
    }

    private void Animate_0()
    {
        DOVirtual.DelayedCall(gen_time,
            () => { 
                GetComponent<SpriteRenderer>().color = base_color;
                GetComponent<BoxCollider2D>().enabled = true;
                Animate_1(); 
            })
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play();   
    }

    private void Animate_1()
    {
        int inv = UnityEngine.Random.Range(0f, 1f) >= 0.5f ? 1 : -1;
        var sequence = DOTween.Sequence();

        sequence
            .Append(transform.DORotate(new Vector3(0f, 0f, rotate_angle * inv), rotate_time)
                        .SetRelative()
                        .SetEase(Ease.InOutBack))
            .Join(DOVirtual.Float(0, 1f, rotate_time, (value) => { make_line(value); }));


        sequence
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play()
            .OnComplete(Animate_2);
            
    }

    private void Animate_2()
    {
        int inv = UnityEngine.Random.Range(0f, 1f) >= 0.5f ? 1 : -1;
        var sequence = DOTween.Sequence();
        Vector3 player_pos = PlayerMove.instance.transform.position;
        move_rotate = UnityEngine.Random.Range(180f, 360f);

        sequence
            .Append(transform.DOMove(player_pos, move_time).SetEase(Ease.OutCubic))
            .Join(transform.DORotate(new Vector3(0f, 0f, move_rotate * inv), move_time)
                        .SetRelative()
                        .SetEase(Ease.OutCubic));

        make_line(0);
        sequence
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play()
            .OnComplete(Animate_1);
    }
}
