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
        binaIsmiText.text = "Bina �smi: " + DBMenager.bilgisayarFakulteIsim;
        ogrenciSayisiText.text = "��renci Say�s�: " + DBMenager.bilgisayarFakulteOgrenciSayisi;
        sinifSayisiText.text = "S�n�f Say�s�: " + DBMenager.bilgisayarFakulteSinifSayisi;
    }
}
