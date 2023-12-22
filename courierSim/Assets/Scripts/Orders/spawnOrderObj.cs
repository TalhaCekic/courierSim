using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class spawnOrderObj : MonoBehaviour
{
    public Image[] pp;

    public string CustomerName;
    public TMP_Text CustomerNameText;

    public int itemNumber;
    public TMP_Text itemNumberText;
    public Image acceptOrderImage;

    public int SelectedOrderTime;
    public float orderTime;
    public TMP_Text orderTimeText;
    public Slider timeSlider;

    public int OrderPrice;
    public int TipPrice;
    public TMP_Text orderPriceText;
    

    void Start()
    {
        orderSpawn();
    }

    private void Update()
    {
        if (OrderManager.instance.isOrderStart )
        {
            orderTime -= Time.deltaTime;
            timeSlider.value = orderTime;
            if(orderTime <= 15)
            {
                acceptOrderImage.color = Color.red;
            }
            else
            {
                acceptOrderImage.color = Color.green;
            }
        }
    }

    void orderSpawn()
    {
        //random pp seçimi
        int randomIndex = Random.Range(0, pp.Length);
        for (int i = 0; i < pp.Length; i++)
        {
            pp[randomIndex].gameObject.SetActive(true);
        }
        //  isim seçimi
        CustomerName = OrderManager.instance.orderName;
        CustomerNameText.text = CustomerName;
        
        // item sayısının seçimi
        itemNumber = OrderManager.instance.orderPriceIndex +1;
        itemNumberText.text = itemNumber.ToString() + " Item";
        
        // satış bedeli seçimi ve tip ücreti
        OrderPrice = OrderManager.instance.orderPrice;
        TipPrice = OrderManager.instance.tipPrice;
        orderPriceText.text = "$ " + OrderPrice.ToString();
        
        // sipariş süresi seçimi
        SelectedOrderTime = OrderManager.instance.orderTime;
        orderTime = SelectedOrderTime;
        timeSlider.maxValue = orderTime;
        timeSlider.value = orderTime;
        orderTimeText.text = SelectedOrderTime.ToString() + " Second";
    }
    //randomize aray sistemi
    private T GetRandomElement<T>(T[] array)
    {
        if (array != null && array.Length > 0)
        {
            int randomIndex = Random.Range(0, array.Length);
            return array[randomIndex];
        }
        else
        {
            return default(T);
        }
    }

    public void StartOrder()
    {
        OrderManager.instance.isOrderStart = true;
    }
}