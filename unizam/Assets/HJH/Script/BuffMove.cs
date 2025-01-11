using UnityEngine;

public class BuffMove : MonoBehaviour
{
    void Start()
    {
        // UI 요소의 월드 좌표를 화면 뷰포트 좌표로 변환
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(gameObject.GetComponent<RectTransform>().position);

        // 화면 왼쪽에 있는 경우 (뷰포트 좌표의 x 값이 0.5보다 작음)
        if (viewportPos.x > 0.5f)
        {
            // 자식 오브젝트의 위치 변경
            transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-240, 0);
        }
    }
}
