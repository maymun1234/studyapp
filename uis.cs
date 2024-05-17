using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class uis : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ToggleUIObject(GameObject uiObject)
{
    if(uiObject.activeSelf) // Eğer UI objesi aktifse
    {
        uiObject.SetActive(false); // Devre dışı bırak
    }
    else // Eğer UI objesi devre dışıysa
    {
        uiObject.SetActive(true); // Aktif hale getir
    }
}

public void CloseUIObject(GameObject uiObject)
{
uiObject.SetActive(false);

}
}