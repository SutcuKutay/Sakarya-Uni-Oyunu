using System;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using System.Collections;

public class theology : MonoBehaviour
{
    public GameObject ilahiyatBilgiPaneli;
    public DatabaseReference fakulteRef;
    public TextMeshProUGUI binaIsmiText;
    public TextMeshProUGUI ogrenciSayisiText;
    public TextMeshProUGUI sinifSayisiText;
    public TextMeshProUGUI bolumBaskaniText;
    public TextMeshProUGUI bolumBaskaniEmailText;

    private string ilahiyatFakulteIsimi;
    private string ilahiyatFakulteOgrenciSayisi;
    private string ilahiyatFakulteSinifSayisi;
    private string statuAdmin = "2";
    private string ilahiyatFakultesi = "3";



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
        ilahiyatBilgiPaneli.SetActive(true);
        Time.timeScale = 0f;
    }

    void UpdatePanel()
    {
        GetTheologyData();
    }

    public void GetTheologyData()
    {
        StartCoroutine(GetIlahiyatData());
        Debug.Log("Ilahiyat Fakultesi Bilgileri Cekildi.");
    }

    public IEnumerator GetIlahiyatData()
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

        DataSnapshot snapshot1 = gorev.Result;


        ilahiyatFakulteIsimi = snapshot1.Child("3").Child("FakulteAdi").Value.ToString();
        ilahiyatFakulteOgrenciSayisi = snapshot1.Child("3").Child("OgrenciSayisi").Value.ToString();
        ilahiyatFakulteSinifSayisi = snapshot1.Child("3").Child("SinifSayisi").Value.ToString();

        var gorevIki = fakulteRef.Child("Kullanicilar").GetValueAsync();
        while (!gorevIki.IsCompleted)
        {
            yield return null;
        }

        DataSnapshot snapshot2 = gorevIki.Result;
        foreach (DataSnapshot user in snapshot2.Children)
        {
            if (user.Child("Fakulte").Value.ToString() == ilahiyatFakultesi)
            {
                if (user.Child("Statu").Value.ToString() == statuAdmin)
                {
                    bolumBaskaniText.text = "Bölüm Baþkaný: " + user.Child("Kisi").Value.ToString();
                    bolumBaskaniEmailText.text = "Bölüm Baþkaný Email: " + user.Child("Email").Value.ToString();
                }
            }
        }
        binaIsmiText.text = "Bina Ýsim: " + ilahiyatFakulteIsimi;
        ogrenciSayisiText.text = "Öðrenci Sayýsý: " + ilahiyatFakulteOgrenciSayisi;
        sinifSayisiText.text = "Sýnýf Sayýsý: " + ilahiyatFakulteSinifSayisi;
    }
}
