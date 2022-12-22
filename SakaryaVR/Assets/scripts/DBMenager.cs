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
    public string statu;
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
            Debug.LogError("Database Hatas�: " + task.Exception);
        }

        var dependencyStatus = task.Result;

        if (dependencyStatus == DependencyStatus.Available)
        {
            usersRef = FirebaseDatabase.DefaultInstance.GetReference("SAU");
            Debug.Log("SAU veritabanina baglanildi.");

        }

        else
        {
            Debug.LogError("Database Hatas�: ");
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
            Debug.LogError("Database Hatas�: " + task.Exception);
        }

        DataSnapshot snapshot = task.Result;

        bool check = false;

        foreach (DataSnapshot user in snapshot.Children)
        {
            if (user.Child("Email").Value.ToString() == mailInput.text)
            {
                check = true;
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
                }
            }

        }
        if (!check)
        {
            Debug.Log("Email ya da �ifre Hatal�!");
        }
    }
}