using UnityEngine;

public class DollInteraction : AnimProperty
{
    public LayerMask Spring1_Button;
    public LayerMask Spring2_Button;

    GameObject InteractTarget;

    protected void Interact()
    {
        Collider[] list = Physics.OverlapBox(transform.position + transform.up * 0.7f + transform.forward * 0.5f, new Vector3(0.4f, 1.4f, 1.0f) * 0.5f, transform.rotation);
        foreach (Collider col in list)
        {
            //----------------------------------------------------------------------------------------------------------------------------------------------------------

            if ((1 << col.gameObject.layer & Spring1_Button) != 0)
            // ��ȣ�ۿ� ����� 'Button1' �� ��
            {
                InteractTarget = col.gameObject;
                col.GetComponentInParent<Animator>()?.SetTrigger("Spring1On");
                InteractTarget = null;
            }
            if ((1 << col.gameObject.layer & Spring2_Button) != 0)
            // ��ȣ�ۿ� ����� 'Button2' �� ��
            {
                InteractTarget = col.gameObject;
                col.GetComponentInParent<Animator>()?.SetTrigger("Using");
                InteractTarget = null;
            }
        }
    }
}
