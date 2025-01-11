using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum monster
{
    man,
    du,
    gu,
    snake
}

public class Encounter : MonoBehaviour
{
    public bool isVisit = false;

    public List<EnemeyData> enemy;
    public Sprite bg;
    public Behaviour reward;

    public monster monster;

    public Image fadeImage; // ���̵� ȿ�� �̹���
    public List<Sprite> changeImages; // ������ ��������Ʈ ���

    public int clearIndex;

    public AudioClip encounterAudio;
    public AudioClip hintaudioAudio;
    //1 - �ξ� ����, 2- ����ȣ ����, 3- �̹��� ����
    public AudioSource audioPlay;

    void Start()
    {
    }

    void Update()
    {
        if (isVisit == true && Input.GetKeyDown(KeyCode.Space)) {
            switch (monster)
            {
                case monster.du:
                    if (GameManager.Instance.isClear[1] == false)
                        StartCoroutine(duFade());
                    break;
                case monster.gu:
                    if (GameManager.Instance.isClear[2] == false)
                        StartCoroutine(guFade());
                    break;
                case monster.snake:
                    StartCoroutine(snakeFade());
                    break;
                case monster.man:
                    if (GameManager.Instance.isClear[0] == false)
                        StartCoroutine(manFade());
                    break;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("�湮");
            isVisit = true;
            if (monster == monster.snake) {
                StartCoroutine(snakeFade());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("����");
        if (GameManager.Instance.isClear[clearIndex] == false)
        {
            audioPlay.clip = hintaudioAudio;
            audioPlay.Play();
        }
        isVisit = false;
    }

    // du ///////////////////////////////////////////////
    IEnumerator duFade()
    {
        GameManager.Instance.isClear[1] = true;
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
            if (encounterAudio != null) { 
                audioPlay.clip = encounterAudio;
                audioPlay.Play();
            }

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
        //yield return StartCoroutine(MoveUp(monster));

        // 1�� ���
        //yield return new WaitForSeconds(0.3f);

        // �ٽ� �Ʒ��� ����
        //yield return StartCoroutine(MoveDown(monster));

        //yield return new WaitForSeconds(0.3f);

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
        float moveGap = 50f; // �̵� �Ÿ� (1�����Ӵ� �̵��� ��)
        float waitTime = 0.04f; // �̵� ���� (��)

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
    //////////////////////////

    // man ///////////////////////////////////////////////

    IEnumerator manFade()
    {
        Transform monster = fadeImage.transform.Find("jumpScare");
        GameManager.Instance.isClear[0] = true;

        if (monster != null)
        {
            monster.gameObject.SetActive(true);

            yield return new WaitForSeconds(1.0f);

            // Image ������Ʈ ��������
            Image monsterImage = monster.GetComponent<Image>();
            if (monsterImage != null)
            {
                // ���� ���� ����
                yield return StartCoroutine(FadeToBlack(monsterImage));
            }
        }

        yield return new WaitForSeconds(0.5f);
    }




    //////////////////////////

    // gu ///////////////////////////////////////////////

    IEnumerator guFade()
    {
        GameManager.Instance.isClear[2] = true;
        Transform monster = fadeImage.transform.Find("gumiho");

        if (monster != null)
        {
            monster.gameObject.SetActive(true);

            if (encounterAudio != null)
            {
                audioPlay.clip = encounterAudio;
                audioPlay.Play();
            }

            yield return new WaitForSeconds(1.0f);

            // Image ������Ʈ ��������
            Image monsterImage = monster.GetComponent<Image>();
            if (monsterImage != null)
            {
                // ���� ���� ����
                yield return StartCoroutine(FadeToBlack(monsterImage));
            }
        }

        yield return new WaitForSeconds(0.5f);
    }




    //////////////////////////

    // snake ///////////////////////////////////////////////

    IEnumerator snakeFade()
    {
        Transform monster = fadeImage.transform.Find("emugi");

        if (monster != null)
        {
            monster.gameObject.SetActive(true);

            if (encounterAudio != null)
            {
                audioPlay.clip = encounterAudio;
                audioPlay.Play();
            }

            yield return new WaitForSeconds(1.0f);

            // Image ������Ʈ ��������
            Image monsterImage = monster.GetComponent<Image>();
            if (monsterImage != null)
            {
                // ���� ���� ����
                yield return StartCoroutine(FadeToBlack2(monsterImage));
            }
        }

        yield return new WaitForSeconds(0.5f);
    }




    //////////////////////////


    IEnumerator FadeToBlack(Image monsterImage)
    {
        Color initialColor = monsterImage.color;
        Color targetColor = Color.black; // ���������� ����

        float transitionTime = 1.0f; // ���� ��ȯ �ð�
        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            monsterImage.color = Color.Lerp(initialColor, targetColor, elapsedTime / transitionTime);
            yield return null;
        }

        monsterImage.color = targetColor;

        yield return new WaitForSeconds(0.3f);
        MoveScnene();

    }

    IEnumerator FadeToBlack2(Image monsterImage)
    {
        Color initialColor = monsterImage.color;
        Color targetColor = Color.black; // ���������� ����

        float transitionTime = 1.0f; // ���� ��ȯ �ð�
        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            monsterImage.color = Color.Lerp(initialColor, targetColor, elapsedTime / transitionTime);
            yield return null;
        }

        monsterImage.color = targetColor;

        yield return new WaitForSeconds(0.3f);
        MoveBossScnene();

    }

    public void MoveScnene()
    {
        for (int i = 0; i < enemy.Count; i++)
        {
            GameManager.Instance.enemies.Add(enemy[i]);
        }
        GameManager.Instance.bg = bg;
        GameManager.Instance.monster = monster;
        GameManager.Instance.Reward = reward;


        GameManager.Instance.playerPos = GameObject.Find("Player").transform.position ;
        SceneManager.LoadScene("BattleScene");

    }

    public void MoveBossScnene()
    {
        for (int i = 0; i < enemy.Count; i++)
        {
            GameManager.Instance.enemies.Add(enemy[i]);
        }
        GameManager.Instance.bg = bg;
        GameManager.Instance.monster = monster;
        GameManager.Instance.Reward = reward;

        SceneManager.LoadScene("BossScene");

    }

}
