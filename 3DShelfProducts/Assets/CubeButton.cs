using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeButton : MonoBehaviour
{
    public ProductManager productManager;
    public CanvasManager canvasManager;
    public Animator animator;
    private void OnMouseDown()
    {
        // Check if ProductManager is assigned
        if (productManager != null)
        {
            productManager.OnShowProductsButtonClicked();
            canvasManager.ShowProductDetailsCanvas();
            animator.SetTrigger("PlayStarAnimation");


        }
        else
        {
            Debug.LogWarning("ProductManager reference is missing.");
        }
    }
}
