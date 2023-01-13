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
    public DatabaseReference sauRef;
    public TextMeshProUGUI binaIsmiText;
    public TextMeshProUGUI ogrenciSayisiText;
    public TextMeshProUGUI sinifSayisiText;
    public TextMeshProUGUI bolumBaskaniText;
    public TextMeshProUGUI bolumBaskaniEmailText;
    public TextMeshProUGUI sinifNoText;

    [SerializeField] private TMP_Dropdown demirbaslar;
    private List<string> demirbasListe = new List<string>();

    [SerializeField] private TMP_Dropdown siniflar;
    private List<string> sinifListe = new List<string>();

    public AudioSource footsteps;

    private string bilgisayarFakulteIsimi;
    private string bilgisayarFakulteOgrenciSayisi;
    private string bilgisayarFakulteSinifSayisi;
    private string statuAdmin = "2";
    private string bilgisayarFakultesi = "1";



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

        DataSnapshot snapshot1 = gorev.Result;


        bilgisayarFakulteIsimi = snapshot1.Child("1").Child("FakulteAdi").Value.ToString();
        bilgisayarFakulteOgrenciSayisi = snapshot1.Child("1").Child("OgrenciSayisi").Value.ToString();
        bilgisayarFakulteSinifSayisi = snapshot1.Child("1").Child("SinifSayisi").Value.ToString();
        //siniflar.ClearOptions();
        //sinifListe.Clear();
        //if (DBMenager.statu == 2.ToString())
        //{
        //    siniflar.enabled = true;
        //    siniflar.gameObject.SetActive(true);
        //    foreach (DataSnapshot yer in snapshot1.Child("1").Child("Yerler").Children)
        //    {
        //        sinifListe.Add(yer.Child("No").Value.ToString());
        //    }
        //    siniflar.AddOptions(sinifListe);
        //    siniflar.RefreshShownValue();
        //}
        //else
        //{
        //    siniflar.enabled = false;
        //    siniflar.gameObject.SetActive(false);
        //}
        //demirbaslar.ClearOptions();
        //demirbasListe.Clear();
        //foreach (DataSnapshot yer in snapshot1.Child("1").Child("Yerler").Children)
        //{
        //    Debug.Log(siniflar.options[siniflar.value].text);
        //    if (siniflar.options[siniflar.value].text == yer.Child("No").Value.ToString())
        //    {
        //        Debug.Log(yer.Child("No").Value.ToString());
        //        sinifNoText.text = yer.Child("No").Value.ToString();
        //        foreach (DataSnapshot demirbas in yer.Child("Demirbaslar").Children)
        //        {
        //            demirbasListe.Add(demirbas.Child("DemirbasNo").Value.ToString() + " " + demirbas.Child("Turu").Value.ToString());
        //            Debug.Log(demirbas.Child("DemirbasNo").Value.ToString() + " " + demirbas.Child("Turu").Value.ToString());
        //        }
        //    }
        //}
        //demirbaslar.AddOptions(demirbasListe);
        //demirbaslar.RefreshShownValue();

        var gorevIki = fakulteRef.Child("Kullanicilar").GetValueAsync();
        while (!gorevIki.IsCompleted)
        {
            yield return null;
        }

        DataSnapshot snapshot2 = gorevIki.Result;
        foreach (DataSnapshot user in snapshot2.Children)
        {
            if (user.Child("Fakulte").Value.ToString() == bilgisayarFakultesi)
            {
                if (user.Child("Statu").Value.ToString() == statuAdmin)
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
