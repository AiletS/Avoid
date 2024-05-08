using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pattern2 : MonoBehaviour
{
    [SerializeField] GameObject pat;

    private LineRenderer line;

    public float start_radius = 10f;
    public float line_time = 1f;
    public float wait_movetime = 1f;
    public float move_speed = 5f;
    public float gen_time = 2f;


    private float start_angle, end_angle;
    Vector3 start_linepos, end_linepos;

    private void Awake()
    {
        line = pat.GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        init();
    }

    private void init()
    {
        start_angle = Random.Range(0f, 360f);
        end_angle = start_angle + 180f + Random.Range(-25f, 25f);
        start_angle = start_angle * Mathf.PI / 180f;
        end_angle = end_angle * Mathf.PI / 180f;

        start_linepos = new Vector2(Mathf.Cos(start_angle), Mathf.Sin(start_angle)) * start_radius;
        end_linepos = new Vector2(Mathf.Cos(end_angle), Mathf.Sin(end_angle)) * start_radius;
        pat.transform.position = start_linepos;
        pat.transform.rotation = Quaternion.FromToRotation(Vector3.up, end_linepos - start_linepos);

        Animate_1();
    }

    private void make_line(float dist)
    {
        line.SetPosition(0, pat.transform.position);
        Vector3 pos = pat.transform.position + (end_linepos - pat.transform.position) * dist;
        line.SetPosition(1, pos);
    }

    private void Animate_1()
    {
        var sequence = DOTween.Sequence();

        sequence
            .Append(DOVirtual.Float(0f, 1f, line_time, (value) => { make_line(value); }));

        sequence
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play()
            .OnComplete(Animate_2);
    }

    private void Animate_2()
    {
        var sequence = DOTween.Sequence();

        float dist = Vector3.Distance(end_linepos, start_linepos);
        float move_time = dist / move_speed;
        sequence
            .PrependInterval(wait_movetime)
            .Append(pat.transform.DOMove(pat.transform.up * dist, move_time)
                                    .SetRelative()
                                    .SetEase(Ease.Linear))
            .Join(DOVirtual.Float(0f, 0f, 0f, (value) => { make_line(0); } ))
            .Append(DOVirtual.DelayedCall(gen_time, () => { init(); }));

        sequence
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play();
    }
}
