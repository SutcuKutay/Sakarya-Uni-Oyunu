using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;
using TMPro;

public class DBMenager : MonoBehaviour
{
    public DatabaseReference usersRef;
    public TMP_InputField mailInput, passInput;

    static public string statu;

    void Start()
    {
        StartCoroutine(Initilization());
    }

    private IEnumerator Initilization()
    {
        var task = FirebaseApp.CheckAndFixDependenciesAsync();
        while (!task.IsCompleted)
        {
            yield return null;
        }

        if (task.IsCanceled || task.IsFaulted)
        {
            Debug.LogError("Database Hatasý: " + task.Exception);
        }

        var dependencyStatus = task.Result;

        if (dependencyStatus == DependencyStatus.Available)
        {
            usersRef = FirebaseDatabase.DefaultInstance.GetReference("SAU");
            Debug.Log("SAU veritabanina baglanildi.");
        }

        else
        {
            Debug.LogError("Database Hatasý: ");
        }
    }

    public void GetData()
    {
        StartCoroutine(GetUser());
    }

    public IEnumerator GetUser()
    {
        var task = usersRef.Child("Kullanicilar").GetValueAsync();
        while (!task.IsCompleted)
        {
            yield return null;
        }

        if (task.IsCanceled || task.IsFaulted)
        {
            Debug.LogError("Database Hatasý: " + task.Exception);
        }

        DataSnapshot snapshot = task.Result;

        bool check = false;

        foreach (DataSnapshot user in snapshot.Children)
        {
            if (user.Child("Email").Value.ToString() == mailInput.text)
            {
                check = true;
            }
            else
            {
                Debug.Log("Email adresi bulunamadý.");
            }
            if (check)
            {
                if (user.Child("Sifre").Value.ToString() == passInput.text)
                {
                    statu = user.Child("Statu").Value.ToString();
                    SceneManager.LoadScene("Game");
                }
                else
                {
                    check = false;
                    Debug.Log("Þifre Hatalý.");
                }
            }
        }
    }
}