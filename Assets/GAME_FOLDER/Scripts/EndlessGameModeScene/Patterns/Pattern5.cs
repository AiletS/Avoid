using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pattern5 : MonoBehaviour
{
    [SerializeField] GameObject pat;

    [SerializeField] float gen_time;
    [SerializeField] float rotate_speed;
    [SerializeField] float move_speed;
    [SerializeField] float pat_gen_time;

    private float now_rotAngle;
    private bool is_gen = false;

    Color init_color, base_color;

    List<GameObject> patterns;

    private void Awake()
    {
        init_color = new Color(1f, 1f, 1f, 0.5f);
        base_color = new Color(1f, 1f, 1f, 1f);
        patterns = new List<GameObject>();
        now_rotAngle = this.transform.localEulerAngles.z;
    }

    private void FixedUpdate()
    {
        if(is_gen)
        {
            now_rotAngle -= rotate_speed * Time.fixedDeltaTime;
            this.transform.localEulerAngles = new Vector3(0f, 0f, now_rotAngle);
        }
    }


    private void OnEnable()
    {
        init();
    }

    private void init()
    {
        //Debug.Log("Pattern 5 enabled");
        is_gen = false;
        GetComponent<SpriteRenderer>().color = init_color;
        float x = Random.Range(-8f, 8f);
        float y = Random.Range(-4f, 4f);
        this.transform.position = new Vector2(x, y);
        this.transform.localEulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));
        now_rotAngle = this.transform.localEulerAngles.z;
        GetComponent<PolygonCollider2D>().enabled = false;
        Animate_0();
    }

    private void Animate_0()
    {
        DOVirtual.DelayedCall(gen_time,
            () => {
                GetComponent<SpriteRenderer>().color = base_color;
                GetComponent<PolygonCollider2D>().enabled = true;
                is_gen = true;
                Animate_1();
            })
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play();
    }

    private void Animate_1()
    {
        int id = generate_pat();
        float x = Random.Range(-8f, 8f);
        float y = Random.Range(-4f, 4f);
        patterns[id].transform.position = this.transform.position + this.transform.up * 0.8f;
        patterns[id].transform.localEulerAngles = this.transform.localEulerAngles;
        patterns[id].SetActive(true);

        patterns[id].transform.DOMove(patterns[id].transform.up * move_speed, 1f)
                            .SetRelative().SetEase(Ease.Linear)
                            .SetLoops(30, LoopType.Incremental)
                            .SetLink(patterns[id].gameObject, LinkBehaviour.KillOnDisable)
                            .OnComplete(() => remove_pat(id))
                            .Play();

        var sequence = DOTween.Sequence();
        sequence
            .Append(DOVirtual.DelayedCall(pat_gen_time, () => Animate_1())
                        .SetLink(gameObject, LinkBehaviour.KillOnDisable));

        sequence
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play();
    }

    private int generate_pat()
    {
        int n = patterns.Count;
        int id = -1;
        for(int i = 0; i < n; i++)
        {
            if(patterns[i].activeSelf == false)
            {
                id = i; break;
            }
        }
        if(id == -1)
        {
            GameObject obj = Instantiate(pat);
            patterns.Add(obj);
            id = n;
        }
        return id;
    }

    private void remove_pat(int id)
    {
        patterns[id].SetActive(false);
    }
}
