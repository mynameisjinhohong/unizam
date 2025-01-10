using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public RawImage fadeImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart()
    {
        gameObject.transform.localScale = Vector3.zero;
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        float fadeCount = 1.0f;  // 시작 알파 값은 1로 설정 (완전 불투명)
        while (fadeCount > 0.0f)
        {
            fadeCount -= 0.01f;  // 알파 값을 감소시킴
            yield return new WaitForSeconds(0.01f);
            fadeImage.color = new Color(255, 255, 255, fadeCount);  // 검정색의 알파 값만 변경
        }

        // 씬 전환
        SceneManager.LoadScene("MainScene");
    }

}
