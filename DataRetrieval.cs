using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public class DataDisplay : MonoBehaviour
{
    public GameObject panelPrefab; // Panel için prefab
    public Transform panelParent; // Panelin ekleneceği ebeveyn transform
    public TMP_Text textPrefab; // Tarih ve süre için prefab

    private const string DAILY_RECORDS_KEY = "DailyStudyRecords";

    private List<GameObject> displayedPanels = new List<GameObject>(); // Oluşturulan panellerin listesi

    public static DataDisplay Instance;

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
        DisplayData();
    }

    public void DisplayData()
    {
        // Önce mevcut panelleri temizle
        ClearDisplayedPanels();

        string savedRecordsString = PlayerPrefs.GetString(DAILY_RECORDS_KEY, "");

        if (!string.IsNullOrEmpty(savedRecordsString))
        {
            string[] recordSplits = savedRecordsString.Split(';');

            foreach (string recordData in recordSplits)
            {
                if (string.IsNullOrEmpty(recordData))
                    continue;

                string[] dataParts = recordData.Split(',');

                if (dataParts.Length != 2)
                {
                    Debug.LogError("Invalid record format encountered!");
                    continue;
                }

                long dateTicks = long.Parse(dataParts[0]);
                DateTime date = new DateTime(dateTicks);
                float studyTimeMinutes = float.Parse(dataParts[1]);

                // Panel oluştur
                GameObject panel = Instantiate(panelPrefab, panelParent);
                panel.SetActive(true);

                // Tarih ve süreyi TMP_Text nesnelerine ekle
                TMP_Text dateText = panel.transform.GetChild(0).GetComponent<TMP_Text>();
                TMP_Text studyTimeText = panel.transform.GetChild(1).GetComponent<TMP_Text>();

                dateText.text = "" + date.ToString("dd MMMM yyyy");
                studyTimeText.text = "Study Time: " + studyTimeMinutes.ToString() + " minutes";

                // Oluşturulan paneli listeye ekle
                displayedPanels.Add(panel);
            }
        }
        else
        {
            Debug.Log("No data found.");
        }
    }

    private void ClearDisplayedPanels()
    {
        // Oluşturulan panelleri temizle
        foreach (var panel in displayedPanels)
        {
            Destroy(panel);
        }
        displayedPanels.Clear();
    }
}
