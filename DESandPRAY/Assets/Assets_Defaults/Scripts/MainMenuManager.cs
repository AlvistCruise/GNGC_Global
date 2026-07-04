using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Wajib ditambahkan untuk urusan pindah Scene

public class MainMenuManager : MonoBehaviour
{
    private Animator animator;

    [Header("Pengaturan Scene")]
    public string nextSceneName = "SampleScene"; // Ganti dengan nama Scene game utamamu nanti
    public float delayBeforeLoad = 1.0f; // Waktu tunggu agar animasi selesai diputar (dalam detik)

    void Awake()
    {
        // Mengambil komponen Animator yang menempel pada tombol
        animator = GetComponent<Animator>();
    }

    // Fungsi ini yang akan dipanggil saat tombol diklik
    public void StartGame()
    {
        // 1. Ubah parameter isStart jadi true agar animasi PlayPower jalan
        if (animator != null)
        {
            animator.SetBool("isStart", true);
        }

        // 2. Jalankan Coroutine untuk menunggu sesaat lalu pindah scene
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        // Menunggu selama beberapa detik (sesuai durasi animasi tombolmu)
        yield return new WaitForSeconds(delayBeforeLoad);

        // Memuat scene baru
        SceneManager.LoadScene(nextSceneName);
    }
}