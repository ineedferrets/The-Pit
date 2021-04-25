using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript_Marko : MonoBehaviour
{
    public InputField Username;


    public MeshRenderer MeshRenderer;

    public Slider RedSlider;
    public Slider GreenSlider;
    public Slider BlueSlider;

    public void StartGame()
    {
        GameLogicScript_Marko.Instance.PlayerName = Username.text;
        SceneManager.LoadScene("GameScene");
    }

    public void SetColor()
    {
        Color c = new Color(RedSlider.value, GreenSlider.value, BlueSlider.value, 1);
        MeshRenderer.material.color = c;
        GameLogicScript_Marko.Instance.PlayerColor = c;
    }
}
