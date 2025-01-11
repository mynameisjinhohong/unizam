using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //�������� ������ ��� �ϴ� ��
    public List<EnemeyData> enemies;
    public Behaviour Reward;
    public monster monster;
    public Sprite bg;

    public int maxHp;
    public int maxMp;
    public PlayerCharacter player;

    // Ŭ������ ���� ���
    public List<bool> isClear = new List<bool>();

    // ����â UI
    public GameObject quitUI;
    public Image fadeImage;
    public GameObject canvas; // Canvas ������Ʈ�� ����

    public Vector3 playerPos;

    public bool moveAble = true;

    private void Awake()
    {
        moveAble = true;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player.hp = maxHp;

        // �� �ε� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (player.hp > maxHp)
        {
            player.hp = maxHp;
        }
        if (player.mp > maxMp)
        {
            player.mp = maxMp;
        }

        // ����â Ȱ��ȭ
        if (Input.GetKey(KeyCode.Escape) && SceneManager.GetActiveScene().name == "MainScene")
        {
            canvas.SetActive(true);
            quitUI.SetActive(true);
        }
    }

    public void Quit()
    {
        if(canvas.gameObject.activeSelf == false)
            canvas.SetActive(true);
        StartCoroutine(Fade());
    }

    public void Quit2() {
        if (canvas.gameObject.activeSelf == false)
            canvas.SetActive(true);
        StartCoroutine(FadeQuit());
    }

    IEnumerator Fade()
    {
        GameManager.Instance.moveAble = false;
        quitUI.SetActive(false);

        // ���̵�ƿ� (���� ���� ������Ŵ)
        float fadeCount = 0.0f;
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f; // ���� ���� ����
            fadeImage.color = new Color(0, 0, 0, fadeCount); // �������� ���� ���� ����
            yield return new WaitForSeconds(0.01f);
        }

        GameManager.Instance.moveAble = true;
        // �� ��ȯ
        SceneManager.LoadScene("StartScene");

        // �� �ε� �Ϸ� �� ���̵��� (���� ���� ���ҽ�Ŵ)
        fadeCount = 1.0f; // ���� �ʱ�ȭ
        while (fadeCount > 0.0f)
        {
            fadeCount -= 0.01f; // ���� ���� ����
            fadeImage.color = new Color(0, 0, 0, fadeCount); // �������� ���� ���� ����
            yield return new WaitForSeconds(0.01f);
        }

        canvas.SetActive(false);
    }

    IEnumerator FadeQuit()
    {
        quitUI.SetActive(false);

        // ���̵�ƿ� (���� ���� ������Ŵ)
        float fadeCount = 0.0f;
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f; // ���� ���� ����
            fadeImage.color = new Color(0, 0, 0, fadeCount); // �������� ���� ���� ����
            yield return new WaitForSeconds(0.01f);
        }

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                                Application.Quit(); 
        #endif
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // StartScene������ Canvas ��Ȱ��ȭ
        if (scene.name == "StartScene")
        {
            canvas.SetActive(false);
        }
        // MainScene������ Canvas Ȱ��ȭ
        else if (scene.name == "MainScene")
        {
            canvas.SetActive(false);
            GameObject.Find("Player").transform.position = playerPos;
        }
    }

    private void OnDestroy()
    {
        // �� �ε� �̺�Ʈ ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
