using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayerUI : MonoBehaviour
{
    public Transform player;
    public RectTransform uiText; 
    public Vector3 offset = new Vector3(0, 5, 0); 
    public Camera mainCamera; // ���� ī�޶� (���� ��ǥ�� ��ũ�� ��ǥ�� ��ȯ�ϱ� ���� �ʿ�)

    void Update()
    {
        if (player == null || uiText == null || mainCamera == null) return;

        // ���� ��ǥ���� �÷��̾� ��ġ + ������ ���
        Vector3 targetWorldPosition = player.position + offset;

        // �÷��̾��� ���� ��ǥ�� ��ũ�� ��ǥ�� ��ȯ
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetWorldPosition);

        // �ؽ�Ʈ UI�� ��ũ�� ��ǥ ������Ʈ
        uiText.position = screenPosition;
    }
}
