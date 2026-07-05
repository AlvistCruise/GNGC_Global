using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public int currentDay = 1;
    // TAMBAHKAN INI: Nama scene Main Menu kamu (harus sama persis dengan yang ada di Project)
    public string mainMenuSceneName = "MainMenu";
    [Header("App References")]
    // Tambahkan variabel ini untuk menyambungkan icon dari Hierarchy
    public AppScript emailApp;
    public AppScript discordApp;
    public AppScript explorerApp;
    public AppScript artInspectorApp;

    // 1. TAMBAHKAN INI: Referensi ke MeawCordScript
    public MeawCordScript meawCordManager;

    [Header("Day 1 Progress")]
    // Tambahkan variabel penanda (flag) ini
    private bool hasOpenedEmail = false;
    private bool hasOpenedDiscord = false;
    private bool hasOpenedExplorer = false;
    private bool hasOpenedInspector = false;

    [Header("Day 3: Final Result UI")]
    public TMP_Text bosEmailText; 
    
    // TAMBAHKAN INI: Referensi ke GameObject tombol restart
    public GameObject restartButtonObject;

    [Header("Objective Settings")]
    // Tarik objek Text (TMP) dari dalam Window objective ke sini
    public TMP_Text objectiveText; 

    // Teks default untuk masing-masing hari (bisa diedit juga lewat Inspector)
    [TextArea(3, 5)]
    public string day1Objective = "- Open all shaking applications.\n- Follow the boss's instructions in the Email.\n\n*Hint: To proceed to the next day, click the power button in the bottom left corner.*";

    [TextArea(3, 5)]
    public string day2Objective = "- Open Meawcord.\n- Start interviewing the 2 candidates you have selected.";

    [TextArea(3, 5)]
    public string day3Objective = "- Open the Email.\n- Read the message from your boss.";

    [Header("Day Indicator Settings")]
    // Tarik objek "Day1_0" dari Hierarchy ke sini
    public SpriteRenderer dayIndicatorRenderer; 
    
    // Array untuk menampung 3 gambar sprite (Day 1, Day 2, Day 3)
    public Sprite[] daySprites;

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
    public void AdvanceDay()
    {
        // Jika masih Hari 1 atau 2, lanjut ke hari berikutnya
        if (currentDay < 3)
        {
            int nextDay = currentDay + 1;
            Debug.Log($"Komputer dimatikan... Menyimpan data... Lanjut ke Hari {nextDay}");
            
            // Panggil fungsi penggantian state hari
            ChangeDayState(nextDay);
        }
        else 
        {
            // Jika Shut Down ditekan pada Hari ke-3 (Game Selesai)
            Debug.Log("Siklus 3 Hari selesai! Komputer dimatikan dan kembali ke Main Menu.");
            
            // Pindah (Load) kembali ke scene Main Menu
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
    // ==========================================
    // PENGATUR STATE HARI
    // ==========================================
    public void ChangeDayState(int day)
    {
        currentDay = day;
        Debug.Log($"--- MEMASUKI HARI KE-{currentDay} ---");

        // --- TAMBAHKAN LOGIKA GANTI GAMBAR DI SINI ---
        // Pastikan referensi tidak kosong dan hari sesuai dengan jumlah gambar di array
        if (dayIndicatorRenderer != null && daySprites != null && day >= 1 && day <= daySprites.Length)
        {
            // Ingat: Array dimulai dari 0. Jadi Hari 1 akan memanggil daySprites[0].
            dayIndicatorRenderer.sprite = daySprites[day - 1];
        }

        // Panggil fungsi sesuai harinya
        if (day == 1) RunDay1();
        else if (day == 2) RunDay2();
        else if (day == 3) RunDay3();
    }

    // ==========================================
    // FUNGSI 3 HARI (DAY 1, DAY 2, DAY 3)
    // ==========================================

    // 3. UPDATE FUNGSI RUNDAY
    private void RunDay1()
    {
        Debug.Log("Setup Day 1: Cek Email Bos & Review CV.");
        
        // --- TAMBAHKAN BARIS INI ---
        if (objectiveText != null) objectiveText.text = day1Objective;
        
        UpdateAppVisibility(true, true, true, true);
        if (emailApp != null) emailApp.SetShake(true);
        if (meawCordManager != null) meawCordManager.UpdateDayUI(1);
        if (restartButtonObject != null) restartButtonObject.SetActive(false); 
    }

    private void RunDay2()
    {
        Debug.Log("Setup Day 2: Waktunya Interview!");
        
        // --- TAMBAHKAN BARIS INI ---
        if (objectiveText != null) objectiveText.text = day2Objective;
        
        UpdateAppVisibility(false, false, true, false);
        if (meawCordManager != null) meawCordManager.UpdateDayUI(2);
        if (discordApp != null) discordApp.SetShake(true); 
    }

    private void RunDay3()
    {
        Debug.Log("Setup Day 3: Pengumuman Hasil dari Bos.");
        
        // --- TAMBAHKAN BARIS INI ---
        if (objectiveText != null) objectiveText.text = day3Objective;
        
        UpdateAppVisibility(false, true, false, false);
        if (emailApp != null) emailApp.SetShake(true);

        if (meawCordManager != null)
        {
            int hiredId = meawCordManager.finalHiredId;
            EvaluateResult(hiredId);
        }

        if (restartButtonObject != null) restartButtonObject.SetActive(true);
    }

    // ==========================================
    // FUNGSI BARU: PENGATUR VISIBILITAS IKON DESKTOP
    // ==========================================
    private void UpdateAppVisibility(bool showExplorer, bool showEmail, bool showDiscord, bool showArtInspector)
    {
        // Akses gameObject dari komponen AppScript untuk mematikan/menyalakan ikonnya di Desktop
        if (explorerApp != null) explorerApp.gameObject.SetActive(showExplorer);
        if (emailApp != null) emailApp.gameObject.SetActive(showEmail);
        if (discordApp != null) discordApp.gameObject.SetActive(showDiscord);
        if (artInspectorApp != null) artInspectorApp.gameObject.SetActive(showArtInspector);
    }
    private void EvaluateResult(int hiredId)
    {
        // Ensure the email text reference is not empty
        if (bosEmailText == null)
        {
            Debug.LogWarning("Boss Email Text is not assigned in the GameManager!");
            return;
        }

        // --- INSERT WIN/LOSE CONDITIONS HERE ---
        // Assumption based on your data: Dimas (ID 3) is Human. 
        // The rest (0, 1, 2, 4) are AI. Adjust this ID to match your actual data.
        
        if (hiredId == 3) 
        {
            // WIN CONDITION (Player chose the Human)
            bosEmailText.text = "Subject: New Employee Performance\n\n" +
                                "Great job! The new designer you chose (Dimas) works incredibly well and naturally. " +
                                "The clients are very satisfied with his unique artistic touch. " +
                                "You have successfully saved our company's reputation from the AI invasion. " +
                                "I'll be giving you a raise next month!\n\n- CEO";
        }
        else if (hiredId != -1) 
        {
            // LOSE CONDITION (Player chose an AI, e.g., Alan, Taylor, etc.)
            bosEmailText.text = "Subject: WARNING LETTER!\n\n" +
                                "What did you do?! The new employee you hired was caught using AI for all the client projects! " +
                                "Our clients are furious after finding hands with 6 fingers in their final designs. " +
                                "Our company is being sued for copyright infringement. You're fired!\n\n- CEO";
        }
        else
        {
            bosEmailText.text = "Subject: Error\n\nYou haven't selected anyone, but you already shut down the computer!";
        }
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

        if (currentDay == 1 && !hasOpenedInspector)
        {   
            hasOpenedInspector = true; // Kunci
        }
    }
}