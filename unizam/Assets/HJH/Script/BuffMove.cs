using UnityEngine;

public class BuffMove : MonoBehaviour
{
    void Start()
    {

        // 화면 픽셀 좌표를 뷰포트 좌표(0~1)로 변환
        Vector3 viewportPos = Camera.main.ScreenToViewportPoint(transform.position);
        // 화면의 왼쪽에 있는 경우 (뷰포트 좌표의 x 값이 0.5보다 작음)
        if (viewportPos.x > 0.5f)
        {
            // 자식 오브젝트의 위치 변경
            transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-450, 0);
        }
        else
        {

            transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }
    }
}
