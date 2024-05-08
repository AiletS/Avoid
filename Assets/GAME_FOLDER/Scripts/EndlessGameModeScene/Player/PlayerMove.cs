using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerMove : MonoBehaviour
{
    static public PlayerMove instance;
    [SerializeField] Camera cam;
    [SerializeField] PatternData patternData;

    // プレイヤー移動変数
    public float dx = 0, dy = 0;
    [SerializeField] float player_speed = 10f;
    private int[,] dir = new int[, ]{ { 135, 90, 45 },{ 180, 0, 0 },{ 225, 270, 315 } };

    // プレイヤー被弾
    private int pattern_damage = 33;
    public int heal_damage = 25;
    private bool invincible = false;
    public float invincible_time = 3f;

    // 被弾カメラ操作
    public float camera_shakeTIme = 1f;
    public float camera_shakeStrength = 1f;

    // ポストエフェクト
    [SerializeField] private Volume postFXVolume;
    private ColorAdjustments coloradj;

    // アニメーション
    //public Animator anim = null;
    //SpriteRenderer SR;

    private void Awake()
    {
        instance = this;
        invincible = false;
        //anim = GetComponent<Animator>();
        //SR = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        postFXVolume.profile.TryGet(out coloradj);
        pattern_damage = patternData.damage;
    }

    private void Update()
    {
        dx = Input.GetAxisRaw("Horizontal");
        dy = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(KeyCode.LeftShift)) player_speed = 5f;
        else player_speed = 10f;

        //if (dx > 0) SR.flipX = true;
        //else if (dx < 0) SR.flipX = false;

        //if (Mathf.Abs(dx) > 0 || Mathf.Abs(dy) > 0) anim.SetBool("IsMove", true);
        //else anim.SetBool("IsMove", false);

        //anim.SetFloat("VerticalVel", dy);
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector3(dx, dy, 0).normalized * player_speed * Time.fixedDeltaTime, Space.World);
        transform.position = new Vector3
            (Mathf.Clamp(transform.position.x, -8.5f, 8.5f)
            , Mathf.Clamp(transform.position.y, -4.5f, 4.5f), 0);

        Vector3 rot = new Vector3(0, 0, dir[(int)dx + 1, (int)dy + 1]);
        transform.localEulerAngles = rot;
    }

    // 動作しなかったら OnTriggerEnter2D に変える
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Pattern")) return;
        if (invincible) return;
        invincible = true;
        UIManager.instance.get_Damage(pattern_damage);

        var sequence = DOTween.Sequence();
        sequence
            .Append(cam.DOShakePosition(camera_shakeTIme, camera_shakeStrength, 10, 30))
            .Join(GetComponent<SpriteRenderer>().DOColor(new Color(1f, 1f, 1f, 0.5f), 0.5f)
                    .SetLoops(6, LoopType.Yoyo))
            .Join(DOVirtual.Float(0, -100, 1.5f, (value) => { coloradj.saturation.value = value; })
                    .SetLoops(2, LoopType.Yoyo))
            .Join(DOVirtual.DelayedCall(invincible_time, () => 
            { invincible = false; }, false));

        var audio_sequence = DOTween.Sequence();
        audio_sequence
            .Append(GameManager.instance.audiosource.DOPitch(0.5f, 1.5f))
            .Append(GameManager.instance.audiosource.DOPitch(1f, 1.5f))
            .SetLink(gameObject, LinkBehaviour.KillOnDisable);
        sequence.Join(audio_sequence);

        sequence
            .SetLink(gameObject, LinkBehaviour.KillOnDisable)
            .Play();
    }
}
