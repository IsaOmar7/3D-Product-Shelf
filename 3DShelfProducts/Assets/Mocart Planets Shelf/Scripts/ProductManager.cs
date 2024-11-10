using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


/// <summary>
/// Represents a product with a prefab, name, description, and price. the rpoducts from the servers
/// </summary>
[System.Serializable]
public class Product
{
    public GameObject prefab;
    public string name;
    public string description;
    public float price;
}

/// <summary>
/// This class Manages products, including fetching from server, displaying on shelf, and showing details on UI.
/// </summary>
public class ProductManager : MonoBehaviour
{

    public GameObject[] productPrefab; 
    public Transform[] shelfSpots;
    // *** UI elements ***
    public GameObject productDetailsCanvas;
    public GameObject submitChangesCanvas;
    public TMP_InputField productNameInputField;
    public TextMeshProUGUI productDescriptionText;
    public TMP_InputField productPriceInputField;
    private string serverUrl = "https://homework.mocart.io/api/products";
    private Product currentProduct;

    /// <summary>
    /// setting quality based on graphics memory and hiding the submit canvas. for mobile and desktop quality
    /// </summary>
    void Start()
    {
        if (SystemInfo.graphicsMemorySize < 2000)
        { 
            QualitySettings.SetQualityLevel(0); 
        }
        else
        {
            QualitySettings.SetQualityLevel(2); 
        }
        submitChangesCanvas.SetActive(false);
    }

    /// <summary>
    /// Called when the "Show Products" button is clicked. Begins fetching products from server.
    /// </summary>
    public void OnShowProductsButtonClicked()
    {
        StartCoroutine(FetchProductsFromServer());
    }

    /// <summary>
    /// Called when the "Submit" button in the canvas is clicked. Shows a confirmation message.
    /// </summary>
    public void OnSubmitButtonClicked()
    {
        StartCoroutine(ShowConfirmationMessage());
    }

    /// <summary>
    /// Shows a confirmation message for submitted changes, hides it after a 3 seconds.
    /// </summary>
    /// <returns>IEnumerator for coroutine</returns>
    private IEnumerator ShowConfirmationMessage()
    {
        productDetailsCanvas.SetActive(false);
        submitChangesCanvas.SetActive(true);
        yield return new WaitForSeconds(3f);
        submitChangesCanvas.SetActive(false);
        productDetailsCanvas.SetActive(true);
    }


    /// <summary>
    /// Fetches products from the server and handles the response.
    /// </summary>
    /// <returns>IEnumerator for coroutine</returns>
    private IEnumerator FetchProductsFromServer()
    {
        UnityWebRequest request = UnityWebRequest.Get(serverUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            try
            {
                string jsonResponse = request.downloadHandler.text;
                List<Product> products = ParseProductList(jsonResponse);
                if (products != null)
                {
                    DisplayProductsOnShelf(products);
                }
                else
                {
                    Debug.LogError("Failed to parse products from server response.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error parsing response: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("Error fetching products: " + request.error);
        }
    }

    /// <summary>
    /// Parses JSON response to retrieve a list of products called from FetchProductsFromServer.
    /// </summary>
    /// <param name="jsonResponse">JSON response from server</param>
    /// <returns>List of products or null if parsing fails</returns>
    private List<Product> ParseProductList(string jsonResponse)
    {
        try
        {
            ProductListWrapper productListWrapper = JsonUtility.FromJson<ProductListWrapper>(jsonResponse);
            return productListWrapper?.products;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Parsing JSON failed: " + ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Displays products on shelf spots, creating and positioning prefabs based on spots I gave it from the scene.
    /// and color the spots who filled in green for product existing there.
    /// </summary>
    /// <param name="products">List of products to display</param>
    private void DisplayProductsOnShelf(List<Product> products)
    {

        ClearShelf();

        for (int i = 0; i < products.Count && i < shelfSpots.Length; i++)
        {
            Product product = products[i];
            string[] parts = product.name.Split(' ');
            int productNumber;

            if (parts.Length > 1 && int.TryParse(parts[1], out productNumber) && productNumber > 0 && productNumber <= productPrefab.Length)
            {
                GameObject productInstance = Instantiate(productPrefab[productNumber - 1], shelfSpots[i].position, shelfSpots[i].rotation, shelfSpots[i]);
                shelfSpots[i].GetComponent<Light>().color = Color.green;
                productInstance.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                ProductDisplay display = productInstance.GetComponent<ProductDisplay>();
                display.Initialize(product, this);
            }
            else
            {
                Debug.LogError($"Invalid product format or prefab index for {product.name}");
            }
        }
        // Set remaining shelf spots to red if there is no products in the spot
        for (int i = products.Count; i < 3 && i < shelfSpots.Length; i++)
        {
            shelfSpots[i].GetComponent<Light>().color = Color.red;
        }
    }
    /// <summary>
    /// Clears all products currently displayed on the shelf.
    /// </summary>
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

    /// <summary>
    /// Shows the product details on the details canvas.
    /// </summary>
    /// <param name="product">Product to display details for</param>
    public void ShowProductDetailsOnCanvas(Product product)
    {
        currentProduct = product;
        productDetailsCanvas.SetActive(true);
        productNameInputField.text = product.name;
        productDescriptionText.text = currentProduct.description;
        productPriceInputField.text = product.price.ToString("F2");

        productNameInputField.onEndEdit.AddListener(UpdateProductName);
        productPriceInputField.onEndEdit.AddListener(UpdateProductPrice);
    }

    /// *** Setters ***
    /// <summary>
    /// Updates the product name based on user input.
    /// </summary>
    /// <param name="newName">New product name</param>
    private void UpdateProductName(string newName)
    {
        currentProduct.name = newName;
        OnSubmitButtonClicked();
    }

    /// <summary>
    /// Updates the product description based on user input.
    /// </summary>
    /// <param name="newDescription">New product description</param>
    private void UpdateProductDescription(string newDescription)
    {
        currentProduct.description = newDescription;
        OnSubmitButtonClicked();
    }

    /// <summary>
    /// Updates the product price based on user input.
    /// </summary>
    /// <param name="newPrice">New product price</param>
    private void UpdateProductPrice(string newPrice)
    {
        if (float.TryParse(newPrice, out float price))
        {
            currentProduct.price = price;
            OnSubmitButtonClicked();
        }
    }

    /// <summary>
    /// Wrapper class for deserializing product list JSON response.
    /// </summary>
    [System.Serializable]
    public class ProductListWrapper
    {
        public List<Product> products;
    }
}
