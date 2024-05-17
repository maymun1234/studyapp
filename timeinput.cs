using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class TimeInput : MonoBehaviour
{
    public TMP_InputField hourInputField; // Saat girişi için TMP_InputField
    public TMP_InputField minuteInputField; // Dakika girişi için TMP_InputField
    public GameObject uiObjectToDisable; // Devre dışı bırakılacak UI nesnesi
    public TMP_Text countdownText; // Geri sayımı gösterecek TMP_Text
    public TMP_Text percentageText; // Yüzde cinsinden kalan süreyi gösterecek TMP_Text
    public Slider progressBar; // Progress bar için Slider
    public Button breakButton;
    public Color renk1;
    public Color renk2;

    public Color renk3;
    public bool isPaused = false;

     [System.Serializable]
    public class ResetMethods
    {
        public UnityEvent resetEvent;
        // Diğer parametreleri buraya ekleyebilirsiniz.
    }

[System.Serializable]
    public class startmethods
    {
        public UnityEvent startEvent;
        // Diğer parametreleri buraya ekleyebilirsiniz.
    }


    public startmethods startmethod;
    public ResetMethods resetMethod;
    
    
    

    

    void Start()
    {
        // Uygulama açıldığında UI nesnesini devre dışı bırak
        uiObjectToDisable.SetActive(false);
        isPaused = false;
    }

    // UI nesnesini aktifleştirmek için metod
    public void ActivateUIObject()
    {
        // UI nesnesini aktif hale getir
        uiObjectToDisable.SetActive(true);

        // Saat ve dakika inputlarını "00" olarak ayarla
        hourInputField.text = "00";
        minuteInputField.text = "00";
    }

     public void HandleBreakButton()
    {
        if (isPaused)
        {
            // Eğer süre duraklatıldıysa, devam et
            isPaused = false;
            breakButton.GetComponentInChildren<TMP_Text>().text = "MOLA VER";
            breakButton.GetComponent<Image>().color = renk1;
           countdownText.color = Color.white;

        }
        else
        {
            // Eğer süre devam ediyorsa, duraklat
            isPaused = true;
            breakButton.GetComponentInChildren<TMP_Text>().text = "DEVAM ET";
            breakButton.GetComponent<Image>().color = renk2;
            
             countdownText.color = renk1;
        }
    }

    private Coroutine countdownRoutine;

    public void GetTimeInput()
{

    
    // Daha önce başlatılan geri sayım rutinini durdur
    if (countdownRoutine != null)
    {
        StopCoroutine(countdownRoutine);
    }

    // Dakika alanı boşsa "00" olarak ayarla
    if (string.IsNullOrEmpty(minuteInputField.text))
    {
        minuteInputField.text = "00";
    }

    // Saat alanı boşsa "00" olarak ayarla
    if (string.IsNullOrEmpty(hourInputField.text))
    {
        hourInputField.text = "00";
    }

    // Saat ve dakika girişlerini ayrı ayrı alıyoruz
    int hour = int.Parse(hourInputField.text);
    int minute = int.Parse(minuteInputField.text);

    // Saat ve dakikayı birleştirerek tam bir zaman oluşturuyoruz (örneğin: 14 saat 30 dakika)
    string time = hour.ToString("00") + ":" + minute.ToString("00");
    
    Debug.Log("Selected time: " + time);
    SaveCurrentDuration();

    // Progress bar'ı sıfırla
    progressBar.value = 0f;
    uiObjectToDisable.SetActive(false);

    // Yeni bir geri sayım rutini başlat
    countdownRoutine = StartCoroutine(StartCountdown(hour, minute));

    if (hourInputField.interactable)
    {
        // Dakika girişini aktif hale getir
        hourInputField.interactable = true;
        minuteInputField.interactable = true;
        // Dakika girişine odaklan
        minuteInputField.Select();
        minuteInputField.ActivateInputField();
    }

    startmethod.startEvent.Invoke();


    
}

public void SaveCurrentDuration()
    {
        // Mevcut süreyi al ve PlayerPrefs'e kaydet
        int hour = int.Parse(hourInputField.text);
        int minute = int.Parse(minuteInputField.text);
        int currentDurationInSeconds = (hour * 3600) + (minute * 60);
        PlayerPrefs.SetInt("currentDuration", currentDurationInSeconds);
        PlayerPrefs.Save();
         Debug.Log("Current duration: " + hour + " hours " + minute + " minutes");
    
    }
   IEnumerator StartCountdown(int hour, int minute)
{
    // Geri sayım saniyesi hesapla
    float countdownSeconds = (hour * 3600) + (minute * 60);
    float totalSeconds = countdownSeconds;
    
    while (countdownSeconds > 0)
    {
        if (!isPaused)
        {
            // Geri sayımı güncelle
            int hoursLeft = Mathf.FloorToInt(countdownSeconds / 3600);
            int minutesLeft = Mathf.FloorToInt((countdownSeconds % 3600) / 60);
            int secondsLeft = Mathf.FloorToInt(countdownSeconds % 60);
            string countdownTextString = string.Format("{0:00}:{1:00}:{2:00}", hoursLeft, minutesLeft, secondsLeft);
            countdownText.text = countdownTextString;

            // Yüzdeyi hesapla ve göster
            float percentage = ((totalSeconds - countdownSeconds) / totalSeconds) * 100f;
            percentageText.text = "%" + percentage.ToString("0") + "";

            // Progress bar'ı güncelle
            progressBar.value = 1f - (countdownSeconds / totalSeconds);
        }

        // Bir saniye bekle
        yield return new WaitForSeconds(1);

        // Eğer duraklatılmadıysa geri sayımı güncelle
        if (!isPaused)
        {
            countdownSeconds--;
        }
    }

    resetMethod.resetEvent.Invoke();
    countdownText.text="00:00:00";
    percentageText.text = "%100";
    progressBar.value = 100f;



    // Geri sayım tamamlandığında bir şey yapabilirsiniz
}

}
