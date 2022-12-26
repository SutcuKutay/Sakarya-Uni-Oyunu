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
    public DatabaseReference fakulteRef;
    public TMP_InputField mailInput, passInput;

    static public string statu;

    static public string bilgisayarFakulteIsim;
    static public string bilgisayarFakulteSinifSayisi;
    static public string bilgisayarFakulteOgrenciSayisi;

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
            fakulteRef = FirebaseDatabase.DefaultInstance.GetReference("SAU");
            Debug.Log("SAU veritabanina baglanildi.");

            StartCoroutine(GetBilgisayarData());
            Debug.Log("Bilgisayar Fakultesi Bilgileri Cekildi.");
        }

        else
        {
            Debug.LogError("Database Hatasý: ");
        }
    }

    public IEnumerator GetBilgisayarData()
    {
        var task = fakulteRef.Child("Fakulteler").GetValueAsync();
        while (!task.IsCompleted)
        {
            yield return null;
        }

        if(task.IsCanceled || task.IsFaulted)
        {
            Debug.LogError("Database Hatasý: " + task.Exception);
        }

        DataSnapshot snapshot = task.Result;

        bilgisayarFakulteIsim = snapshot.Child("1").Child("FakulteAdi").Value.ToString();
        bilgisayarFakulteOgrenciSayisi = snapshot.Child("1").Child("OgrenciSayisi").Value.ToString();
        bilgisayarFakulteSinifSayisi = snapshot.Child("1").Child("SinifSayisi").Value.ToString();
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