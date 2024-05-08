using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pattern9 : MonoBehaviour
{
    [SerializeField] GameObject[] pats;
    [SerializeField] float gen_time;
    [SerializeField] float move_time;

    private void OnEnable()
    {
        for(int i = 0; i < pats.Length; i++)
        {
            pats[i].GetComponent<CircleCollider2D>().enabled = false;
            pats[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        }
        animate();
    }

    private void animate()
    {
        float dist = 8.8f / (pats.Length - 1);
        for(int i = 0; i < pats.Length; i++)
        {
            float x = -8.3f;
            if (pats.Length == 8 && i % 2 == 1) x = 8.3f;
            pats[i].transform.position = new Vector3(x, 4.4f - dist * i);
        }

        var sequence = DOTween.Sequence();

        sequence.Append(DOVirtual.DelayedCall(gen_time, () =>
        {
            for (int i = 0; i < pats.Length; i++)
            {
                pats[i].GetComponent<CircleCollider2D>().enabled = true;
                pats[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            }
        }));

        for(int i = 0; i < pats.Length; i++)
        {
            float x = 16.6f;
            if (pats.Length == 8 && i % 2 == 1) x = -16.6f;
            sequence.Join(pats[i].transform.DOMoveX(x, move_time)
                .SetRelative()
                .SetLoops(100, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad));
            sequence.Join(pats[i].transform.DOMoveY(-0.2f, 0.45f)
                .SetRelative()
                .SetLoops(100, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad));
        }
        sequence
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play();
    }
}
