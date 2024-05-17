using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CurrentLessonDisplay : MonoBehaviour
{
    // TextMeshProUGUI referansı
    public TextMeshProUGUI currentLessonText;

    void Start()
    {
        // Mevcut dersi kontrol et ve ekrana yazdır
        DisplayCurrentLesson();
    }

    private void Update() {
        DisplayCurrentLesson();
    }



    public void DisplayCurrentLesson()
    {
        // Mevcut ders anahtarını kontrol et
        if (PlayerPrefs.HasKey("currentLesson"))
        {
            // Mevcut ders adını al
            string currentLesson = PlayerPrefs.GetString("currentLesson");

            // Ekrandaki TextMeshProUGUI nesnesine mevcut dersi yazdır
            currentLessonText.text = "" + currentLesson;
        }
        else
        {
            // Eğer mevcut ders anahtarı yoksa, ekrana "Mevcut Ders Yok" yazdır
            currentLessonText.text = "Mevcut Ders Yok";
        }
    }
}
