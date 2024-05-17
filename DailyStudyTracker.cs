using UnityEngine;
using System;
using System.Collections.Generic;

public class DailyStudyTracker : MonoBehaviour
{
    private const string DAILY_RECORDS_KEY = "DailyStudyRecords"; // PlayerPrefs key for records
    private const char RECORD_SEPARATOR = ','; // Separator for record data

    [System.Serializable]
    public class DailyStudyData
    {
        public DateTime date;
        public float studyTimeMinutes;

        public DailyStudyData(DateTime date, float studyTimeMinutes)
        {
            this.date = date;
            this.studyTimeMinutes = studyTimeMinutes;
        }
    }

    public List<DailyStudyData> dailyRecords;

    private void Start()
    {
        LoadRecords(); // Load records on game start
    }

    public void AddRecord(DateTime date, float studyTimeMinutes)
    {
        LoadRecords(); // Load records every time a record is added

        // Check for existing record on the same date
        DailyStudyData existingRecord = dailyRecords.Find(record => record.date.Date == date.Date);
        if (existingRecord != null)
        {
            existingRecord.studyTimeMinutes += studyTimeMinutes;
        }
        else
        {
            dailyRecords.Add(new DailyStudyData(date, studyTimeMinutes));
        }

        SaveRecords(); // Save records after adding/updating
    }

    public float GetStudyTimeForDate(DateTime date)
    {
        LoadRecords(); // Load records every time study time is requested

        DailyStudyData record = dailyRecords.Find(d => d.date.Date == date.Date);
        return record != null ? record.studyTimeMinutes : 0f;
    }

    public List<DailyStudyData> GetAllRecords()
    {
        LoadRecords(); // Load records every time all records are requested
        return dailyRecords;
    }

    public void AddDuration()
    {
        DateTime today = DateTime.Today;
        int currentDurationInteger = PlayerPrefs.GetInt("currentDuration", 0);
        float currentDuration = (float)currentDurationInteger;

        AddRecord(today, currentDuration);
      
    DataDisplay.Instance.DisplayData();

    }

    private void SaveRecords()
    {
        string recordsString = string.Join(";", dailyRecords.ConvertAll(record =>
            $"{record.date.Ticks}{RECORD_SEPARATOR}{record.studyTimeMinutes}"));

        PlayerPrefs.SetString(DAILY_RECORDS_KEY, recordsString);
        PlayerPrefs.Save();
    }

    private void LoadRecords()
    {
        string savedRecordsString = PlayerPrefs.GetString(DAILY_RECORDS_KEY, "");
        if (!string.IsNullOrEmpty(savedRecordsString))
        {
            string[] recordSplits = savedRecordsString.Split(';');

            dailyRecords = new List<DailyStudyData>();

            foreach (string recordData in recordSplits)
            {
                if (string.IsNullOrEmpty(recordData))
                    continue;

                string[] dataParts = recordData.Split(RECORD_SEPARATOR);
                if (dataParts.Length != 2)
                {
                    Debug.LogError("Invalid record format encountered!");
                    continue;
                }

                long dateTicks = long.Parse(dataParts[0]);
                DateTime date = new DateTime(dateTicks);
                float studyTimeMinutes = float.Parse(dataParts[1]);

                dailyRecords.Add(new DailyStudyData(date, studyTimeMinutes));
            }
        }
        else
        {
            dailyRecords = new List<DailyStudyData>();
        }
    }
}
