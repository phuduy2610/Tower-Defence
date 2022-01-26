using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Shopping : MonoBehaviour
{
    [SerializeField]
    private Sprite attackIcon;
    [SerializeField]
    private GameObject[] gemIcon;

    [SerializeField]
    private TMP_Text[] costTxt;

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
    [SerializeField]
    private int[] arrowDamage = new int[6];
    public int[] ArrowDamage{
        get{
            return arrowDamage;
        }
        set{
            arrowDamage = value;
        }
    }

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

    public Sprite[] ArrowSprites { get => arrowSprites; set => arrowSprites = value; }
    public GameObject CurrentArrowImage { get => currentArrowImage; set => currentArrowImage = value; }

    public void Setup()
    {
        //Khởi tạo giá ban đầu
        if (MenuController.Instance.CurrentMoney != -1)
        {
            CurrentMoneyDisplay = MenuController.Instance.CurrentMoney;
        }
        //Xem đã mua tên nào rồi
        if (MenuController.Instance.ArrowBought == null || MenuController.Instance.ArrowBought.Length == 0)
        {
            MenuController.Instance.ArrowBought[0] = true;
            for (int i = 1; i < 6; i++)
            {
                MenuController.Instance.ArrowBought[i] = false;
            }
        }
        else
        {
            for (int i = 0; i < MenuController.Instance.ArrowBought.Length; i++)
            {
                if (MenuController.Instance.ArrowBought[i])
                {
                    BuyButtons[i].SetActive(false);
                    EquipButtons[i].SetActive(true);
                    //Thay cost bằng attack
                    costTxt[i].text = arrowDamage[i].ToString();
                    //Thay icon
                    gemIcon[i].GetComponent<Image>().sprite = attackIcon;
                }
            }
        }
        //Xem đang trang bị tên nào
        if (MenuController.Instance.ArrowSprite != null)
        {
            currentArrowImage.GetComponent<Image>().sprite = MenuController.Instance.ArrowSprite;
        }

    }

    public void BuyArrow(Button clickedBtn)
    {
        //Mua tên
        int itemIndex = getBuyIndex(clickedBtn.name);
        if (CurrentMoneyDisplay >= arrowCost[itemIndex])
        {
            CurrentMoneyDisplay -= arrowCost[itemIndex];
            BuyButtons[itemIndex].SetActive(false);
            EquipButtons[itemIndex].SetActive(true);
            //Thay cost bằng attack
            costTxt[itemIndex].text = arrowDamage[itemIndex].ToString();
            //Thay icon
            gemIcon[itemIndex].GetComponent<Image>().sprite = attackIcon;
            MenuController.Instance.ArrowBought[itemIndex] = true;
            Debug.Log(MenuController.Instance.CurrentMoney);
            MenuController.Instance.SavePlayerData();
        }
    }

    public void EquipArrow(Button clickedBtn)
    {
        //Trang bị tên đã mua
        int itemIndex = getBuyIndex(clickedBtn.name);
        currentArrowImage.GetComponent<Image>().sprite = arrowSprites[itemIndex];
        MenuController.Instance.ArrowSprite = arrowSprites[itemIndex];
        MenuController.Instance.WeaponSelected = itemIndex;
        MenuController.Instance.SavePlayerData();
    }

    private int getBuyIndex(string buttonPress)
    {
        int index = int.Parse(buttonPress.Substring(buttonPress.Length - 1));
        return index;
    }
}
