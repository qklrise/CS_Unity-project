using UnityEngine;

public class DragAlpha : DropStateGravity
{
    Material[] oriMateArray = null; 
    public Material dragAlpha;
    Renderer[] rendArray = null;
    [SerializeField, Range(0.0f, 1.0f)] float alphaValue = 0.4f;
    protected override void OnDragSet()
    {
        base.OnDragSet();
        rendArray = GetComponentsInChildren<Renderer>();
        oriMateArray = new Material[rendArray.Length];

        for(int i = 0; i < rendArray.Length; i++)
        {
            oriMateArray[i] = rendArray[i].material;
            rendArray[i].material = dragAlpha;
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
        for (int i = 0; i < rendArray.Length; i++)
        { 
            rendArray[i].material = oriMateArray[i];
        }
        base.EndDragSet();
    }
}
