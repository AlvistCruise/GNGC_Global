using UnityEngine;

public class AppScript : MonoBehaviour
{
    public int Id;
    public GameObject redDotNotif;
    
    // 1. Deklarasikan variabel animator di sini
    private Animator animator; 

    void Awake() 
    {
        // Awake dipanggil paling pertama kali saat objek aktif, sebelum Start() manapun berjalan.
        animator = GetComponent<Animator>(); 
    }

    public void SetShake(bool state)
    {
        if (animator != null)
        {
            // Namanya harus persis dengan yang ada di Animator
            animator.SetBool("IsShake", state);
        }
        else
        {
            Debug.LogWarning($"Animator belum terpasang di objek {gameObject.name}");
        }
    }

    public void SetRedDotActive(bool status)
    {
        if (status)
        {
            redDotNotif.SetActive(true);
        } else
        {
            redDotNotif.SetActive(false);
        }
    }
}