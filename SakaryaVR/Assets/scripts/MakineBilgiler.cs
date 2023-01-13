using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using System.Threading;

public class MakineBilgiler : MonoBehaviour
{
    public GameObject makineBilgiPaneli;
    public DatabaseReference fakulteRef;
    public TextMeshProUGUI binaIsmiText;
    public TextMeshProUGUI ogrenciSayisiText;
    public TextMeshProUGUI sinifSayisiText;
    public TextMeshProUGUI bolumBaskaniText;
    public TextMeshProUGUI bolumBaskaniEmailText;

    public AudioSource footsteps;

    private string makineFakulteIsimi;
    private string makineFakulteOgrenciSayisi;
    private string makineFakulteSinifSayisi;
    private string statuAdmin = "2";
    private string makineFakultesi = "2";



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            footsteps.mute = true;
            ShowPanel();
            UpdatePanel();
        }
    }

    public void FootstepSound()
    {
        footsteps.mute = false;
    }

    void ShowPanel()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        makineBilgiPaneli.SetActive(true);
        Time.timeScale = 0f;
    }

    void UpdatePanel()
    {
        GetMachineData();
    }

    public void GetMachineData()
    {
        StartCoroutine(GetMakineData());
        Debug.Log("Makine Fakultesi Bilgileri Cekildi.");
    }

    public IEnumerator GetMakineData()
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


        makineFakulteIsimi = kutay.Child("2").Child("FakulteAdi").Value.ToString();
        makineFakulteOgrenciSayisi = kutay.Child("2").Child("OgrenciSayisi").Value.ToString();
        makineFakulteSinifSayisi = kutay.Child("2").Child("SinifSayisi").Value.ToString();

        var gorevIki = fakulteRef.Child("Kullanicilar").GetValueAsync();
        while (!gorevIki.IsCompleted)
        {
            yield return null;
        }

        DataSnapshot ibrahim = gorevIki.Result;
        foreach (DataSnapshot user in ibrahim.Children)
        {
            if (user.Child("Fakulte").Value.ToString() == makineFakultesi)
            {
                if (user.Child("Statu").Value.ToString() == statuAdmin)
                {
                    bolumBaskaniText.text = "Bölüm Baþkaný: " + user.Child("Kisi").Value.ToString();
                    bolumBaskaniEmailText.text = "Bölüm Baþkaný Email: " + user.Child("Email").Value.ToString();
                }
            }
        }
        binaIsmiText.text = "Bina Ýsim: " + makineFakulteIsimi;
        ogrenciSayisiText.text = "Öðrenci Sayýsý: " + makineFakulteOgrenciSayisi;
        sinifSayisiText.text = "Sýnýf Sayýsý: " + makineFakulteSinifSayisi;
    }
}
