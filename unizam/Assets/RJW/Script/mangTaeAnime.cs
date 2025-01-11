using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mangTaeAnime : MonoBehaviour
{
    public List<Sprite> mangTaePics; // 이미지 리스트
    public Image currentPic;         // 현재 표시 중인 이미지
    private int currentIndex = 0;    // 현재 인덱스
    private float timer = 0f;        // 시간 카운터
    public float changeInterval = 0.1f; // 이미지 변경 간격 (초)

    void Start()
    {
        currentPic = GetComponent<Image>();

        // 초기 이미지 설정
        if (mangTaePics.Count > 0)
        {
            currentPic.sprite = mangTaePics[currentIndex];
        }
    }

    void Update()
    {
        if (mangTaePics.Count == 0)
            return;

        // 시간 누적
        timer += Time.deltaTime;

        // 변경 간격에 도달하면 이미지 변경
        if (timer >= changeInterval)
        {
            timer = 0f; // 타이머 초기화
            currentIndex = (currentIndex + 1) % mangTaePics.Count; // 다음 인덱스 계산
            currentPic.sprite = mangTaePics[currentIndex]; // 이미지 변경
        }
    }
}
