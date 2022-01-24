using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Shopping : MonoBehaviour
{
    //Các sprite tên khác nhau
    [SerializeField]
    private Sprite[] arrowSprites = new Sprite[6];
    [SerializeField]
    //Giá tiền
    private int[] arrowCost = new int[6];
    [SerializeField]
    //Nút mua
    private GameObject[] BuyButtons = new GameObject[6];
    [SerializeField]
    //Nút trang bị
    private GameObject[] EquipButtons = new GameObject[6];
    //Tiền hiện tại
    [SerializeField]
    private TMP_Text currentMoneyTxt;
    //Hình tên hiện tại
    [SerializeField]
    private GameObject currentArrowImage;

    private int currentMoney;
    public int CurrentMoneyDisplay
    {
        get
        {
            return currentMoney;
        }
        set
        {
            currentMoney = value;
            MenuController.Instance.CurrentMoney = currentMoney;
            currentMoneyTxt.text = currentMoney.ToString();
        }
    }


    void Start()
    {
        //Khởi tạo giá ban đầu
        if (MenuController.Instance.CurrentMoney != 0)
        {
            CurrentMoneyDisplay = MenuController.Instance.CurrentMoney;
        }
        else
        {
            CurrentMoneyDisplay = 10000;
        }
        //Xem đã mua tên nào rồi
        if (MenuController.Instance.arrowBought == null || MenuController.Instance.arrowBought.Length == 0)
        {
            MenuController.Instance.arrowBought[0] = true;
            for (int i = 1; i < 6; i++)
            {
                MenuController.Instance.arrowBought[i] = false;
            }
        }
        else
        {
            for (int i = 0; i < MenuController.Instance.arrowBought.Length; i++)
            {
                if (MenuController.Instance.arrowBought[i])
                {
                    BuyButtons[i].SetActive(false);
                    EquipButtons[i].SetActive(true);
                }
            }
        }
        //Xem đang trang bị tên nào
        if (MenuController.Instance.ArrowSprite != null)
        {
            currentArrowImage.GetComponent<Image>().sprite = MenuController.Instance.ArrowSprite;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuyArrow(Button clickedBtn)
    {
        //Mua tên
        int itemIndex = getBuyIndex(clickedBtn.name);
        CurrentMoneyDisplay -= arrowCost[itemIndex];
        BuyButtons[itemIndex].SetActive(false);
        EquipButtons[itemIndex].SetActive(true);
        MenuController.Instance.arrowBought[itemIndex] = true;
        Debug.Log(MenuController.Instance.CurrentMoney);
    }

    public void EquipArrow(Button clickedBtn)
    {
        //Trang bị tên đã mua
        int itemIndex = getBuyIndex(clickedBtn.name);
        currentArrowImage.GetComponent<Image>().sprite = arrowSprites[itemIndex];
        MenuController.Instance.ArrowSprite = arrowSprites[itemIndex];
    }

    private int getBuyIndex(string buttonPress)
    {
        int index = int.Parse(buttonPress.Substring(buttonPress.Length - 1));
        return index;
    }



}
