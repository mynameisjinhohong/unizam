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
    public bool boss;
    public Sprite bg;

    public int maxHp;
    public int maxMp;
    public PlayerCharacter player;

    // ����â UI
    public GameObject quitUI;
    public Image fadeImage;
    public GameObject canvas; // Canvas ������Ʈ�� ����

    private void Awake()
    {
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
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
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
        }
    }

    private void OnDestroy()
    {
        // �� �ε� �̺�Ʈ ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
