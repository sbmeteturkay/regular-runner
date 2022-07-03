using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public int stack = 0;
    public int startStack;
    private int maxStack=20;
    public float animationWeight;
    public Animator playerAnimator;
    public Slider stackSlider;
    //menu manager start game() reference
    public static bool moving = false;

    public int currencyAmount = 0;
    public TMPro.TMP_Text currencyText,stackPrice,currentCollectedCoin,playerStackUI;
    public int stackUpgradePrice = 10;
    public int currentCollectedCurrency = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(gameManager==null)
            gameManager = this;
        #region Player pref check
        if (!PlayerPrefs.HasKey("stackUpgradePrice"))
        {
            PlayerPrefs.SetInt("stackUpgradePrice",10);
        }
        if (!PlayerPrefs.HasKey("currency"))
        {
            PlayerPrefs.SetInt("currency",0);
        }
        if (!PlayerPrefs.HasKey("startStack"))
        {
            PlayerPrefs.SetInt("startStack", 0);
        }
        #endregion
        stackUpgradePrice = PlayerPrefs.GetInt("stackUpgradePrice");
        startStack = PlayerPrefs.GetInt("startStack");
        currencyAmount = PlayerPrefs.GetInt("currency");
        currencyText.text = currencyAmount.ToString();
        stackPrice.text = stackUpgradePrice.ToString();
        stack += startStack;
        stackSlider.value = stack;
    }
    public void IncreaseCurrencyAmount(int i)
    {
        currencyAmount += i;
        PlayerPrefs.SetInt("currency", currencyAmount);
        currencyText.text = currencyAmount.ToString();
        currentCollectedCurrency += i;
        currentCollectedCoin.text = currentCollectedCurrency.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            playerAnimator.SetBool("running", true);
            stackSlider.gameObject.SetActive(true);
        }
        else
        {
            playerAnimator.SetBool("running", false);
            stackSlider.gameObject.SetActive(false);
        }
        if (MenuManager.MenuManagerInstance.GameState)
        {
            animationWeight = (float)stack / (float)maxStack;
            playerAnimator.SetLayerWeight(1, animationWeight);
            stackSlider.value = stack;
        }
        else
        {
            playerAnimator.SetLayerWeight(1, 0);
        }

    }
    public void CheckPrice()
    {
        stackUpgradePrice = PlayerPrefs.GetInt("stackUpgrade");
        stackPrice.text = stackUpgradePrice.ToString();
        currencyAmount = PlayerPrefs.GetInt("currency");
        currencyText.text = currencyAmount.ToString();
    }
    public void SetStack(int number)
    {
        stack = number;
    }
    public void UpdateStack()
    {
        playerStackUI.text = stack.ToString();
    }
    public void IncreaseStack(int number)
    {
        stack += number;
    }
    public void DecreaseStack(int number)
    {
        if (stack - number < 0)
        {
            stack = 0;
        }
        else
        {
            stack -= number;
        }
    }
    public int GetStack()
    {
        return stack;
    }
    public void UpgradeStartStack()
    {
        if(PlayerPrefs.GetInt("currency")>= PlayerPrefs.GetInt("stackUpgradePrice")){
            PlayerPrefs.SetInt("currency", PlayerPrefs.GetInt("currency") - PlayerPrefs.GetInt("stackUpgradePrice"));
            startStack += 1;
            PlayerPrefs.SetInt("startStack", startStack);
            stack = startStack;
            stackSlider.value = stack;
            PlayerPrefs.SetInt("stackUpgradePrice", PlayerPrefs.GetInt("stackUpgradePrice") + 10);
            CheckPrice();
        }

    }
}
