using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.Networking;
using TMPro;



[System.Serializable]
public class Product
{
    public GameObject prefab;
    public string name;
    public string description;
    public float price;
}


public class ProductManager : MonoBehaviour
{

    public GameObject[] productPrefab; // Prefab for displaying each product on the shelf
    public Transform[] shelfSpots;   // Array of 3 shelf spots

    // References for the Product Details Canvas
    public GameObject productDetailsCanvas;
    public TMP_InputField productNameInputField;
    public TextMeshProUGUI productDescriptionText;
    public TMP_InputField productPriceInputField;

    private string serverUrl = "https://homework.mocart.io/api/products";
    private Product currentProduct;  // The product being displayed on the Canvas


    public void OnShowProductsButtonClicked()
    {
        Debug.Log("Show Products Button Clicked");  // Log for button click


        StartCoroutine(FetchProductsFromServer());
    }

    public void OnSubmitButtonClicked()
    {
        // Display a message indicating that the fields have changed
        Debug.Log("Product details updated!");

        // Show a confirmation message to the user
        StartCoroutine(ShowConfirmationMessage("Product details updated!"));
    }

    private IEnumerator ShowConfirmationMessage(string message)
    {
        // Create a temporary TextMeshProUGUI for the confirmation message
        GameObject messageObject = new GameObject("ConfirmationMessage");
        TextMeshProUGUI messageText = messageObject.AddComponent<TextMeshProUGUI>();

        // Set the message text and style
        messageText.text = message;
        messageText.fontSize = 24; // Set font size
        messageText.alignment = TextAlignmentOptions.Center; // Center the text
        messageText.color = Color.green; // Set text color

        // Set the RectTransform properties
        RectTransform rectTransform = messageObject.GetComponent<RectTransform>();
        rectTransform.SetParent(productDetailsCanvas.transform); // Set parent to the Canvas
        rectTransform.localPosition = new Vector3(0, -50, 0); // Position it below the input fields
        rectTransform.sizeDelta = new Vector2(200, 50); // Set size

        // Show the message for a few seconds
        yield return new WaitForSeconds(2f);

        // Destroy the message object
        Destroy(messageObject);
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
        Debug.Log(products); 
        ClearShelf();

        for (int i = 0; i < products.Count && i < shelfSpots.Length; i++)
        {
            Product product = products[i];
            GameObject productInstance = Instantiate(productPrefab[i], shelfSpots[i].position, shelfSpots[i].rotation, shelfSpots[i]);
            ProductDisplay display = productInstance.GetComponent<ProductDisplay>();
            display.Initialize(product, this);
        }
    }

    private void ClearShelf()
    {
        Debug.Log("Clear Shelf Func");
        foreach (Transform spot in shelfSpots)
        {
            Debug.Log("inside the first for");
            Debug.Log(spot);
            foreach (Transform child in spot)
            {
                Debug.Log("inside the second for");
                Destroy(child.gameObject);
            }
        }
    }

    // Show product details on the Canvas when a product is clicked
    public void ShowProductDetailsOnCanvas(Product product)
    {
        currentProduct = product;
        Debug.Log(product);
        Debug.Log(product.name);

        // Activate the Canvas and set input fields to current product details
        productDetailsCanvas.SetActive(true);
        productNameInputField.text = product.name;
        productDescriptionText.text = currentProduct.description;
        productPriceInputField.text = product.price.ToString("F2");

        // Add listeners to save changes
        productNameInputField.onEndEdit.AddListener(UpdateProductName);
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
