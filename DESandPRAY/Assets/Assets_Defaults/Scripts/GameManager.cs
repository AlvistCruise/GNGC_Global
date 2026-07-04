using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public int currentDay = 1;
    [Header("App References")]
    // Tambahkan variabel ini untuk menyambungkan icon dari Hierarchy
    public AppScript emailApp;
    public AppScript discordApp;
    public AppScript explorerApp;
    public AppScript artInspectorApp;

    [Header("Day 1 Progress")]
    // Tambahkan variabel penanda (flag) ini
    private bool hasOpenedEmail = false;
    private bool hasOpenedDiscord = false;
    private bool hasOpenedExplorer = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Mulai permainan dari Hari 1
        ChangeDayState(1);
    }

    // ==========================================
    // PENGATUR STATE HARI
    // ==========================================
    public void ChangeDayState(int day)
    {
        currentDay = day;
        Debug.Log($"--- MEMASUKI HARI KE-{currentDay} ---");

        if (day == 1) RunDay1();
        else if (day == 2) RunDay2();
        else if (day == 3) RunDay3();
    }

    // ==========================================
    // FUNGSI 3 HARI (DAY 1, DAY 2, DAY 3)
    // ==========================================
    private void RunDay1()
    {
        Debug.Log("Setup Day 1: Cek Email Bos & Review CV.");
        // Nanti kita taruh logika awal hari 1 di sini (misal: Email bergetar)
        // Bikin icon Email bergetar saat Day 1 dimulai
        if (emailApp != null) 
        {
            emailApp.SetShake(true);
        }
    }

    private void RunDay2()
    {
        Debug.Log("Setup Day 2: Waktunya Interview!");
        // Nanti kita taruh logika awal hari 2 di sini
    }

    private void RunDay3()
    {
        Debug.Log("Setup Day 3: Pengumuman Hasil dari Bos.");
        // Nanti kita taruh logika awal hari 3 di sini
    }

    // ==========================================
    // PENERIMA INPUT DARI DESKTOP MANAGER
    // ==========================================
    
    // Fungsi ini yang akan dipanggil oleh DesktopManager setiap kali ada ikon diklik
    public void OnAppOpened(int appId)
    {
        // Berdasarkan array IdName: {"Explorer", "Email", "Meawcord", "ArtInspection"}
        switch (appId)
        {
            case 0:
                HandleExplorer();
                break;
            case 1:
                HandleEmail();
                break;
            case 2:
                HandleDiscord();
                break;
            case 3:
                HandleArtInspector();
                break;
            default:
                Debug.LogWarning("App ID tidak dikenali!");
                break;
        }
    }

    // ==========================================
    // FUNGSI MASING-MASING IKON (4 IKON)
    // ==========================================
    private void HandleExplorer()
    {
        Debug.Log("Fungsi: Explorer (Folder) Dibuka.");
        // Logika apa yang terjadi jika folder dibuka di hari tertentu
        if (currentDay == 1 && hasOpenedDiscord && !hasOpenedExplorer)
        {
            if (explorerApp != null) explorerApp.SetShake(false);
            
            hasOpenedExplorer = true; // Kunci agar tidak terpanggil lagi
            Debug.Log("Tahap Tutorial Hari 1 Selesai!");
        }
    }

    private void HandleEmail()
    {
        Debug.Log("Fungsi: Email Dibuka.");
        // Logika apa yang terjadi jika email dibuka di hari tertentu
        // jika day 1, buka email dari bos
        if (currentDay == 1 && !hasOpenedEmail)
        {
            if (emailApp != null) emailApp.SetShake(false);
            if (discordApp != null) discordApp.SetShake(true);
            
            hasOpenedEmail = true; // Kunci agar jika diklik lagi, kode di dalam blok ini tidak dijalankan
        }
    }

    private void HandleDiscord()
    {
        Debug.Log("Fungsi: Meawcord (Discord) Dibuka.");
        // Logika apa yang terjadi jika discord dibuka di hari tertentu
        // jika day 1, setelah buka email bos lalu buka meawcord untuk mendapatkan CV dari kandidat
        // Logika untuk Day 1
        if (currentDay == 1 && hasOpenedEmail && !hasOpenedDiscord)
        {
            if (discordApp != null) discordApp.SetShake(false);
            if (explorerApp != null) explorerApp.SetShake(true);
            
            hasOpenedDiscord = true; // Kunci
        }
        
    }

    private void HandleArtInspector()
    {
        Debug.Log("Fungsi: Art Inspector Dibuka.");
        // Logika apa yang terjadi jika art inspector dibuka di hari tertentu
    }
}