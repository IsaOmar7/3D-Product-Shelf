using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProductDisplay : MonoBehaviour
{
    public TextMeshProUGUI productNameDisplay;

    private Product product;
    private ProductManager productManager;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        transform.Rotate(Vector3.up * 10f * Time.deltaTime);
        float scale = Mathf.PingPong(Time.time * 0.01f, 0.1f - 0.05f) + 0.05f;
        transform.localScale = new Vector3(scale, scale, scale);
    }
    // Initialize with product data and manager reference
    public void Initialize(Product product, ProductManager manager)
    {
        this.product = product;
        productManager = manager;

    }

    // Detect clicks to show product details on the Canvas
    private void OnMouseDown()
    {
        productManager.ShowProductDetailsOnCanvas(product);
        audioSource.enabled=true;
        audioSource.Play();
    }
}
