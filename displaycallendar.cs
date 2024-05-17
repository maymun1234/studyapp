using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class CalendarDisplay : MonoBehaviour
{
    public TMP_Text calendarTextPrefab; // Takvim metni için prefab
    public Transform calendarParent; // Takvim metnin ekleneceği ebeveyn transform
    public TMP_Text monthText; // Gösterilen ayın ismini yazacak TMP objesi
    public TMP_Text selectedDateText; // Seçilen tarihin hangi gün olduğunu yazacak TMP objesi
    public Color defaultColor = Color.black; // Varsayılan renk
    public Color selectedColor = Color.red; // Seçilen tarih için renk
    public Color thirdcolor = Color.blue;

    public RectTransform selectednumber;
    public RectTransform panelbg;
    

    public float cellWidth = 100f; // Hücre genişliği
    public float cellHeight = 100f; // Hücre yüksekliği
    public int daysInWeek = 7; // Haftadaki gün sayısı

    public DateTime currentMonth; // Şu an gösterilen ayın tarihi


    public static CalendarDisplay Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentMonth = DateTime.Now;
        DisplayCalendar(currentMonth);
        DisplayMonthName(currentMonth);
        int selectedDay = DateTime.Now.Day;
        TMP_Text selectedDayText = calendarParent.GetChild(selectedDay - 1).GetComponent<TMP_Text>();
         Debug.LogError(calendarParent.childCount);
        ResetTextColors();
       
        DisplaySelectedDate(selectedDay, selectedDayText);
        DisplaySelectedDate(selectedDay, selectedDayText);
    }

    private void DisplayCalendar(DateTime displayMonth)
{
    ClearCalendar();
    // Şu anda bulunduğumuz ayın ilk gününü al
    DateTime firstDayOfMonth = new DateTime(displayMonth.Year, displayMonth.Month, 1);

    // Ayın son gününü al
    DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

    // İlk günün haftanın kaçıncı günü olduğunu bul
    int dayOfWeek = (int)firstDayOfMonth.DayOfWeek;
    dayOfWeek = (dayOfWeek == 0) ? 6 : dayOfWeek - 1;

    // Takvim metnini oluştur
    for (int i = 0; i < DateTime.DaysInMonth(displayMonth.Year, displayMonth.Month); i++)
    {
        // Günü hesapla
        DateTime day = firstDayOfMonth.AddDays(i);

        // Ayın sınırları içinde mi kontrol et
        if (day < firstDayOfMonth || day > lastDayOfMonth)
            continue;

        // Hücrenin konumunu hesapla
        float x = ((i + dayOfWeek) % daysInWeek) * cellWidth;
        float y = -((i + dayOfWeek) / daysInWeek) * cellHeight;

        // Takvim metni örneğini oluştur
        TMP_Text dayText = Instantiate(calendarTextPrefab, calendarParent);
        dayText.gameObject.SetActive(true);

        // Gün metnini ayarla ve konumunu güncelle
        dayText.text = day.Day.ToString();
        dayText.rectTransform.anchoredPosition = new Vector2(x, y);

        // İsteğe bağlı olarak özel stiller veya işlevler uygulayabilirsiniz

        // Gün metnine tıklama işlevi ekle
        int dayNumber = day.Day;
        Button button = dayText.gameObject.GetComponent<Button>();
        button.onClick.AddListener(() => DisplaySelectedDate(dayNumber, dayText)); // Tıklanan günün metnini de ilet

        // Hafta sonu günlerini farklı renkte göster
        if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday)
        {
            dayText.color = thirdcolor; // Hafta sonları mavi renkte gösterilecek
        }
    }
}
    private void DisplayMonthName(DateTime displayMonth)
    {
        // Ay ismini al ve ekrana yaz
        monthText.text = displayMonth.ToString("MMMM yyyy");
    }

   private void DisplaySelectedDate(int day, TMP_Text dayText)
{
    // Seçilen günün tarihini hesapla ve yaz
    DateTime selectedDate = new DateTime(currentMonth.Year, currentMonth.Month, day);
    selectedDateText.text = selectedDate.ToString("dd MMMM yyyy");

    // Tüm metinleri varsayılan renge dönüştür
    ResetTextColors();
     panelbg.gameObject.SetActive(true);

    // Tıklanan günün rengini değiştir
    dayText.color = selectedColor;
    selectednumber = dayText.rectTransform;
    panelbg.position = selectednumber.position;

    // Hafta sonu günlerini beyaz olarak ayarla
    if (selectedDate.DayOfWeek == DayOfWeek.Saturday || selectedDate.DayOfWeek == DayOfWeek.Sunday)
    {
        dayText.color = selectedColor; // Hafta sonları beyaz renkte gösterilecek
    }
    PlayerPrefs.SetInt("SelectedDayIndex", day);
    PlayerPrefs.SetString("SelectedDate", selectedDate.ToString("dd.MM.yyyy"));
        PlayerPrefs.Save();
        monthplanner.Instance.showeventsinmonth(selectedDate);

}

   private void ResetTextColors()
{
    // Takvim metnindeki tüm günlerin rengini varsayılan renge dönüştür
    foreach (Transform child in calendarParent)
    {
        TMP_Text dayText = child.GetComponent<TMP_Text>();
        if (dayText != null)
        {
            dayText.color = defaultColor;
            
            // Tıklanan sayının hafta sonu olup olmadığını kontrol et
            int dayNumber = int.Parse(dayText.text);
            DateTime day = new DateTime(currentMonth.Year, currentMonth.Month, dayNumber);
            if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday)
            {
                dayText.color = thirdcolor; // Hafta sonları thirdcolor renkte gösterilecek
            }
        }
    }
   
}




    private void ClearCalendar()
    {
         panelbg.gameObject.SetActive(false);
        // Takvim metnindeki tüm eski günleri temizle
        foreach (Transform child in calendarParent)
        {
            Destroy(child.gameObject);
        }
    }
[SerializeField]
     public List<DateTime> pendingDates = new List<DateTime>(); // Bekleyen tarih listesi

    public void ShowPreviousMonth()
    {
        currentMonth = currentMonth.AddMonths(-1);
        DisplayCalendar(currentMonth);
        DisplayMonthName(currentMonth);
   
        
    pendingDates.Clear(); // Bekleyen tarih listesini temizle
    }

    public void ShowNextMonth()
    {
        currentMonth = currentMonth.AddMonths(1);
        DisplayCalendar(currentMonth);
        DisplayMonthName(currentMonth);
        
     
       
    pendingDates.Clear(); // Bekleyen tarih listesini temizle
    }

  

public void SpawnPanelAtDate(DateTime date)
{
    //Debug.Log("Pending Dates List Length: " + pendingDates.Count);
    pendingDates.Add(date);

    // Belirtilen tarihin ayındaki günün indeksini bul
    int dayIndex = date.Day - 1;
    //Debug.LogError(dayIndex);
    //Debug.LogError(calendarParent.childCount);

    // Hedef tarihin ay ve yıl bilgisini al
    int targetMonth = currentMonth.Month;
    //Debug.LogError(targetMonth);
    int targetYear = date.Year;

    // Belirtilen tarihin ayındaki günlerin panele eklenmesi
    foreach (DateTime pendingDate in pendingDates)
    {
        // Listenin elemanının ay ve yıl bilgisini al
        int pendingMonth = pendingDate.Month;
        int pendingYear = pendingDate.Year;
        Transform dayPosition = calendarParent.GetChild(dayIndex);
                Transform firstChild = dayPosition.GetChild(0);
        firstChild.gameObject.SetActive(false);
        

        // Eğer hedef ay ve yıl ile aynıysa panele ekleme işlemini yap
        if (pendingMonth == targetMonth && pendingYear == targetYear)
        {
            
            //Debug.LogError("hedef ay"+pendingMonth);
            // Belirtilen tarihin gün indeksi geçerli mi kontrol et
            if (dayIndex >= 0 && dayIndex < DateTime.DaysInMonth(date.Year, currentMonth.Month))
            {
                
                

                if (currentMonth.Month == pendingMonth)
                {
                    // panelbg2'nin pozisyonunu belirtilen günün pozisyonuna ayarla
                  
                        firstChild.gameObject.SetActive(true);
                       // Debug.LogError(pendingDate);
                    
                }
                else
                {
                    
                }

            }
            else
            {
                firstChild.gameObject.SetActive(false);
            }
        }
    }
}


}
