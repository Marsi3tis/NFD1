using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaleMenuUI : MonoBehaviour
{
    public static SaleMenuUI Instance { get; private set; }

    [Header("Panels")]
    public GameObject menuPanel;
    public GameObject promptObject; // "Press E to sell" text

    [Header("Ganja Row")]
    public TMP_Text ganjaCountText;
    public TMP_Text ganjaPriceText;
    public Slider   ganjaSlider;
    public TMP_Text ganjaSellAmountText;
    public Button   ganjaSellButton;

    [Header("Kr Row")]
    public TMP_Text krCountText;
    public TMP_Text krPriceText;
    public Slider   krSlider;
    public TMP_Text krSellAmountText;
    public Button   krSellButton;

    [Header("Sniegas Row")]
    public TMP_Text sniegasCountText;
    public TMP_Text sniegasPriceText;
    public Slider   sniegasSlider;
    public TMP_Text sniegasSellAmountText;
    public Button   sniegasSellButton;

    [Header("Total")]
    public TMP_Text totalValueText;
    public Button   sellAllSelectedButton;

    private SalePoint currentSalePoint;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        menuPanel.SetActive(false);
        if (promptObject != null) promptObject.SetActive(false);

        // Slider listeners
        ganjaSlider.onValueChanged.AddListener(_ => RefreshUI());
        krSlider.onValueChanged.AddListener(_ => RefreshUI());
        sniegasSlider.onValueChanged.AddListener(_ => RefreshUI());

        // Sell buttons
        ganjaSellButton.onClick.AddListener(() => SellItem(ItemType.Ganja));
        krSellButton.onClick.AddListener(() => SellItem(ItemType.Kr));
        sniegasSellButton.onClick.AddListener(() => SellItem(ItemType.Sniegas));
        sellAllSelectedButton.onClick.AddListener(SellAllSelected);
    }

    public void ShowPrompt(bool show)
    {
        if (promptObject != null)
            promptObject.SetActive(show);
    }

    public void Open(SalePoint salePoint)
    {
        currentSalePoint = salePoint;
        menuPanel.SetActive(true);
        RefreshUI();
    }

    public void Close()
    {
        menuPanel.SetActive(false);
        currentSalePoint = null;
    }

    private void RefreshUI()
    {
        if (currentSalePoint == null) return;

        int ganjaCount   = InventoryManager.Instance.GetCount(ItemType.Ganja);
        int krCount      = InventoryManager.Instance.GetCount(ItemType.Kr);
        int sniegasCount = InventoryManager.Instance.GetCount(ItemType.Sniegas);

        // Update counts
        ganjaCountText.text   = "x" + ganjaCount;
        krCountText.text      = "x" + krCount;
        sniegasCountText.text = "x" + sniegasCount;

        // Update prices
        ganjaPriceText.text   = "€" + currentSalePoint.GetPrice(ItemType.Ganja)   + " each";
        krPriceText.text      = "€" + currentSalePoint.GetPrice(ItemType.Kr)      + " each";
        sniegasPriceText.text = "€" + currentSalePoint.GetPrice(ItemType.Sniegas) + " each";

        // Clamp and update sliders
        ganjaSlider.maxValue   = ganjaCount;
        krSlider.maxValue      = krCount;
        sniegasSlider.maxValue = sniegasCount;

        int ganjaAmt   = Mathf.RoundToInt(ganjaSlider.value);
        int krAmt      = Mathf.RoundToInt(krSlider.value);
        int sniegasAmt = Mathf.RoundToInt(sniegasSlider.value);

        ganjaSellAmountText.text   = ganjaAmt.ToString();
        krSellAmountText.text      = krAmt.ToString();
        sniegasSellAmountText.text = sniegasAmt.ToString();

        // Total value preview
        float total = (ganjaAmt   * currentSalePoint.GetPrice(ItemType.Ganja))
                    + (krAmt      * currentSalePoint.GetPrice(ItemType.Kr))
                    + (sniegasAmt * currentSalePoint.GetPrice(ItemType.Sniegas));

        totalValueText.text = "Total: €" + total;
    }

    private void SellItem(ItemType type)
    {
        if (currentSalePoint == null) return;

        int amount = type switch
        {
            ItemType.Ganja   => Mathf.RoundToInt(ganjaSlider.value),
            ItemType.Kr      => Mathf.RoundToInt(krSlider.value),
            ItemType.Sniegas => Mathf.RoundToInt(sniegasSlider.value),
            _ => 0
        };

        if (amount <= 0) return;

        currentSalePoint.SellItem(type, amount);

        // Reset slider
        switch (type)
        {
            case ItemType.Ganja:   ganjaSlider.value   = 0; break;
            case ItemType.Kr:      krSlider.value      = 0; break;
            case ItemType.Sniegas: sniegasSlider.value = 0; break;
        }

        RefreshUI();
    }

    private void SellAllSelected()
    {
        SellItem(ItemType.Ganja);
        SellItem(ItemType.Kr);
        SellItem(ItemType.Sniegas);
    }
}
