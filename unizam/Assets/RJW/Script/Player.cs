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

            // �̵� ���� ���
            Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;

            // �̵� �� ���� ��ġ ���
            Vector2 targetPosition = rigid.position + nextVec;

            // x ��ǥ�� ���� ���� ����
            targetPosition.x = Mathf.Clamp(targetPosition.x, -4.71f, 11219f);

            // �̵�
            rigid.MovePosition(targetPosition);


            // �ִϸ����� �ӵ� ����
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
