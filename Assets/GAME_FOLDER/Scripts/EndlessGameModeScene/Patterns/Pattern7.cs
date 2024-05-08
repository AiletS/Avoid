using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pattern7 : MonoBehaviour
{
    LineRenderer line;

    [SerializeField] float init_time = 1.5f;
    [SerializeField] float gen_time = 1.5f;
    [SerializeField] float line_time = 2.5f;
    [SerializeField] float move_time = 2f;
    //[SerializeField] GameObject[] objs;
    GameObject[] objs;
    private Vector3[] objs_pos = new Vector3[7];

    private bool is_rot = false;
    Color init_color, base_color;

    private void Awake()
    {
        init_color = new Color(1f, 1f, 1f, 0.5f);
        base_color = new Color(1f, 1f, 1f, 1f);
        objs_pos[0] = new Vector3(0, 0, 0);
        objs_pos[1] = new Vector3(-0.5f, -0.2f, 0);
        objs_pos[2] = new Vector3(0.5f, -0.2f, 0);
        objs_pos[3] = new Vector3(-1f, -0.4f, 0);
        objs_pos[4] = new Vector3(1f, -0.4f, 0);
        objs_pos[5] = new Vector3(-1.5f, -0.6f, 0);
        objs_pos[6] = new Vector3(1.5f, -0.6f, 0);
        objs = new GameObject[this.transform.childCount];
        for(int i = 0; i < this.transform.childCount; i++)
        {
            objs[i] = this.transform.GetChild(i).gameObject;
        }
    }

    private void OnEnable()
    {
        init();
    }

    private void FixedUpdate()
    {
        if(is_rot)
        {
            Vector3 dif = PlayerMove.instance.transform.position - this.transform.position;
            this.transform.rotation = Quaternion.FromToRotation(Vector3.down, dif);
        }
    }

    private void init()
    {
        line = GetComponent<LineRenderer>();
        line.startWidth = 0.02f;
        line.endWidth = 0.02f;
        float x = Random.Range(-8f, 8f);
        float y = Random.Range(-4f, 4f);
        this.transform.position = new Vector2(x, y);
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].GetComponent<SpriteRenderer>().color = init_color;
            //objs[i].transform.position = objs_pos[i] + this.transform.position;
            objs[i].transform.localPosition = objs_pos[i];
            objs[i].GetComponent<BoxCollider2D>().enabled = false;
        }
        Animate_0();
    }

    private void make_line(float dist)
    {
        line.SetPosition(0, this.transform.position);
        Vector3 pos = this.transform.position + (PlayerMove.instance.transform.position - this.transform.position) * dist;
        line.SetPosition(1, pos);
    }

    private void Animate_0()
    {
        is_rot = true;
        DOVirtual.DelayedCall(init_time,
            () => {
                for (int i = 0; i < objs.Length; i++)
                {
                    objs[i].GetComponent<SpriteRenderer>().color = base_color;
                    objs[i].GetComponent<BoxCollider2D>().enabled = true;
                }
                Animate_1();
            })
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play();
    }

    private void Animate_1()
    {
        var sequence = DOTween.Sequence();
        sequence
            .Append(DOVirtual.Float(0, 1, line_time, (value) => { make_line(value); }).SetEase(Ease.Linear));

        sequence
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play()
            .OnComplete(Animate_2);
    }

    private void Animate_2()
    {
        is_rot = false;
        var sequence = DOTween.Sequence();
        for(int i = 0; i < objs.Length; i++)
        {
            Vector3 pos = objs[i].transform.position - objs[i].transform.up * 20;
            sequence.Join(objs[i].transform.DOMove(pos, move_time).SetEase(Ease.InOutBack));
        }

        sequence
            .Append(DOVirtual.DelayedCall(gen_time, () => { init(); }));

        make_line(0);
        sequence
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play();
    }
}
