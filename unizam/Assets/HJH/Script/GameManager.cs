using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //전투씬에 전달해 줘야 하는 것
    public List<EnemeyData> enemies;
    public Behaviour Reward;
    public monster monster;
    public Sprite bg;

    public int maxHp;
    public int maxMp;
    public PlayerCharacter player;

    // 클리어한 적들 목록
    public List<bool> isClear = new List<bool>();

    // 종료창 UI
    public GameObject quitUI;
    public Image fadeImage;
    public GameObject canvas; // Canvas 오브젝트를 참조

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

    public void Init()
    {
        player.hp = maxHp;
        player.mp = 0;
        isClear = new List<bool>();
        for(int i =0; i<3; i++)
        {
            isClear.Add(false);
        }
    }

    private void Start()
    {
        Init();

        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        //GameObject.Find("Player").transform.localPosition = playerPos;
        if (player.hp > maxHp)
        {
            player.hp = maxHp;
        }
        if (player.mp > maxMp)
        {
            player.mp = maxMp;
        }

        // 종료창 활성화
        if (Input.GetKey(KeyCode.Escape) && (SceneManager.GetActiveScene().name == "MainScene" || SceneManager.GetActiveScene().name == "BattleScene" || SceneManager.GetActiveScene().name == "BossScene"))
        {
            canvas.SetActive(true);
            quitUI.SetActive(true);
        }
    }

    public void Quit()
    {
        if(canvas.gameObject.activeSelf == false)
            canvas.SetActive(true);
        Init();
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

        // 페이드아웃 (알파 값을 증가시킴)
        float fadeCount = 0.0f;
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f; // 알파 값을 증가
            fadeImage.color = new Color(0, 0, 0, fadeCount); // 검정색의 알파 값만 변경
            yield return new WaitForSeconds(0.01f);
        }

        GameManager.Instance.moveAble = true;
        // 씬 전환
        SceneManager.LoadScene("StartScene");

        // 씬 로드 완료 후 페이드인 (알파 값을 감소시킴)
        fadeCount = 1.0f; // 알파 초기화
        while (fadeCount > 0.0f)
        {
            fadeCount -= 0.01f; // 알파 값을 감소
            fadeImage.color = new Color(0, 0, 0, fadeCount); // 검정색의 알파 값만 변경
            yield return new WaitForSeconds(0.01f);
        }

        canvas.SetActive(false);
    }

    IEnumerator FadeQuit()
    {
        quitUI.SetActive(false);

        // 페이드아웃 (알파 값을 증가시킴)
        float fadeCount = 0.0f;
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f; // 알파 값을 증가
            fadeImage.color = new Color(0, 0, 0, fadeCount); // 검정색의 알파 값만 변경
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
        // StartScene에서는 Canvas 비활성화
        if (scene.name == "StartScene")
        {
            canvas.SetActive(false);
        }
        // MainScene에서는 Canvas 활성화
        else if (scene.name == "MainScene")
        {
            canvas.SetActive(false);
            GameObject.Find("Player").transform.localPosition = playerPos;
        }
        else if (scene.name == "BattleScene"|| scene.name == "BossScene") {
            BattleManager.instance.restart.onClick.AddListener(() =>
            {
                Quit();
                Init();
            }
            );
            BattleManager.instance.quit.onClick.AddListener(() => Quit2());
        }
    }

    private void OnDestroy()
    {
        // 씬 로드 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
