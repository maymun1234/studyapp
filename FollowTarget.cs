using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target; // Hedef nesne
    public GameObject uiPanel; // UI Paneli

    void LateUpdate()
    {
        // Hedef nesnenin pozisyonunu ve rotasyonunu al
        Vector3 targetPosition = target.position;
        Quaternion targetRotation = target.rotation;

        // UI Panelini hedef nesnenin arkasına yerleştir
        uiPanel.transform.position = targetPosition;
        
        // UI Panelinin yalnızca y ekseninde dönmesini sağla
        uiPanel.transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
    }
}
