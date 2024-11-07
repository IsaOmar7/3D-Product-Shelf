using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.Networking;
using TMPro;



[System.Serializable]
public class Product
{
    public string id;
    public string name;
    public string description;
    public float price;
}


public class ProductManager : MonoBehaviour
{

    public GameObject productPrefab; // Prefab for displaying each product on the shelf
    public Transform[] shelfSpots;   // Array of 3 shelf spots

    // References for the Product Details Canvas
    public GameObject productDetailsCanvas;
    public TMP_InputField productNameInputField;
    public TMP_InputField productDescriptionInputField;
    public TMP_InputField productPriceInputField;

    private string serverUrl = "https://homework.mocart.io/api/products";
    private Product currentProduct;  // The product being displayed on the Canvas


    public void OnShowProductsButtonClicked()
    {
        Debug.Log("Show Products Button Clicked");  // Log for button click
        StartCoroutine(FetchProductsFromServer());
    }

    private IEnumerator FetchProductsFromServer()
    {
        UnityWebRequest request = UnityWebRequest.Get(serverUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            List<Product> products = JsonUtility.FromJson<ProductListWrapper>(jsonResponse).products;
            DisplayProductsOnShelf(products);
        }
        else
        {
            Debug.LogError("Error fetching products: " + request.error);
        }
    }

    private void DisplayProductsOnShelf(List<Product> products)
    {
        ClearShelf();

        for (int i = 0; i < products.Count && i < shelfSpots.Length; i++)
        {
            Product product = products[i];
            GameObject productInstance = Instantiate(productPrefab, shelfSpots[i].position, shelfSpots[i].rotation);
            ProductDisplay display = productInstance.GetComponent<ProductDisplay>();
            display.Initialize(product, this);
        }
    }

    private void ClearShelf()
    {
        foreach (Transform spot in shelfSpots)
        {
            foreach (Transform child in spot)
            {
                Destroy(child.gameObject);
            }
        }
    }

    // Show product details on the Canvas when a product is clicked
    public void ShowProductDetailsOnCanvas(Product product)
    {
        currentProduct = product;

        // Activate the Canvas and set input fields to current product details
        productDetailsCanvas.SetActive(true);
        productNameInputField.text = product.name;
        productDescriptionInputField.text = product.description;
        productPriceInputField.text = product.price.ToString("F2");

        // Add listeners to save changes
        productNameInputField.onEndEdit.AddListener(UpdateProductName);
        productDescriptionInputField.onEndEdit.AddListener(UpdateProductDescription);
        productPriceInputField.onEndEdit.AddListener(UpdateProductPrice);
    }

    private void UpdateProductName(string newName)
    {
        currentProduct.name = newName;
    }

    private void UpdateProductDescription(string newDescription)
    {
        currentProduct.description = newDescription;
    }

    private void UpdateProductPrice(string newPrice)
    {
        if (float.TryParse(newPrice, out float price))
        {
            currentProduct.price = price;
        }
    }

    [System.Serializable]
    public class ProductListWrapper
    {
        public List<Product> products;
    }
}
