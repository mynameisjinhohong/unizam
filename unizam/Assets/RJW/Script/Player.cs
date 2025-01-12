using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Image fadeImage;

    public Vector2 inputVec;
    public float speed;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator animator;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.moveAble == true) 
        {

            // 이동 벡터 계산
            Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;

            // 이동 후 예상 위치 계산
            Vector2 targetPosition = rigid.position + nextVec;

            // x 좌표를 범위 내로 제한
            targetPosition.x = Mathf.Clamp(targetPosition.x, -4.71f, 11219f);

            // 이동
            rigid.MovePosition(targetPosition);


            // 애니메이터 속도 설정
            animator.SetFloat("speed", Mathf.Abs(inputVec.x));

        }
    }


    private void LateUpdate()
    {
        if (inputVec.x != 0 && GameManager.Instance.moveAble == true)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }
}
