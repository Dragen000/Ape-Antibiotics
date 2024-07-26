using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    #region Singleton
    public static HUDManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }
    #endregion

    [SerializeField] private GameObject hudManager;
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject instructionHolder;

    //============================= UI Manager ====================================
    // Start is called before the first frame update
    private void Start()
    {
        if (hudManager != null)
        {
            ShowHud();
            HideHint();
        }
    }

    public void ShowHud()
    {
        hudManager.SetActive(true);
        win.SetActive(false);
        gameOver.SetActive(false);
    }

    public void ShowWin()
    {
        hudManager.SetActive(true);
        win.SetActive(true);
        gameOver.SetActive(false);
    }

    public void ShowGameOver()
    {
        hudManager.SetActive(true);
        win.SetActive(false);
        gameOver.SetActive(true);
    }

    public void ShowCurrentHint(Sprite newImg)
    {
        Image hudImage = instructionHolder.GetComponent<Image>();
        hudImage.sprite = newImg;
        hudImage.SetNativeSize();
        instructionHolder.SetActive(true);
        
    }

    public void HideHint()
    {
        instructionHolder.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
