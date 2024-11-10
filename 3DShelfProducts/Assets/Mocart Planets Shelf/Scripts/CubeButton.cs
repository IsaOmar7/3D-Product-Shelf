using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handles interactions with a the star button, triggering actions in the ProductManager, CanvasManager, and Star Animator.
/// </summary>
public class CubeButton : MonoBehaviour
{
    public ProductManager productManager;
    public CanvasManager canvasManager;
    public Animator animator;
    
    /// <summary>
    /// Called when the cube button is clicked.
    /// Checks if ProductManager is assigned, and if so, triggers product display and animations.
    /// </summary>
    private void OnMouseDown()
    {
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
