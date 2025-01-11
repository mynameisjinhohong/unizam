using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Encounter : MonoBehaviour
{
    public Image fadeImage; // 페이드 효과 이미지
    public List<Sprite> changeImages; // 변경할 스프라이트 목록

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

            Image monsterImage = monster.GetComponent<Image>(); // Image 컴포넌트 가져오기
            if (monsterImage != null && changeImages.Count > 0)
            {
                // changeImages 목록의 스프라이트들로 순차적으로 변경
                //StartCoroutine(ChangeMonsterImages(monsterImage));
                foreach (Sprite sprite in changeImages)
                {
                    monsterImage.sprite = sprite; // Image 컴포넌트의 스프라이트 변경
                    yield return new WaitForSeconds(0.5f); // 0.5초 대기
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
            monsterImage.sprite = sprite; // Image 컴포넌트의 스프라이트 변경
            yield return new WaitForSeconds(1.0f); // 0.5초 대기
        }
    }

    IEnumerator MoveMonster(Transform monster)
    {
        float endY = -6920f; // 목표 y 값
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
