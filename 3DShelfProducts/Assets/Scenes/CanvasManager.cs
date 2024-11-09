using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject welcomeCanvas;        
    public GameObject productDetailsCanvas;
    // Start is called before the first frame update
    void Start()
    {
        welcomeCanvas.SetActive(true);
        productDetailsCanvas.SetActive(false);
    }

    public void ShowProductDetailsCanvas()
    {
        welcomeCanvas.SetActive(false);
        productDetailsCanvas.SetActive(true);
    }
}
