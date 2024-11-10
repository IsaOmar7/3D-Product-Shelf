using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Manages the display of different UI canvases in the scene, such as the welcome screen and product details screen.
/// </summary>
public class CanvasManager : MonoBehaviour
{
    public GameObject welcomeCanvas;        
    public GameObject productDetailsCanvas;

    /// <summary>
    /// Initializes the canvas display settings.
    /// Activates the welcome canvas and hides the product details canvas.
    /// </summary>
    void Start()
    {
        welcomeCanvas.SetActive(true);
        productDetailsCanvas.SetActive(false);
    }

    /// <summary>
    /// Switches the active canvas from the welcome screen to the product details screen.
    /// </summary>
    public void ShowProductDetailsCanvas()
    {
        welcomeCanvas.SetActive(false);
        productDetailsCanvas.SetActive(true);
    }
}
