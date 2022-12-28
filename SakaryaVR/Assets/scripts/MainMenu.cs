using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Oyundan ��k�ld�.");
    }

    public void GuestEnter()
    {
        SceneManager.LoadScene("Game");
        Debug.Log("Misafir olarak giri� yap�ld�.");
    }
}
