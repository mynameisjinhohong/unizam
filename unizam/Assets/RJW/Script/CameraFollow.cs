using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // 플레이어 객체
    public Vector3 offset;    // 카메라와 플레이어 사이의 오프셋
    public List<Transform> backgroundImages;  // 배경 이미지 UI 리스트

    private Vector3 lastPosition;  // 카메라의 마지막 위치 (배경 이동을 위해)

    void Start()
    {
        // 카메라를 플레이어의 자식으로 설정
        transform.SetParent(player);

        // 처음 카메라의 위치 기록
        lastPosition = transform.position;
    }

    void LateUpdate()
    {
        // 카메라 위치를 플레이어의 위치 + 오프셋으로 설정
        transform.localPosition = offset;

        // 배경 이미지도 함께 움직이도록 설정
        MoveBackground();

        // 카메라의 마지막 위치 업데이트
        lastPosition = transform.position;
    }

    // 배경 이미지를 카메라와 함께 움직이게 하는 함수
    void MoveBackground()
    {
        foreach (Transform background in backgroundImages)
        {
            // 배경 위치를 카메라의 이동량만큼 이동시킴
            Vector3 backgroundMove = transform.position - lastPosition;
            background.localPosition += backgroundMove;
        }
    }
}
