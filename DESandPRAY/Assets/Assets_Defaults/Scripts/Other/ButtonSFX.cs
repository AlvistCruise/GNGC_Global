using UnityEngine;
using UnityEngine.EventSystems; // Wajib ditambahkan untuk deteksi hover UI

public class ButtonSFX : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    // Fungsi ini terpanggil otomatis saat mouse mulai menyorot tombol (Hover)
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    // Fungsi ini terpanggil otomatis saat tombol diklik (Mouse Down)
    public void OnPointerDown(PointerEventData eventData)
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}