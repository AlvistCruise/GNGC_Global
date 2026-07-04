using UnityEngine;
using UnityEngine.SceneManagement; // Wajib untuk mengatur pindah scene

public class MainMenuManagera : MonoBehaviour
{
    // Fungsi untuk tombol Start
    public void PlayGame()
    {
        // Ganti "SampleScene" dengan nama file Scene game utama kamu
        // Pastikan huruf besar/kecil dan spasinya sama persis!
        SceneManager.LoadScene("SampleScene"); 
    }

    // Fungsi ekstra untuk tombol Exit sekalian
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game ditutup!"); // Hanya muncul di editor Unity
    }
}