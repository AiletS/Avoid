using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Pattern3 : MonoBehaviour
{
    Material material;
    [SerializeField] private float end_radius;
    [SerializeField] private float time;
    [SerializeField] private PatternData patternData;

    private void Awake()
    {
        material = GetComponent<Image>().material;
    }

    private void Update()
    {
        Vector2 pos = -PlayerMove.instance.transform.position;
        pos.x /= 19.2f;
        pos.y /= 10.8f;
        material.SetVector("_pos", pos);
    }

    private void OnEnable()
    {
        init();
    }

    private void init()
    {
        material.SetFloat("_raduis", 3f);
        Animate_1();
    }

    private void Animate_1()
    {
        var sequence = DOTween.Sequence();
        sequence
            .Append(DOVirtual.Float(3f, 1f, 1f, (value) =>
            {
                material.SetFloat("_radius", value);
            }))
            .Append(DOVirtual.Float(1f, end_radius, time, (value) =>
            {
                material.SetFloat("_radius", value);
            }))
            .AppendInterval(patternData.PatternTimes[3] - time - 2f)
            .Append(DOVirtual.Float(end_radius, 3f, 1f, (value) =>
            {
                material.SetFloat("_radius", value);
            }));

        sequence
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play();
    }
}
