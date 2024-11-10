using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Displays product information in the scene and handles interactions for showing detailed product data.
/// </summary>
public class ProductDisplay : MonoBehaviour
{
    public TextMeshProUGUI productNameDisplay;

    private Product product;
    private ProductManager productManager;
    private AudioSource audioSource;

    private const float RotationSpeed = 10f;          
    private const float ScaleMin = 0.05f;             
    private const float ScaleMaxOffset = 0.1f - 0.05f; 
    private const float ScaleSpeed = 0.01f;

    /// <summary>
    /// Initializes AudioSource components with sound effects.
    /// </summary>
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Updates the display with a rotation and pulsing scale effect to create a visual interaction.
    /// </summary>
    void Update()
    {
        transform.Rotate(Vector3.up * RotationSpeed * Time.deltaTime);
        float scale = Mathf.PingPong(Time.time * ScaleSpeed, ScaleMaxOffset) + ScaleMin;
        transform.localScale = new Vector3(scale, scale, scale);
    }

    /// <summary>
    /// Initializes the display with product data and a reference to the ProductManager.
    /// </summary>
    /// <param name="product">The product data to display.</param>
    /// <param name="manager">Reference to the ProductManager for handling product interactions.</param>
    public void Initialize(Product product, ProductManager manager)
    {
        this.product = product;
        productManager = manager;

    }

    /// <summary>
    /// Handles mouse click events to show product details on the Canvas and play an audio effect.
    /// </summary>
    private void OnMouseDown()
    {
        productManager.ShowProductDetailsOnCanvas(product);
        audioSource.enabled=true;
        audioSource.Play();
    }
}
