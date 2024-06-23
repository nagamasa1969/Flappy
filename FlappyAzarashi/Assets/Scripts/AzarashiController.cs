using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AzarashiController : MonoBehaviour
{
    Rigidbody2D rb2d;
    Animator animator;
    float angle;
    bool isDead;

    public float maxHeight;
    public float flapVelocity;
    public float relativeVelocityX;
    public GameObject sprite;

    public bool IsDead()
    {
        return isDead;
    }



    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = sprite.GetComponent<Animator>();
    }

    private void Update()
    {
        // 最高高度に達していない限りタップの入力を受け付ける
        if(Input.GetButton("Fire1") && transform.position.y < maxHeight)
        {
            Flap();
        }

        // 角度を反映
        ApplyAngle();

        // アングルが水平以上だったら、アニメーターのflapフラグをtrueにする。
        animator.SetBool("flap", angle >= 0.0f && !isDead);
    }

    public void Flap()
    {
        // 死んだらはばたけない
        if (isDead) return;

        // 重力が効いていないときは操作しない
        if (rb2d.isKinematic) return;

        // Velocityを直接置き換えて上方向に上昇
        rb2d.velocity = new Vector2(0.0f, flapVelocity);
    }

    void ApplyAngle()
    {
        // 現在の速度、相対速度から進んでいいる角度を求める
        float targetAngle;
        if (isDead)
        {
            targetAngle = 180.0f;
        }
        else
        {
            targetAngle = Mathf.Atan2(rb2d.velocity.y, relativeVelocityX) * Mathf.Rad2Deg;
        }
        

        // 回転アニメをスムージング
        angle = Mathf.Lerp(angle, targetAngle, Time.deltaTime * 10.0f);

        // Rotationの反映
        sprite.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        // クラッシュエフェクト
        Camera.main.SendMessage("Clash");

        // 何かにぶつかったら死亡フラグを立てる
        isDead = true;
    }

    public void SetSteerActive(bool active)
    {
        // Rigidbodyのオン、オフを切り替える
        rb2d.isKinematic = !active;
    }
}
