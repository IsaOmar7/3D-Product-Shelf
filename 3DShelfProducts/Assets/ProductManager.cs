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
    public GameObject submitChangesCanvas;
    public TMP_InputField productNameInputField;
    public TextMeshProUGUI productDescriptionText;
    public TMP_InputField productPriceInputField;

    private string serverUrl = "https://homework.mocart.io/api/products";
    private Product currentProduct;  // The product being displayed on the Canvas

    void Start()
    {
        submitChangesCanvas.SetActive(false);
    }

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
        StartCoroutine(ShowConfirmationMessage());
    }

    private IEnumerator ShowConfirmationMessage()
    {
        productDetailsCanvas.SetActive(false);
        submitChangesCanvas.SetActive(true);

        // Show the message for a few seconds
        yield return new WaitForSeconds(3f);

       
        submitChangesCanvas.SetActive(false);
        productDetailsCanvas.SetActive(true);
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
            string[] parts = product.name.Split(' ');
            string productNumber = parts.Length > 1 ? parts[1] : "0";
            int prefabIndex = int.Parse(productNumber);
            GameObject productInstance = Instantiate(productPrefab[prefabIndex-1], shelfSpots[i].position, shelfSpots[i].rotation, shelfSpots[i]);
            shelfSpots[i].GetComponent<Light>().color = Color.green;
            productInstance.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            ProductDisplay display = productInstance.GetComponent<ProductDisplay>();
            display.Initialize(product, this);
        }
        for (int i = products.Count; i < 3 && i < shelfSpots.Length; i++)
        {
            shelfSpots[i].GetComponent<Light>().color = Color.red;
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
        OnSubmitButtonClicked();
    }

    private void UpdateProductDescription(string newDescription)
    {
        currentProduct.description = newDescription;
        OnSubmitButtonClicked();
    }

    private void UpdateProductPrice(string newPrice)
    {
        if (float.TryParse(newPrice, out float price))
        {
            currentProduct.price = price;
            OnSubmitButtonClicked();
        }
    }

    [System.Serializable]
    public class ProductListWrapper
    {
        public List<Product> products;
    }
}
