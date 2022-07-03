using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuManager : MonoBehaviour
{
    public static MenuManager MenuManagerInstance;
    public bool GameState;
    public GameObject startMenuItem,finishMenuItem;
    public Animator cameraAnimator;
    public GameObject LevelParent;
    public TMPro.TMP_Text startLevelText;
    Transform path;
    Vector3 transformLevel;
    // Start is called before the first frame update
    void Start()
    {
        SetLevelText();
        GameState = false;
        if(MenuManagerInstance==null)
            MenuManagerInstance = this;
        LevelParent.transform.GetChild(0).gameObject.SetActive(false);
        if (LevelParent.transform.childCount > PlayerPrefs.GetInt("level"))
        {
            LevelParent.transform.GetChild(PlayerPrefs.GetInt("level") - 1).gameObject.SetActive(true);
        }
        else
        {
            LevelParent.transform.GetChild(PlayerPrefs.GetInt("level") % LevelParent.transform.childCount).gameObject.SetActive(true);
        }
        transformLevel = LevelParent.transform.position;
    }
    public void StartGame()
    {
        GameState = true; ;
        startMenuItem.SetActive(false);
        GameManager.moving = true;
    }
    public void FinishGame()
    {
        finishMenuItem.SetActive(true);
        cameraAnimator.enabled = true;
        cameraAnimator.SetTrigger("finish");
    }
    public void SetLevelText()
    {
        if (!PlayerPrefs.HasKey("level"))
        {
            PlayerPrefs.SetInt("level", 1);
        }
        startLevelText.text = "Level " + PlayerPrefs.GetInt("level").ToString();
    }
    public void NextLevel()
    {
        if (LevelParent.transform.childCount > PlayerPrefs.GetInt("level"))
        {
            LevelParent.transform.GetChild(PlayerPrefs.GetInt("level") - 1).gameObject.SetActive(false);
            for (int i = 0; i < LevelParent.transform.GetChild(PlayerPrefs.GetInt("level") - 1).gameObject.transform.GetChild(1).childCount; i++)
            {
                LevelParent.transform.GetChild(PlayerPrefs.GetInt("level") - 1).gameObject.transform.GetChild(1).transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else
        {
            LevelParent.transform.GetChild(PlayerPrefs.GetInt("level") % LevelParent.transform.childCount).gameObject.SetActive(false);
            for (int i = 0; i < LevelParent.transform.GetChild(PlayerPrefs.GetInt("level") % LevelParent.transform.childCount).gameObject.gameObject.transform.GetChild(1).childCount; i++)
            {
                LevelParent.transform.GetChild(PlayerPrefs.GetInt("level") % LevelParent.transform.childCount).gameObject.gameObject.transform.GetChild(1).transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        if (LevelParent.transform.childCount>PlayerPrefs.GetInt("level"))
        {
            LevelParent.transform.GetChild(PlayerPrefs.GetInt("level") - 1).gameObject.SetActive(true);
        }
        else
        {
            LevelParent.transform.GetChild(PlayerPrefs.GetInt("level")% LevelParent.transform.childCount).gameObject.SetActive(true);
        }
        SetLevelText();
        finishMenuItem.SetActive(false);
        cameraAnimator.SetTrigger("reset");
        GameManager.moving = false;
        GameManager.gameManager.playerAnimator.SetTrigger("reset");
        startMenuItem.SetActive(true);
        GameObject.FindGameObjectWithTag("Respawn").transform.position = transformLevel;
        GameManager.gameManager.stack =GameManager.gameManager.startStack;

    }
}
