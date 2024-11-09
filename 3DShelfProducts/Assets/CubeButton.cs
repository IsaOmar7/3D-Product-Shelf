using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeButton : MonoBehaviour
{
    public ProductManager productManager;
    public CanvasManager canvasManager;

    private void OnMouseDown()
    {
        // Check if ProductManager is assigned
        if (productManager != null)
        {
            productManager.OnShowProductsButtonClicked();
            canvasManager.ShowProductDetailsCanvas();
           
        }
        else
        {
            Debug.LogWarning("ProductManager reference is missing.");
        }
    }
}
