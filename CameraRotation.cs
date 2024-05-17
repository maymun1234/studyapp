using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour
{
    public Transform target; // Döndürülecek nesne
    public float rotationSpeed = 5.0f; // Dönme hızı
    public float smoothness = 5.0f; // Yumuşaklık değeri

    private bool isRotating = false; // Dönüş durumu
    private Quaternion initialRotation; // Başlangıç rotasyonu
    private Vector3 initialPosition; // Başlangıç pozisyonu
    private IEnumerator followPathCoroutine; // Yol takip coroutine'i

    void Start()
    {
        // Başlangıç rotasyonunu ve pozisyonunu sakla
        initialRotation = transform.rotation;
        initialPosition = transform.position;
    }

    void Update()
    {
        // Sol fare tuşuna basılı tutuluyorsa
        if (Input.GetMouseButtonDown(0))
        {
            isRotating = true; // Dönüş başladı

            // Kameranın hedef noktasına doğru bakması için rotasyonu ayarla
            Vector3 lookDir = (target.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            // Yalnızca y rotasyonunu hedef noktasına bakacak şekilde ayarla
            targetRotation.x = 0f;
            targetRotation.z = 0f;
            transform.rotation = targetRotation;

            // Yol takip coroutine'ini başlat
            if (followPathCoroutine != null)
                StopCoroutine(followPathCoroutine);
            followPathCoroutine = FollowPath();
            StartCoroutine(followPathCoroutine);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isRotating = false; // Dönüş bitti
        }

        // Dönüş yapılıyorsa
        if (isRotating)
        {
            // Fare hareketlerini al
            float mouseX = Input.GetAxis("Mouse X");

            // Hedef noktasını al
            Vector3 targetPosition = target.position;

            // Y ekseni etrafında dönme
            transform.RotateAround(targetPosition, Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Yavaşça başlangıç rotasyonuna dön
            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, Time.deltaTime * smoothness);

            // Yavaşça başlangıç pozisyonuna dön
            transform.position = Vector3.Lerp(transform.position, initialPosition, Time.deltaTime * smoothness);
        }
    }

    IEnumerator FollowPath()
    {
        while (Vector3.Distance(transform.position, initialPosition) > 0.01f)
        {
            // Hedefe doğru hareket et
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, Time.deltaTime * smoothness);
            yield return null;
        }
    }
}
