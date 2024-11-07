using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProductDisplay : MonoBehaviour
{
    public TextMeshProUGUI productNameDisplay;

    private Product product;
    private ProductManager productManager;

    // Initialize with product data and manager reference
    public void Initialize(Product product, ProductManager manager)
    {
        this.product = product;
        productManager = manager;

        // Set display name on the prefab (optional)
        productNameDisplay.text = product.name;
    }

    // Detect clicks to show product details on the Canvas
    private void OnMouseDown()
    {
        productManager.ShowProductDetailsOnCanvas(product);
    }
}
