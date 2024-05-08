using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pattern6 : MonoBehaviour
{
    [SerializeField] GameObject[] pats;

    [SerializeField] float move_speed;
    [SerializeField] float amplitude;
    [SerializeField] float gen_time;

    Color init_color, base_color;

    private void Awake()
    {
        init_color = new Color(1f, 1f, 1f, 0.08f);
        base_color = new Color(1f, 1f, 1f, 1f);
    }


    private void OnEnable()
    {
        init();
    }

    private void init()
    {
        for (int i = 0; i < 6; i++)
        {
            pats[i].GetComponent<SpriteRenderer>().color = init_color;
            pats[i].GetComponent<PolygonCollider2D>().enabled = false;
            pats[i].transform.position = Vector3.zero;
        }
        Animate_0();
    }

    private void Animate_0()
    {
        DOVirtual.DelayedCall(gen_time,
            () => {
                for(int i = 0; i < 6; i++)
                {
                    pats[i].GetComponent<SpriteRenderer>().color = base_color;
                    pats[i].GetComponent<PolygonCollider2D>().enabled = true;
                }
                Animate_1();
            })
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play();
    }

    private void Animate_1()
    {
        DOVirtual.Float(0f, 180f, move_speed, (value) =>
        {
            for (int i = 0; i < 6; i++)
            {
                Vector3 n_pos = pats[i].transform.up * amplitude * Mathf.Sin(value * Mathf.PI / 180f);
                pats[i].transform.position = n_pos;
            }
        }).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Incremental)
          .SetLink(gameObject, LinkBehaviour.KillOnDisable).Play();
    }
}
