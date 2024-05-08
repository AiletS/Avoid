using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pattern8 : MonoBehaviour
{
    [SerializeField] private float line_duration;
    [SerializeField] private float obj_interval;
    [SerializeField] private float pat_interval;
    [SerializeField] private float duration;
    [SerializeField] private Vector3 init_scale;
    [SerializeField] private GameObject pat;

    private LineRenderer line;
    private Vector3 base_scale = new Vector3(0.5f, 0.5f, 1);
    private Color init_color = Color.red;
    private Color base_color = Color.red;

    private List<GameObject> objs = new List<GameObject>();

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        init_color.a = 0f;
        base_color.a = 0.2f;
        line.startWidth = 0.2f;
        line.endWidth = 0.2f;
        line.startColor = init_color;
        line.endColor = init_color;
    }

    private void OnEnable()
    {
        animate();
    }

    private void animate()
    {
        var sequence = DOTween.Sequence();
        // x 位置決定、ラインの位置決定
        float x = Random.Range(-8f, 8f);
        line.SetPosition(0, new Vector3(x, 6f, 0));
        line.SetPosition(1, new Vector3(x, -6f, 0));

        // 前兆のライン生成
        sequence
            .Append(line.DOColor(new Color2(init_color, init_color), new Color2(base_color, base_color), line_duration)
                            .SetLoops(6, LoopType.Yoyo));

        // パターンのライン生成、0.8 間隔に 4.8から -5.6まで 13個生成
        for (int i = 0; i < 13; i++)
        {
            Vector3 pos = new Vector3(x, 4.8f - 0.8f * i, 0);
            sequence
                .Append(DOVirtual.DelayedCall(obj_interval, () =>
                {
                    int id = generate();
                    objs[id].transform.position = pos;
                    objs[id].transform.localScale = init_scale;
                    objs[id].SetActive(true);
                    objs[id].transform.DOScale(base_scale, 0.2f).Play();
                    DOVirtual.DelayedCall(duration, () =>
                    {
                        objs[id].transform.DOScale(new Vector3(0f, 0f, 1f), 0.2f)
                            .Play().OnComplete(() => { objs[id].SetActive(false); });
                    }).SetLink(objs[id], LinkBehaviour.KillOnDisable);
                    
                }));
        }

        // pat_interval 後 繰り返し
        sequence
            .Join(DOVirtual.DelayedCall(pat_interval, () => { animate(); }));

        sequence
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play();
    }

    private int generate()
    {
        int n = objs.Count;
        int id = -1;
        for (int i = 0; i < n; i++)
        {
            if (objs[i].activeSelf == false)
            {
                id = i; break;
            }
        }
        if (id == -1)
        {
            GameObject obj = Instantiate(pat);
            obj.SetActive(false);
            objs.Add(obj);
            id = n;
        }
        return id;
    }
}
