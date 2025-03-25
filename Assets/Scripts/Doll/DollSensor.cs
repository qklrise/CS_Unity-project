using UnityEngine;

public class DollSensor : MonoBehaviour
{
    public LayerMask mask;
    protected KeyCode colorKey; // 인형 색별로 입력키를 받을 변수
    protected Collider[] list;

    private void Start()
    {
        SetColorKey(); // 색에 따른 입력키를 설정
    }
    void Update()
    {
        if (Input.GetKeyDown(colorKey) && !GameManager.isPuzzle)
        {
            Vector3 half = new Vector3 (0.3f, 1.0f, 0.3f); // overlapbox로 생성할 박스 콜라이더의 각변 길이의 반
            list = Physics.OverlapBox(transform.position + transform.up + transform.forward * 0.5f, half , Quaternion.identity, mask);
            // 인형의 중심점에서 특정 방향만큼 이동한 곳에 박스 콜라이더를 생성해서 지정한 layer를 가진 오브젝트가 있으면 배열로 저장
            if(list.Length > 0) // 박스 콜라이더의 위치에 지정한 layer를 가진 오브젝트가 있다면
            {
                Operate();
            }
        }
    }

    protected virtual void SetColorKey() // 인형의 색별로 상속해서 사용
    {
        
    }

    protected virtual void Operate() //작동시킬 장치별로 상속해서 정의를 다르게 함 
    {

    }
}
