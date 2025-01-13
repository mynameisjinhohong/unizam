using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayerUI : MonoBehaviour
{
    public Transform player;
    public RectTransform uiText; 
    public Vector3 offset = new Vector3(0, 5, 0); 
    public Camera mainCamera; // 메인 카메라 (월드 좌표를 스크린 좌표로 변환하기 위해 필요)

    void Update()
    {
        if (player == null || uiText == null || mainCamera == null) return;

        // 월드 좌표에서 플레이어 위치 + 오프셋 계산
        Vector3 targetWorldPosition = player.position + offset;

        // 플레이어의 월드 좌표를 스크린 좌표로 변환
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetWorldPosition);

        // 텍스트 UI의 스크린 좌표 업데이트
        uiText.position = screenPosition;
    }
}
