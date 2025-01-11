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
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

        animator.SetFloat("speed", Mathf.Abs(inputVec.x));
    }

    private void LateUpdate()
    {
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade()
    {
        float fadeCount = 0;
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            fadeImage.color = new Color(0, 0, 0, fadeCount);
        }

        Transform monster = fadeImage.transform.Find("downToUP");

        monster.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MoveMonster(monster));
    }

    IEnumerator MoveMonster(Transform monster)
    {
        float endY = -600f; // 목표 y 값
        float moveGap = 50f; // 이동 거리 (1프레임당 이동할 값)
        float waitTime = 0.04f; // 이동 간격 (초)

        Vector3 currentPos = monster.position;

        // 목표 y 값에 도달할 때까지 이동
        while (currentPos.y > endY)
        {
            currentPos.y -= moveGap;

            // 목표 y 값을 초과하여 내려가지 않도록 제한
            if (currentPos.y < endY)
            {
                currentPos.y = endY;
            }

            monster.position = currentPos;

            // 이동 간격 기다리기
            yield return new WaitForSeconds(waitTime);
        }

        // 정확히 목표 y 값으로 설정
        currentPos.y = endY;
        monster.position = currentPos;

        // 2초 대기 후 씬 전환
        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene("BattleScene");
    }
}
