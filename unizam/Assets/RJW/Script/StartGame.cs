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
        float fadeCount = 1.0f;  // ���� ���� ���� 1�� ���� (���� ������)
        while (fadeCount > 0.0f)
        {
            fadeCount -= 0.01f;  // ���� ���� ���ҽ�Ŵ
            yield return new WaitForSeconds(0.01f);
            fadeImage.color = new Color(255, 255, 255, fadeCount);  // �������� ���� ���� ����
        }

        // �� ��ȯ
        SceneManager.LoadScene("MainScene");
    }

}
