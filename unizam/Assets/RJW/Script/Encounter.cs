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
    public bool clear = false;

    public List<EnemeyData> enemy;
    public Sprite bg;
    public Behaviour reward;

    public monster monster;

    public Image fadeImage; // 페이드 효과 이미지
    public List<Sprite> changeImages; // 변경할 스프라이트 목록

    public int clearIndex;

    public AudioClip encounterAudio;
    public AudioClip hintaudioAudio;
    //1 - 두억 등장, 2- 구미호 등장, 3- 이무기 등장
    public AudioSource audioPlay;


    public GameObject encounterUI;

    void Start()
    {
    }

    void Update()
    {
        if (isVisit == true) {

            clear = true;

            if(GameObject.FindWithTag("Hint") != null && GameObject.FindWithTag("Hint").active == true)
                GameObject.FindWithTag("Hint").SetActive(false);


            switch (monster)
            {
                case monster.du:
                    if (GameManager.Instance.isClear[1] == false)
                    {
                        StartCoroutine(duFade());
                        GameManager.Instance.moveAble = false;
                    }
                    break;
                case monster.gu:
                    if (GameManager.Instance.isClear[2] == false)
                    {
                        StartCoroutine(guFade());
                        GameManager.Instance.moveAble = false;
                    }
                    break;
                case monster.snake:
                    //StartCoroutine(snakeFade());
                    //GameManager.Instance.moveAble = false;
                    if (GameManager.Instance.isClear[0] == true && GameManager.Instance.isClear[1] == true && GameManager.Instance.isClear[2] == true)
                    {
                        StartCoroutine(snakeFade());
                        GameManager.Instance.moveAble = false;
                    }
                    break;
                case monster.man:
                    if (GameManager.Instance.isClear[0] == false)
                    {
                        StartCoroutine(manFade());
                        GameManager.Instance.moveAble = false;
                    }
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Scan") && monster == monster.snake)
        {
            StartCoroutine(snakeFade());
        }
        if (collision.CompareTag("Player"))
        {
            Debug.Log("방문");
            if (GameManager.Instance.isClear[clearIndex] == false)
            {
                audioPlay.clip = hintaudioAudio;
                audioPlay.Play();
            }
            isVisit = true;

            /*
            if (monster == monster.snake) {
                StartCoroutine(snakeFade());
            }
            */
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("나감");
        isVisit = false;
    }

    // du ///////////////////////////////////////////////
    IEnumerator duFade()
    {

        //encounterUI.SetActive(true);
        //yield return new WaitForSeconds(1.3f);
        //encounterUI.SetActive(false);

        GameManager.Instance.isClear[1] = true;

        Transform monster = fadeImage.transform.Find("downToUP");
        if (monster != null)
        {
            float fadeCount = 0;

            // 빠르게 하얀색으로 깜빡이는 반복
            for (int i = 0; i < 3; i++) // 3번 깜빡이기
            {
                // 하얀색으로 페이드 인
                while (fadeCount < 1.0f)
                {
                    fadeCount += 0.1f; // 페이드 속도 조정
                    fadeImage.color = new Color(1, 1, 1, fadeCount); // 하얀색으로 변경
                    yield return new WaitForSeconds(0.02f);
                }

                // 초기화
                fadeCount = 0;

                // 하얀색으로 페이드 아웃
                while (fadeCount < 1.0f)
                {
                    fadeCount += 0.1f;
                    fadeImage.color = new Color(1, 1, 1, 1 - fadeCount); // 투명으로 변경
                    yield return new WaitForSeconds(0.02f);
                }

                fadeCount = 0;
            }

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

        // 아래로 내림
        yield return StartCoroutine(MoveDown(monster));

        // 1초 대기
        yield return new WaitForSeconds(1.0f);

        // 원래 위치로 올림
        //yield return StartCoroutine(MoveUp(monster));

        // 1초 대기
        //yield return new WaitForSeconds(0.3f);

        // 다시 아래로 내림
        //yield return StartCoroutine(MoveDown(monster));

        //yield return new WaitForSeconds(0.3f);

        // Y값 설정 완료 후 색상 변경 시작
        Image monsterImage = monster.GetComponent<Image>();
        if (monsterImage != null)
        {
            yield return StartCoroutine(FadeToBlack(monsterImage));
        }
    }

    IEnumerator MoveDown(Transform monster)
    {
        float endY = -6920f; // 목표 y 값
        float moveGap = 30f; // 이동 거리 (1프레임당 이동할 값) - 더 빠르게 이동
        float waitTime = 0.015f; // 이동 간격 (초)

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

            yield return new WaitForSeconds(waitTime);
        }

        currentPos.y = endY;
        monster.position = currentPos;
    }

    IEnumerator MoveUp(Transform monster)
    {
        float startY = -721.0f; // 원래 위치
        float moveGap = 50f; // 이동 거리 (1프레임당 이동할 값)
        float waitTime = 0.04f; // 이동 간격 (초)

        Vector3 currentPos = monster.position;

        // 원래 위치로 돌아갈 때까지 이동
        while (currentPos.y < startY)
        {
            currentPos.y += moveGap;

            // 원래 위치를 초과하지 않도록 제한
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

            float fadeCount = 0;

            // 빠르게 하얀색으로 깜빡이는 반복
            for (int i = 0; i < 3; i++) // 3번 깜빡이기
            {
                // 하얀색으로 페이드 인
                while (fadeCount < 1.0f)
                {
                    fadeCount += 0.1f; // 페이드 속도 조정
                    fadeImage.color = new Color(1, 1, 1, fadeCount); // 하얀색으로 변경
                    yield return new WaitForSeconds(0.02f);
                }

                // 초기화
                fadeCount = 0;

                // 하얀색으로 페이드 아웃
                while (fadeCount < 1.0f)
                {
                    fadeCount += 0.1f;
                    fadeImage.color = new Color(1, 1, 1, 1 - fadeCount); // 투명으로 변경
                    yield return new WaitForSeconds(0.02f);
                }

                fadeCount = 0;
            }

            // 검은색으로 완전히 페이드 아웃
            while (fadeCount < 1.0f)
            {
                fadeCount += 0.01f;
                fadeImage.color = new Color(0, 0, 0, fadeCount); // 검은색으로 변경
                yield return new WaitForSeconds(0.01f);
            }

            /*
            monster.gameObject.SetActive(true);

            //yield return new WaitForSeconds(1.0f);

            // encounterUI.SetActive(true);
            //yield return new WaitForSeconds(1.3f);
            //encounterUI.SetActive(false);

            StartCoroutine(BlinkFade());
            */

            // Image 컴포넌트 가져오기
            Image monsterImage = monster.GetComponent<Image>();

            
            if (monsterImage != null)
            {
                // 색상 변경 시작
                yield return StartCoroutine(FadeToBlack(monsterImage));
            }
        }

        yield return new WaitForSeconds(0.5f);
    }




    //////////////////////////

    // gu ///////////////////////////////////////////////

    IEnumerator guFade()
    {

        //encounterUI.SetActive(true);
        //yield return new WaitForSeconds(1.3f);
        //encounterUI.SetActive(false);

        GameManager.Instance.isClear[2] = true;

        /*
        float fadeCount = 0;
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            fadeImage.color = new Color(0, 0, 0, fadeCount);
        }
        */


        Transform monster = fadeImage.transform.Find("gumiho");

        if (monster != null)
        {
            float fadeCount = 0;

            // 빠르게 하얀색으로 깜빡이는 반복
            for (int i = 0; i < 3; i++) // 3번 깜빡이기
            {
                // 하얀색으로 페이드 인
                while (fadeCount < 1.0f)
                {
                    fadeCount += 0.1f; // 페이드 속도 조정
                    fadeImage.color = new Color(1, 1, 1, fadeCount); // 하얀색으로 변경
                    yield return new WaitForSeconds(0.02f);
                }

                // 초기화
                fadeCount = 0;

                // 하얀색으로 페이드 아웃
                while (fadeCount < 1.0f)
                {
                    fadeCount += 0.1f;
                    fadeImage.color = new Color(1, 1, 1, 1 - fadeCount); // 투명으로 변경
                    yield return new WaitForSeconds(0.02f);
                }

                fadeCount = 0;
            }

            // 검은색으로 완전히 페이드 아웃
            while (fadeCount < 1.0f)
            {
                fadeCount += 0.01f;
                fadeImage.color = new Color(0, 0, 0, fadeCount); // 검은색으로 변경
                yield return new WaitForSeconds(0.01f);
            }

            monster.gameObject.SetActive(true);

            if (encounterAudio != null)
            {
                audioPlay.clip = encounterAudio;
                audioPlay.Play();
            }

            yield return new WaitForSeconds(1.0f);

            // Image 컴포넌트 가져오기
            Image monsterImage = monster.GetComponent<Image>();
            if (monsterImage != null)
            {
                // 색상 변경 시작
                yield return StartCoroutine(FadeToBlack(monsterImage));
            }
        }

        yield return new WaitForSeconds(0.5f);
    }




    //////////////////////////

    // snake ///////////////////////////////////////////////

    IEnumerator snakeFade()
    {
        yield return new WaitForSeconds(1.0f);
        //encounterUI.SetActive(true);
        //yield return new WaitForSeconds(1.3f);
        //encounterUI.SetActive(false);


        Transform monster = fadeImage.transform.Find("emugi");

        if (monster != null)
        {
            float fadeCount = 0;

            // 빠르게 하얀색으로 깜빡이는 반복
            for (int i = 0; i < 3; i++) // 3번 깜빡이기
            {
                // 하얀색으로 페이드 인
                while (fadeCount < 1.0f)
                {
                    fadeCount += 0.1f; // 페이드 속도 조정
                    fadeImage.color = new Color(1, 1, 1, fadeCount); // 하얀색으로 변경
                    yield return new WaitForSeconds(0.02f);
                }

                // 초기화
                fadeCount = 0;

                // 하얀색으로 페이드 아웃
                while (fadeCount < 1.0f)
                {
                    fadeCount += 0.1f;
                    fadeImage.color = new Color(1, 1, 1, 1 - fadeCount); // 투명으로 변경
                    yield return new WaitForSeconds(0.02f);
                }

                fadeCount = 0;
            }

                        // 검은색으로 완전히 페이드 아웃
            while (fadeCount < 1.0f)
            {
                fadeCount += 0.01f;
                fadeImage.color = new Color(0, 0, 0, fadeCount); // 검은색으로 변경
                yield return new WaitForSeconds(0.01f);
            }

            monster.gameObject.SetActive(true);

            if (encounterAudio != null)
            {
                audioPlay.clip = encounterAudio;
                audioPlay.Play();
            }

            yield return new WaitForSeconds(1.0f);

            // Image 컴포넌트 가져오기
            Image monsterImage = monster.GetComponent<Image>();
            if (monsterImage != null)
            {
                // 색상 변경 시작
                yield return StartCoroutine(FadeToBlack2(monsterImage));
            }
        }

        yield return new WaitForSeconds(0.5f);
    }




    //////////////////////////


    IEnumerator FadeToBlack(Image monsterImage)
    {
        Color initialColor = monsterImage.color;
        Color targetColor = Color.black; // 검은색으로 변경

        float transitionTime = 1.0f; // 색상 전환 시간
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
        Color targetColor = Color.black; // 검은색으로 변경

        float transitionTime = 1.0f; // 색상 전환 시간
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
        GameManager.Instance.moveAble = true;
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
        GameManager.Instance.moveAble = true;
        SceneManager.LoadScene("BossScene");

    }

}
