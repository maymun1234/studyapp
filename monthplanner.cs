using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class monthplanner : MonoBehaviour
{
    public TMP_Text selectedDateTMP;
    public TMP_InputField title;
    public TMP_InputField description;
    public TMP_Text eventttitle;
    public TMP_Text eventtdescription;
    



    // Etkinlikleri saklamak için anahtarlar
    private const string EventsKey = "Events";

    // Inspector'da görmek için etkinlik listesi
    [SerializeField] private List<string> events = new List<string>();

     public static monthplanner Instance;

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

    // Start is called before the first frame update
    void Start()
    {
        // Mevcut etkinlikleri yükle
        events = GetEvents();
         foreach (var eventDetail in events)
        {
            string[] eventParts = eventDetail.Split(':');
            if (eventParts.Length == 2)
            {
                DateTime eventDate;
                if (DateTime.TryParse(eventParts[0], out eventDate))
                {
                    CalendarDisplay.Instance.SpawnPanelAtDate(eventDate);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
          foreach (var eventDetail in events)
        {
            string[] eventParts = eventDetail.Split(':');
            if (eventParts.Length == 2)
            {
                DateTime eventDate;
                if (DateTime.TryParse(eventParts[0], out eventDate))
                {
                    CalendarDisplay.Instance.SpawnPanelAtDate(eventDate);
                }
            }
        }
    }

    public void adddatetocallendar()
    {
        // Input kutularındaki bilgileri al
        string eventTitle = title.text;
        string eventDescription = description.text;

        // Seçilen tarihi al
        string selectedDate = PlayerPrefs.GetString("SelectedDate");

        // Eğer boş bir tarih varsa, hata mesajı yazdır ve işlemi sonlandır
        if (string.IsNullOrEmpty(selectedDate))
        {
            Debug.LogError("Hata: Kaydedilecek bir tarih yok!");
            return;
        }

        // Yeni bir etkinlik oluştur
        string newEvent = $"{selectedDate}: {eventTitle} - {eventDescription}";

        // Yeni etkinliği listeye ekle
        events.Add(newEvent);

        // Güncellenmiş etkinlik listesini kaydet
        SaveEvents(events);

        // Kayıt başarıyla tamamlandı mesajını yazdır
        Debug.Log("Etkinlik başarıyla kaydedildi: " + newEvent);
        showeventsinmonth(DateTime.Parse(selectedDate));
        string selecteddate = PlayerPrefs.GetString("SelectedDate");
    }

    public void loadadddatepanel()
    {
        string selecteddate = PlayerPrefs.GetString("SelectedDate");
        Debug.LogWarning(selecteddate);
        selectedDateTMP.text = selecteddate;
    }

    public GameObject panelPrefab; // Panel prefabı için referans değişkeni
public Transform panelParent;

    public void showeventsinmonth(DateTime date)
    {
        // Belirtilen tarihteki etkinlikleri bul
        List<string> eventsInDay = GetEventsInDay(date);

        foreach (Transform child in panelParent)
    {
        Destroy(child.gameObject);
    }

    // Bulunan etkinlikleri konsola yazdır
    foreach (string eventDetail in eventsInDay)
    {
        Debug.Log(eventDetail);
        string[] eventParts = eventDetail.Split(':');
        if (eventParts.Length == 2)
        {
            // Yeni bir panel oluştur
            GameObject eventPanel = Instantiate(panelPrefab, panelParent);
            eventPanel.SetActive(true);

            // Panelin içindeki TMP_Text nesnelerini bul
            TMP_Text titleText = eventPanel.transform.GetChild(1).GetComponent<TMP_Text>();
            TMP_Text descriptionText = eventPanel.transform.GetChild(2).GetComponent<TMP_Text>();

            // Etkinlik başlığını ve açıklamasını ayarla
            titleText.text = eventParts[1].Split('-')[0].Trim();
            descriptionText.text = eventParts[1].Split('-')[1].Trim();
        }

    }
    }

    private List<string> GetEvents()
    {
        // PlayerPrefs'ten etkinlikleri al
        string eventsJson = PlayerPrefs.GetString(EventsKey, "{}");
        EventList eventList = JsonUtility.FromJson<EventList>(eventsJson);
        return eventList.events;
    }

    private void SaveEvents(List<string> events)
    {
        // Etkinlikleri JSON formatında sakla
        EventList eventList = new EventList { events = events };
        string eventsJson = JsonUtility.ToJson(eventList);
        PlayerPrefs.SetString(EventsKey, eventsJson);
        PlayerPrefs.Save();
    }

    [Serializable]
    private class EventList
    {
        public List<string> events = new List<string>();
    }

   private List<string> GetEventsInDay(DateTime date)
{
    // Belirtilen tarihteki etkinlikleri bul
    List<string> eventsInDay = new List<string>();

    // Belirtilen tarihteki etkinlikleri tüm etkinliklerden ayır
    foreach (string eventDetail in events)
    {
        string[] eventParts = eventDetail.Split(':');
        if (eventParts.Length == 2)
        {
            DateTime eventDate;
            if (DateTime.TryParse(eventParts[0], out eventDate))
            {
                if (eventDate.Date == date.Date) // Sadece belirtilen günün etkinliklerini al
                {
                    eventsInDay.Add(eventDetail);
                }
            }
        }
    }

    return eventsInDay;
}



public void RemoveEventFromButton()
{
    // Event butonunu bul
    GameObject eventButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
    
    // Event butonunun parentını (paneli) bul
    GameObject eventPanel = eventButton.transform.parent.gameObject;

    // Eventin başlığını ve tarihini al
    TMP_Text titleText = eventPanel.transform.GetChild(1).GetComponent<TMP_Text>();
    string eventTitle = titleText.text;

    // Belirtilen etkinliği listeden kaldır
    if (!string.IsNullOrEmpty(eventTitle))
    {
        events.RemoveAll(eventDetail => eventDetail.Contains(eventTitle));
        SaveEvents(events);
    }

    // Paneli ve içindeki etkinliği sil
    Destroy(eventPanel);
}



}


