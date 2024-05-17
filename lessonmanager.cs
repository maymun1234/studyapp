using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DersKaydetme : MonoBehaviour
{
    // Panel nesnesi referansı
    public GameObject panelPrefab;
    public GameObject panelContainer1;

    // Ana panel referansı
    public Transform panelContainer;

    // Input alanı ve buton referansları
    public TMP_InputField dersAdiInput;
    public Button kaydetButton;
    public Button aabuto;

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

    void Start()
    {
        // Butonun tıklama olayını dinle
        kaydetButton.onClick.AddListener(KaydetButonuTiklandi);

        // Tabloyu yükle (eğer daha önce kaydedilmişse)
        TabloyuYukle();
    }

    void KaydetButonuTiklandi()
    {
        // Input alanından ders adını al
        string dersAdi = dersAdiInput.text;

        // Ders adı boşsa işlem yapma
        if (string.IsNullOrEmpty(dersAdi))
        {
            Debug.LogWarning("Ders adı boş olamaz!");
            return;
        }

        // Ders bilgisini oluştur
        DersBilgisi yeniDers = new DersBilgisi();
        yeniDers.dersAdi = dersAdi;
        yeniDers.harcananSure = 0f; // Süreyi otomatik olarak sıfır ata

        // Ders bilgisini listeye ekle
        dersBilgileri.Add(yeniDers);

        // Tabloyu güncelle
        PlayerPrefs.SetString("dersTablosu", DersBilgileriniStringeCevir(dersBilgileri));
        PlayerPrefs.Save();

        // Yeni panel oluştur
        PanelOlustur(yeniDers);
    }

    void TabloyuYukle()
    {
        // PlayerPrefs'ten tabloyu yükle
        string kaydedilmisDersler = PlayerPrefs.GetString("dersTablosu");

        if (!string.IsNullOrEmpty(kaydedilmisDersler))
        {
            dersBilgileri = StringdenDersBilgilerine(kaydedilmisDersler);

            // Kaydedilmiş dersler için panelleri oluştur
            foreach (DersBilgisi ders in dersBilgileri)
            {
                PanelOlustur(ders);
            }

            
        }
    }

    string DersBilgileriniStringeCevir(List<DersBilgisi> dersBilgileri)
    {
        string stringDeger = "";

        foreach (DersBilgisi ders in dersBilgileri)
        {
            stringDeger += ders.dersAdi + "," + ders.harcananSure + ";";
        }

        return stringDeger;
    }

    List<DersBilgisi> StringdenDersBilgilerine(string stringDeger)
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

    void PanelOlustur(DersBilgisi ders)
    {
        // Panel örneğini instantiate et
        GameObject yeniPanel = Instantiate(panelPrefab, panelContainer);


        // Panelin altında bulunan ders adı ve ders süresi Text nesnelerini bul
        TextMeshProUGUI dersAdiText = yeniPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI harcananSureText = yeniPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        Button silButon = yeniPanel.transform.GetChild(2).GetComponent<Button>();
    yeniPanel.SetActive(true);

        // Ders bilgilerini Text nesnelerine ata
        dersAdiText.text = ders.dersAdi;
        harcananSureText.text = FormatSaatVeDakika(ders.harcananSure);

        silButon.onClick.AddListener(() => SilButonuTiklandi(yeniPanel));

        aabuto.transform.SetAsLastSibling();
        
        
        Button panelButon = yeniPanel.GetComponent<Button>();
    panelButon.onClick.AddListener(() => PanelButonuTiklandi(ders.dersAdi));
    }


    private void Update() {
        int dsjjsdjds = PlayerPrefs.GetInt("currentDuration", 0);
        //Debug.LogWarning(dsjjsdjds);
    }


    void SilButonuTiklandi(GameObject panel)
{
    // Paneli listeden kaldır
    DersBilgisi ders = dersBilgileri.Find(item => item.dersAdi == panel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
    dersBilgileri.Remove(ders);
    Destroy(panel);
    
    // Tabloyu güncelle
    PlayerPrefs.SetString("dersTablosu", DersBilgileriniStringeCevir(dersBilgileri));
    PlayerPrefs.Save();
}

    void PanelButonuTiklandi(string dersAdi)
{
    Debug.Log("Panel Butonuna Tıklandı: " + dersAdi);
    PlayerPrefs.SetString("currentLesson", dersAdi);
    PlayerPrefs.Save();
    Debug.Log("Seçilen ders: " + dersAdi);
    panelContainer1.SetActive(false);
}


public void DurationCompleted()
{
    string currentLesson = PlayerPrefs.GetString("currentLesson");

    // Mevcut dersin bilgisini listeden bul
    DersBilgisi ders = dersBilgileri.Find(item => item.dersAdi == currentLesson);

    // Eğer mevcut ders bulunamazsa uyarı ver ve metoddan çık
    if (ders == null)
    {
        Debug.LogWarning("Mevcut ders bulunamadı!");
        return;
    }

    // Mevcut süreyi al ve mevcut dersin harcanan süresi ile topla
    int currentDuration = PlayerPrefs.GetInt("currentDuration", 0);
    Debug.LogWarning(currentDuration);
    float updatedDuration = currentDuration + ders.harcananSure;

    // Toplam süreyi güncelle
    
    PlayerPrefs.Save();

    Debug.Log("Mevcut dersin süresi güncellendi: " + updatedDuration);

    // Mevcut dersin harcanan süresini güncelle
    ders.harcananSure = updatedDuration;

    // Ders bilgisini tabloya kaydet
    PlayerPrefs.SetString("dersTablosu", DersBilgileriniStringeCevir(dersBilgileri));
    PlayerPrefs.Save();

    Debug.Log("Mevcut dersin harcanan süresi güncellendi ve tabloya kaydedildi: " + updatedDuration);
    UpdatePanelDuration(currentLesson, updatedDuration);
}


private string FormatSaatVeDakika(float sure)
    {
        int saat = Mathf.FloorToInt(sure / 3600);
        int dakika = Mathf.FloorToInt((sure % 3600) / 60);
        return string.Format("{0} saat {1} dakika", saat, dakika);
    }

    private string FormatSaatVeDakika(int saat, int dakika)
    {
        return string.Format("{0} saat {1} dakika", saat, dakika);
    }
private void UpdatePanelDuration(string lessonName, float updatedDuration)
{
    foreach (Transform child in panelContainer)
    {
        // Panelin ders adı metni "lesson" kelimesini içeriyorsa
        if (child.name.Contains("lesson"))
        {
            TextMeshProUGUI dersAdiText = child.GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI harcananSureText = child.GetChild(1).GetComponent<TextMeshProUGUI>();

            // Eğer ders adı verilen dersin adıyla eşleşiyorsa
            if (dersAdiText.text == lessonName)
            {
                // Güncellenmiş süreyi saat ve dakika cinsine dönüştür
                int saat = Mathf.FloorToInt(updatedDuration / 3600);
                int dakika = Mathf.FloorToInt((updatedDuration % 3600) / 60);

                // Yeni süreyi harcanan süre metninin içine yerleştir
                harcananSureText.text = string.Format("{0} saat {1} dakika", saat, dakika);
                break;
            }
        }
    }
}}
