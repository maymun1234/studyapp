using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Completed : MonoBehaviour
{
    // Ders bilgilerini tutacak sınıf
    [System.Serializable]
    public class DersBilgisi
    {
        public string dersAdi;
        public float harcananSure;
    }

    // Ders bilgilerini depolayacak listemiz
    [SerializeField]
    private List<DersBilgisi> dersBilgileri = new List<DersBilgisi>();

    // Debuggg metodunu güncelleyerek ders ve süre bilgilerini PlayerPrefs'e kaydet
    public void Debuggg()
{
        // Mevcut dersin adını ve süresini PlayerPrefs'ten al
    string currentLesson = PlayerPrefs.GetString("currentLesson");
    float currentDuration = PlayerPrefs.GetFloat("currentDuration");

    // Yeni dersin adını ve süresini PlayerPrefs'ten al
    string newLesson = PlayerPrefs.GetString("newLesson");
    float newDuration = PlayerPrefs.GetFloat("newDuration");

    // Eğer yeni dersin adı mevcut dersin adına eşitse, süreyi toplar
    if (newLesson == currentLesson)
    {
        // Mevcut dersin süresine yeni dersin süresini ekleyerek toplam süreyi hesapla
        float totalDuration = currentDuration + newDuration;

        // Toplam süreyi PlayerPrefs'e kaydet
        PlayerPrefs.SetFloat("currentDuration", totalDuration);
        PlayerPrefs.Save();

        // Debug.Log ile alınan bilgileri konsola yazdır
        Debug.Log("Current lesson: " + currentLesson);
        Debug.Log("Current duration: " + totalDuration); // Toplam süreyi yazdır
    }
    else
    {
        // Eğer yeni dersin adı mevcut dersin adına eşit değilse, yeni dersi başka bir ders olarak kabul eder
        // Bu durumda ek bir işlem yapılabilir veya bir uyarı mesajı gösterilebilir
        Debug.LogWarning("Yeni ders mevcut dersle aynı değil!");
    }

}

    // Stringden ders bilgilerini oluştur
    private List<DersBilgisi> StringdenDersBilgilerine(string stringDeger)
    {
        List<DersBilgisi> dersBilgileri = new List<DersBilgisi>();

        string[] dersler = stringDeger.Split(';');

        foreach (string dersString in dersler)
        {
            if (!string.IsNullOrEmpty(dersString))
            {
                string[] dersBilgisi = dersString.Split(',');

                DersBilgisi ders = new DersBilgisi();
                ders.dersAdi = dersBilgisi[0];
                ders.harcananSure = float.Parse(dersBilgisi[1]);

                dersBilgileri.Add(ders);
            }
        }

        return dersBilgileri;
    }

    // Ders bilgilerini stringe çevir
    private string DersBilgileriniStringeCevir(List<DersBilgisi> dersBilgileri)
    {
        string stringDeger = "";

        foreach (DersBilgisi ders in dersBilgileri)
        {
            stringDeger += ders.dersAdi + "," + ders.harcananSure + ";";
        }

        return stringDeger;
    }
}
