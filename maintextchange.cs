using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class maintextchange : MonoBehaviour
{
       public TextMeshPro tmpObject;

    void Start()
    {
        // Bağlı olduğumuz GameObject içindeki TextMeshProUGUI bileşenini alıyoruz
        tmpObject = GetComponent<TextMeshPro>();
        ChangeText(2);
    }

    // Metod, gelen parametrelere göre uygun yazıyı döndürür
    public void ChangeText(int param1)
    {
        if (param1 == 1)
        {
            tmpObject.text="ders CALIS ";
        }
        else if (param1 == 2)
        {
            tmpObject.text="hos geldın";
        }

        else if (param1 == 3)
        {
            tmpObject.text="sure doldu";
        }
        else
        {
            Debug.LogError("Parametreler hatalı! Geçerli parametreler 1 veya 2 olmalıdır.");
           
        }
        tmpObject.paragraphSpacing = -31.75f;
    }

    // TextMeshPro objesindeki metni değiştirme metodunu çağırma
   

    // Örnek kullanım
    
}