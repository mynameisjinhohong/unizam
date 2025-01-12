using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // 플레이어 Transform
    public List<RectTransform> backgroundUI; // 배경 UI 리스트

    private Vector3 offset; // 플레이어와 카메라의 초기 간격
    private Vector3 previousPlayerPosition; // 플레이어의 이전 위치

    void Start()
    {
        if (player != null)
        {
            offset = transform.position - player.position;
            previousPlayerPosition = player.position;
        }
    }

    void LateUpdate()
    {
        //if (player != null)
        //{
        //    // 카메라 위치 갱신
        //    transform.position = player.position + offset;

        //    // 플레이어 이동량 계산
        //    Vector3 playerDelta = player.position - previousPlayerPosition;

        //    // 배경 UI 이동
        //    foreach (RectTransform bg in backgroundUI)
        //    {
        //        if (bg != null)
        //        {
        //            bg.position -= playerDelta; // 배경 UI를 플레이어 이동량만큼 이동
        //        }
        //    }

        //    // 플레이어의 현재 위치 저장
        //    previousPlayerPosition = player.position;
        //}
    }
}
