using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Encounter : MonoBehaviour
{
    public Image fadeImage; // ���̵� ȿ�� �̹���
    public List<Sprite> changeImages; // ������ ��������Ʈ ���

    void Start()
    {
    }

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
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
        if (monster != null)
        {
            monster.gameObject.SetActive(true);

            Image monsterImage = monster.GetComponent<Image>();
            if (monsterImage != null && changeImages.Count > 0)
            {
                foreach (Sprite sprite in changeImages)
                {
                    monsterImage.sprite = sprite;
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MoveSequence(monster));
    }

    IEnumerator MoveSequence(Transform monster)
    {
        if (monster == null) yield break;

        // �Ʒ��� ����
        yield return StartCoroutine(MoveDown(monster));

        // 1�� ���
        yield return new WaitForSeconds(1.0f);

        // ���� ��ġ�� �ø�
        yield return StartCoroutine(MoveUp(monster));

        // 1�� ���
        yield return new WaitForSeconds(1.0f);

        // �ٽ� �Ʒ��� ����
        yield return StartCoroutine(MoveDown(monster));

        yield return new WaitForSeconds(1.0f);

        // Y�� ���� �Ϸ� �� ���� ���� ����
        Image monsterImage = monster.GetComponent<Image>();
        if (monsterImage != null)
        {
            yield return StartCoroutine(FadeToBlack(monsterImage));
        }
    }

    IEnumerator MoveDown(Transform monster)
    {
        float endY = -6920f; // ��ǥ y ��
        float moveGap = 30f; // �̵� �Ÿ� (1�����Ӵ� �̵��� ��) - �� ������ �̵�
        float waitTime = 0.015f; // �̵� ���� (��)

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

            yield return new WaitForSeconds(waitTime);
        }

        currentPos.y = endY;
        monster.position = currentPos;
    }

    IEnumerator MoveUp(Transform monster)
    {
        float startY = -721.0f; // ���� ��ġ
        float moveGap = 30f; // �̵� �Ÿ� (1�����Ӵ� �̵��� ��) - �� ������ �̵�
        float waitTime = 0.015f; // �̵� ���� (��)

        Vector3 currentPos = monster.position;

        // ���� ��ġ�� ���ư� ������ �̵�
        while (currentPos.y < startY)
        {
            currentPos.y += moveGap;

            // ���� ��ġ�� �ʰ����� �ʵ��� ����
            if (currentPos.y > startY)
            {
                currentPos.y = startY;
            }

            monster.position = currentPos;

            yield return new WaitForSeconds(waitTime);
        }

        currentPos.y = startY;
        monster.position = currentPos;
    }

    IEnumerator FadeToBlack(Image monsterImage)
    {
        Color initialColor = monsterImage.color;
        Color targetColor = Color.black; // ���������� ����

        float transitionTime = 2.0f; // ���� ��ȯ �ð�
        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            monsterImage.color = Color.Lerp(initialColor, targetColor, elapsedTime / transitionTime);
            yield return null;
        }

        monsterImage.color = targetColor;

        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene("BattleScene");
    }
}
