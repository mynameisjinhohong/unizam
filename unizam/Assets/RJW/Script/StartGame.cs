using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public RawImage fadeImage;
    public List<Button> buttonList;

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
        buttonList[0].gameObject.transform.localScale = Vector3.zero;
        buttonList[1].gameObject.transform.localScale = Vector3.zero;
        StartCoroutine(Fade());
    }

    public void GameQuit()
    {
        buttonList[0].gameObject.transform.localScale = Vector3.zero;
        buttonList[1].gameObject.transform.localScale = Vector3.zero;

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit(); 
        #endif

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
        SceneManager.LoadScene("OpeningScene");
        // SceneManager.LoadScene("MainScene");
    }

}
