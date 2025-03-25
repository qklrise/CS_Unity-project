using UnityEngine;

public class DollSensor : MonoBehaviour
{
    public LayerMask mask;
    protected KeyCode colorKey; // ���� ������ �Է�Ű�� ���� ����
    protected Collider[] list;

    private void Start()
    {
        SetColorKey(); // ���� ���� �Է�Ű�� ����
    }
    void Update()
    {
        if (Input.GetKeyDown(colorKey) && !GameManager.isPuzzle)
        {
            Vector3 half = new Vector3 (0.3f, 1.0f, 0.3f); // overlapbox�� ������ �ڽ� �ݶ��̴��� ���� ������ ��
            list = Physics.OverlapBox(transform.position + transform.up + transform.forward * 0.5f, half , Quaternion.identity, mask);
            // ������ �߽������� Ư�� ���⸸ŭ �̵��� ���� �ڽ� �ݶ��̴��� �����ؼ� ������ layer�� ���� ������Ʈ�� ������ �迭�� ����
            if(list.Length > 0) // �ڽ� �ݶ��̴��� ��ġ�� ������ layer�� ���� ������Ʈ�� �ִٸ�
            {
                Operate();
            }
        }
    }

    protected virtual void SetColorKey() // ������ ������ ����ؼ� ���
    {
        
    }

    protected virtual void Operate() //�۵���ų ��ġ���� ����ؼ� ���Ǹ� �ٸ��� �� 
    {

    }
}
