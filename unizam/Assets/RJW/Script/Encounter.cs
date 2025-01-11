using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Encounter : MonoBehaviour
{
    public Image fadeImage; // ���̵� ȿ�� �̹���
    public List<Sprite> changeImages; // ������ ��������Ʈ ���

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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

            Image monsterImage = monster.GetComponent<Image>(); // Image ������Ʈ ��������
            if (monsterImage != null && changeImages.Count > 0)
            {
                // changeImages ����� ��������Ʈ��� ���������� ����
                //StartCoroutine(ChangeMonsterImages(monsterImage));
                foreach (Sprite sprite in changeImages)
                {
                    monsterImage.sprite = sprite; // Image ������Ʈ�� ��������Ʈ ����
                    yield return new WaitForSeconds(0.5f); // 0.5�� ���
                }
            }
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MoveMonster(monster));
    }

    IEnumerator ChangeMonsterImages(Image monsterImage)
    {
        foreach (Sprite sprite in changeImages)
        {
            monsterImage.sprite = sprite; // Image ������Ʈ�� ��������Ʈ ����
            yield return new WaitForSeconds(1.0f); // 0.5�� ���
        }
    }

    IEnumerator MoveMonster(Transform monster)
    {
        float endY = -6920f; // ��ǥ y ��
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
