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
        float endY = -600f; // ��ǥ y ��
        float moveGap = 50f; // �̵� �Ÿ� (1�����Ӵ� �̵��� ��)
        float waitTime = 0.04f; // �̵� ���� (��)

        Vector3 currentPos = monster.position;

        // ��ǥ y ���� ������ ������ �̵�
        while (currentPos.y > endY)
        {
            currentPos.y -= moveGap;

            // ��ǥ y ���� �ʰ��Ͽ� �������� �ʵ��� ����
            if (currentPos.y < endY)
            {
                currentPos.y = endY;
            }

            monster.position = currentPos;

            // �̵� ���� ��ٸ���
            yield return new WaitForSeconds(waitTime);
        }

        // ��Ȯ�� ��ǥ y ������ ����
        currentPos.y = endY;
        monster.position = currentPos;

        // 2�� ��� �� �� ��ȯ
        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene("BattleScene");
    }
}
