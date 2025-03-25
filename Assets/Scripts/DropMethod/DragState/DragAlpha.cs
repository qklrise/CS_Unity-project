using UnityEngine;

public class DragAlpha : DropStateGravity
{
    Material oriMaterial; 
    public Material dragAlpha;
    Renderer[] rendArray = null;
    [SerializeField, Range(0.0f, 1.0f)] float alphaValue = 0.4f;
    protected override void OnDragSet()
    {
        base.OnDragSet();
        rendArray = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in rendArray) 
        {
            if(oriMaterial == null) oriMaterial = renderer.material;
            renderer.material = dragAlpha;
        }
        //dragAlpha.SetColor("_Color",Color.green);
        dragAlpha.color = Color.green;
        dragAlpha.SetFloat("_Alpha", alphaValue);
    }

    protected override void DragAlphaTF(bool tf)
    {
        if(tf && dragAlpha.color != Color.green) 
            dragAlpha.color = Color.green;

        else if(!tf && dragAlpha.color != Color.red) 
            dragAlpha.color = Color.red;
    }

    protected override void EndDragSet()
    {
        foreach (Renderer renderer in rendArray)
        {
            renderer.material = oriMaterial;
        }
        base.EndDragSet();
    }
}
