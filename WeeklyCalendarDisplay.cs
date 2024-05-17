using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Globalization;

public class WeeklyCalendarDisplay : MonoBehaviour
{
    public TMP_Text calendarTextPrefab; // Takvim metni için prefab
    public GameObject backgroundpanel;
    public Transform calendarParent; // Takvim metnin ekleneceği ebeveyn transform
    public TMP_Text monthText; // Gösterilen ayın ismini yazacak TMP objesi
    public TMP_Text selectedDateText; // Seçilen tarihin hangi gün olduğunu yazacak TMP objesi
    public TMP_Text weekNumberText; // Haftanın numarasını yazacak TMP objesi
    public Color defaultColor = Color.black; // Varsayılan renk
    public Color selectedColor = Color.red; // Seçilen tarih için renk
    public Color thirdcolor = Color.blue;

    public float cellWidth = 100f; // Hücre genişliği
    public float cellHeight = 100f; // Hücre yüksekliği

    private List<DateTime> weekDays; // Haftanın günlerini tutacak liste
    private DateTime currentWeek; // Şu an gösterilen haftanın başlangıç tarihi

    private void Start()
    {
        // Başlangıçta bu haftayı göster
        currentWeek = GetStartOfWeek(DateTime.Now);
        DisplayCalendar();

        // Başlangıçta bu haftayı gösterdiğimiz için haftanın numarasını güncelle
        UpdateWeekNumber();

        // Başlangıçta bugünü seç
        SelectToday();
        ResetTextColors();
        DisplayWeekName(weekDays[0]);
    }

    private void CreateWeekDaysList()
    {
        weekDays = new List<DateTime>();

        for (int i = 0; i < 7; i++)
        {
            weekDays.Add(currentWeek.AddDays(i));
        }
    }

    private void DisplayCalendar()
    {
        ClearCalendar();
        CreateWeekDaysList();

        // Haftanın günlerini takvimde göster
        for (int i = 0; i < weekDays.Count; i++)
        {
            DateTime day = weekDays[i];

            // Hücrenin konumunu hesapla
            float x = i * cellWidth;
            float y = 0;

            // Takvim metni örneğini oluştur
            TMP_Text dayText = Instantiate(calendarTextPrefab, calendarParent);
            dayText.gameObject.SetActive(true);

            // Gün metnini ayarla ve konumunu güncelle
            dayText.text = day.Day.ToString();
            dayText.rectTransform.anchoredPosition = new Vector2(x, y);

            // İsteğe bağlı olarak özel stiller veya işlevler uygulayabilirsiniz

            // Gün metnine tıklama işlevi ekle
            int dayIndex = i;
            Button button = dayText.gameObject.GetComponent<Button>();
            button.onClick.AddListener(() => DisplaySelectedDate(dayIndex));
            if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday)
            {
                dayText.color = thirdcolor; // Hafta sonları thirdcolor renkte gösterilecek
            }
        }
    }

  private void ResetTextColors()
{
    // Takvim metnindeki tüm günlerin rengini varsayılan renge dönüştür
    int childCount = calendarParent.childCount;
    for (int i = 0; i < childCount; i++)
    {

        TMP_Text dayText = calendarParent.GetChild(i).GetComponent<TMP_Text>();

        if (dayText != null)
        {
            dayText.color = defaultColor;
             Transform firstChild = dayText.transform.GetChild(0);
            firstChild.gameObject.SetActive(false);

            // Son iki günü thirdcolor olarak ayarla
            if (i >= childCount - 2)
            {
                dayText.color = thirdcolor; // Son iki günü thirdcolor renkte gösterilecek
            }
        }
    }
}

private void DisplaySelectedDate(int dayIndex)
{
    // Seçilen günün tarihini hesapla ve yaz
    DateTime selectedDate = weekDays[dayIndex];
    selectedDateText.text = selectedDate.ToString("dd MMMM yyyy");

    // Tüm metinleri varsayılan renge dönüştür
    ResetTextColors();

    // Tıklanan günün metnini ve rengini değiştir
    TMP_Text selectedDayText = calendarParent.GetChild(dayIndex).GetComponent<TMP_Text>();
    Transform firstChild = selectedDayText.transform.GetChild(0);
            firstChild.gameObject.SetActive(true);
    selectedDayText.color = selectedColor;
    GameObject backgroundPanel1 = Instantiate(backgroundpanel, selectedDayText.transform);
        backgroundPanel1.transform.SetAsFirstSibling();

    // Hafta sonu ise thirdcolor uygula
    
}

    private void ClearCalendar()
    {
        // Takvim metnindeki tüm eski günleri temizle
        foreach (Transform child in calendarParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void SelectToday()
{
    // Bugünün indeksini bul ve seç
    DateTime today = DateTime.Now;
    int todayIndex = (int)(today - currentWeek).TotalDays;
    todayIndex = Mathf.Clamp(todayIndex, 0, 6); // Eğer haftanın dışında bir günse, en yakın Pazartesi'ye çek
    DisplaySelectedDate(todayIndex);
}

    private DateTime GetStartOfWeek(DateTime date)
    {
        int diff = date.DayOfWeek - DayOfWeek.Monday;
        if (diff < 0)
            diff += 7;
        return date.AddDays(-1 * diff).Date;
    }

    public void ShowPreviousWeek()
{
    currentWeek = currentWeek.AddDays(-7);
    CreateWeekDaysList();
    DisplayCalendar();
    DisplayWeekName(weekDays[0]);
}

public void ShowNextWeek()
{
    currentWeek = currentWeek.AddDays(7);
    CreateWeekDaysList();
    DisplayCalendar();
    DisplayWeekName(weekDays[0]);
}

    private void UpdateWeekNumber()
    {
        int weekNumber = GetWeekNumberOfYear(currentWeek);
        weekNumberText.text = "Week " + weekNumber.ToString();
    }
 private int GetWeekNumberOfYear(DateTime date)
    {
        // Haftanın yıl içindeki numarasını döndürür
        CultureInfo ciCurr = CultureInfo.CurrentCulture;
        int weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        return weekNum;
    }
 private void DisplayWeekName(DateTime displayWeek)
{
    DateTime startOfWeek = GetStartOfWeek(displayWeek);
    DateTime endOfWeek = startOfWeek.AddDays(6);

    string startOfWeekText = startOfWeek.ToString("dd MMMM yyyy");
    string endOfWeekText = endOfWeek.ToString("dd MMMM yyyy");

    monthText.text = startOfWeekText + " - " + endOfWeekText;
    monthText.text = startOfWeekText + " haftası";
}

}
