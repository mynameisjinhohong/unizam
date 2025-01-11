using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mangTaeAnime : MonoBehaviour
{
    public List<Sprite> mangTaePics; // �̹��� ����Ʈ
    public Image currentPic;         // ���� ǥ�� ���� �̹���
    private int currentIndex = 0;    // ���� �ε���
    private float timer = 0f;        // �ð� ī����
    public float changeInterval = 0.1f; // �̹��� ���� ���� (��)

    void Start()
    {
        currentPic = GetComponent<Image>();

        // �ʱ� �̹��� ����
        if (mangTaePics.Count > 0)
        {
            currentPic.sprite = mangTaePics[currentIndex];
        }
    }

    void Update()
    {
        if (mangTaePics.Count == 0)
            return;

        // �ð� ����
        timer += Time.deltaTime;

        // ���� ���ݿ� �����ϸ� �̹��� ����
        if (timer >= changeInterval)
        {
            timer = 0f; // Ÿ�̸� �ʱ�ȭ
            currentIndex = (currentIndex + 1) % mangTaePics.Count; // ���� �ε��� ���
            currentPic.sprite = mangTaePics[currentIndex]; // �̹��� ����
        }
    }
}
