using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BilgisayarBilgiler : MonoBehaviour
{
    public GameObject bilgisayarBilgiPaneli;
    public TextMeshProUGUI binaIsmiText;
    public TextMeshProUGUI ogrenciSayisiText;
    public TextMeshProUGUI sinifSayisiText;

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
        binaIsmiText.text = "Bina Ýsmi: " + DBMenager.bilgisayarFakulteIsim;
        ogrenciSayisiText.text = "Öðrenci Sayýsý: " + DBMenager.bilgisayarFakulteOgrenciSayisi;
        sinifSayisiText.text = "Sýnýf Sayýsý: " + DBMenager.bilgisayarFakulteSinifSayisi;
    }
}
