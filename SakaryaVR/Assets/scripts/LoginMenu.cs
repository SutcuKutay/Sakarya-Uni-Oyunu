using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginMenu : MonoBehaviour
{
    string mail = "admin@gmail.com";
    string password = "123";
    public TMP_InputField mailInput;
    public TMP_InputField passInput;

    public void Enter()
    {
        if(mailInput.text == mail && passInput.text == password)
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            Debug.Log("Hatalý Giriþ");
        }
    }
}
