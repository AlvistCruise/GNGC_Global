// using System.Collections.Generic;
using UnityEngine;

public class DesktopManager : MonoBehaviour
{
    [Header("Window Settings")]
    // Masukkan 4 GameObject Window dari Hierarchy ke array ini melalui Inspector.
    // PASTIKAN URUTANNYA SESUAI ID!
    // Element 0 = Window Explorer
    // Element 1 = Window Email
    // Element 2 = Window Discord
    // Element 3 = Window ArtInspector
    public GameObject[] windows; 
    [Header("Objective Window Settings")]
    // Tambahkan variabel ini untuk menyimpan referensi Window Objective
    public GameObject objectiveWindow;

    private InputManager inputManager;

    void Start()
    {
        inputManager = gameObject.GetComponent<InputManager>();

        // Saat game mulai, pastikan semua window aplikasi disembunyikan
        foreach (GameObject win in windows)
        {
            if (win != null) win.SetActive(false);
        }
        
        // Pastikan Window Objective menyala di awal karena belum ada app yang terbuka
        if (objectiveWindow != null) objectiveWindow.SetActive(true);
    }

    void Update()
    {
        if(inputManager.newClick)
        {
            GameObject clickedObj = inputManager.clickedObj;

            // Buka Aplikasi
            if(clickedObj.CompareTag("App"))
            {
                int clickedId = clickedObj.GetComponentInParent<AppScript>().Id;
                OpenWindow(clickedId);
            }

            // Drag Window
            if(clickedObj.CompareTag("DragWindow"))
            {
                int clickedWindowId = clickedObj.GetComponentInParent<WindowScript>().Id;
                Debug.Log($"Drag window id: {clickedWindowId}");
            }

            // Tutup/Minimize Window
            if(clickedObj.CompareTag("CloseWindow"))
            {
                int clickedWindowId = clickedObj.GetComponentInParent<WindowScript>().Id;
                Debug.Log($"Closed App id: {clickedWindowId}");
                MinimizeWindow(clickedWindowId);
            }
        }
    }

    public void OpenWindow(int AppId)
    {
        Debug.Log($"Membuka aplikasi ID: {AppId}");

        // Lapor ke GameManager
        if(GameManager.Instance != null) 
        {
            GameManager.Instance.OnAppOpened(AppId);
        }

        // Buka app yang dipilih, tutup yang lain
        for(int i = 0; i < windows.Length; i++)
        {
            if (windows[i] != null)
            {
                if (i == AppId)
                {
                    windows[i].SetActive(true); 
                }
                else
                {
                    windows[i].SetActive(false); 
                }
            }
        }

        // MATIKAN Window Objective karena sekarang ada aplikasi yang sedang terbuka
        if (objectiveWindow != null) objectiveWindow.SetActive(false);
    }

    public void MinimizeWindow(int AppId)
    {
        // Matikan window yang sedang aktif
        if (AppId >= 0 && AppId < windows.Length && windows[AppId] != null)
        {
            windows[AppId].SetActive(false);
        }

        // Cek apakah Window Objective harus ditampilkan kembali
        CheckObjectiveVisibility();
    }

    public void Shutdown()
    {
        // TODO: Logika saat komputer dimatikan
    }

    // --- FUNGSI BARU ---
    // Mengecek apakah masih ada aplikasi yang terbuka di layar
    private void CheckObjectiveVisibility()
    {
        if (objectiveWindow == null) return;

        bool isAnyAppOpen = false;
        
        // Cek satu per satu apakah ada window di dalam array yang statusnya sedang aktif
        foreach (GameObject win in windows)
        {
            if (win != null && win.activeSelf)
            {
                isAnyAppOpen = true;
                break; // Jika ketemu satu saja yang aktif, langsung stop pengecekan
            }
        }

        // Jika isAnyAppOpen = true, maka SetActive(false)
        // Jika isAnyAppOpen = false, maka SetActive(true)
        objectiveWindow.SetActive(!isAnyAppOpen);
    }
}