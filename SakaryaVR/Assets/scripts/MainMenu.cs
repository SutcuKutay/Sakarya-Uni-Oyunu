using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public AudioSource music;

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Oyundan Çýkýldý.");
    }

    public void GuestEnter()
    {
        SceneManager.LoadScene("Game");
        Debug.Log("Misafir olarak giriþ yapýldý.");
    }

    public void FullScreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }

    public void Music(bool isPlay)
    {
        music.mute = !isPlay;
    }
}
