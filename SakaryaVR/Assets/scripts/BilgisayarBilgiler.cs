using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using System.Threading;

public class BilgisayarBilgiler : MonoBehaviour
{
    public GameObject bilgisayarBilgiPaneli;
    public DatabaseReference fakulteRef;
    public TextMeshProUGUI binaIsmiText;
    public TextMeshProUGUI ogrenciSayisiText;
    public TextMeshProUGUI sinifSayisiText;
    public TextMeshProUGUI bolumBaskaniText;
    public TextMeshProUGUI bolumBaskaniEmailText;

    private string bilgisayarFakulteIsimi;
    private string bilgisayarFakulteOgrenciSayisi;
    private string bilgisayarFakulteSinifSayisi;



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowPanel();
            UpdatePanel();
        }
    }

    void ShowPanel()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        bilgisayarBilgiPaneli.SetActive(true);
        Time.timeScale = 0f;
    }

    void UpdatePanel()
    {
        GetPCData();
    }

    public void GetPCData()
    {
        StartCoroutine(GetBilgisayarData());
        Debug.Log("Bilgisayar Fakultesi Bilgileri Cekildi.");
    }

    public IEnumerator GetBilgisayarData()
    {
        fakulteRef = FirebaseDatabase.DefaultInstance.GetReference("SAU");

        var gorev = fakulteRef.Child("Fakulteler").GetValueAsync();
        while (!gorev.IsCompleted)
        {
            yield return null;
        }

        if (gorev.IsCanceled || gorev.IsFaulted)
        {
            Debug.LogError("Database Hatasý: " + gorev.Exception);
        }

        DataSnapshot kutay = gorev.Result;


        bilgisayarFakulteIsimi = kutay.Child("1").Child("FakulteAdi").Value.ToString();
        bilgisayarFakulteOgrenciSayisi = kutay.Child("1").Child("OgrenciSayisi").Value.ToString();
        bilgisayarFakulteSinifSayisi = kutay.Child("1").Child("SinifSayisi").Value.ToString();
        
        var gorevIki = fakulteRef.Child("Kullanicilar").GetValueAsync();
        while (!gorevIki.IsCompleted)
        {
            yield return null;
        }

        DataSnapshot ibrahim = gorevIki.Result;
        bool check = false; 
        foreach (DataSnapshot user in ibrahim.Children)
        {

            if (user.Child("Statu").Value.ToString() == 2.ToString())
            {
                check = true;
            }
            if (check)
            {
                if (user.Child("Fakulte").Value.ToString() == 1.ToString())
                {
                    bolumBaskaniText.text = "Bölüm Baþkaný: " + user.Child("Kisi").Value.ToString();
                    bolumBaskaniEmailText.text = "Bölüm Baþkaný Email: " + user.Child("Email").Value.ToString();
                }
            }
        }
        binaIsmiText.text = "Bina Ýsim: " + bilgisayarFakulteIsimi;
        ogrenciSayisiText.text = "Öðrenci Sayýsý: " + bilgisayarFakulteOgrenciSayisi;
        sinifSayisiText.text = "Sýnýf Sayýsý: " + bilgisayarFakulteSinifSayisi;
    }
}
