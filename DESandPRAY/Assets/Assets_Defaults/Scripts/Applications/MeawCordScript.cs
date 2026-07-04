using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Tambahkan ini jika pakai UI bawaan
using TMPro; // Wajib ditambahkan untuk TextMeshPro teks dialog

[System.Serializable]
public struct InterviewData 
{
    public int candidateId;
    public string candidateName;      // Nama kandidat
    public Sprite profilePicture;     // Gambar profil kandidat
    [TextArea(2, 3)]
    public string defaultGreeting;    // Teks sapaan awal saat profil diklik
    [TextArea(2, 5)] 
    public string[] answers; // Akan diisi 5 jawaban di Inspector
}

public class MeawCordScript : MonoBehaviour
{
    [Header("Chat Panels Settings")]
    // Masukkan GameObject Chat dari Hierarchy ke array ini.
    // Urutannya WAJIB sama dengan ID di ProfileId!
    // Contoh: Element 0 = AlviciaRoxiesChat, Element 1 = AlanDezainChat, dst.
    public GameObject[] chatPanels;
    public List<int> acceptedId;
    private InputManager inputManager;
    public ExplorerScript explorerScript;
    public ArtInspectorScript artInspectorScript;

    [Header("Day UI Containers")]
    // Masukkan GameObject Parent yang menampung semua UI (profil, chat) untuk Hari 1
    public GameObject day1Container; 
    // Masukkan GameObject Parent yang menampung semua UI (layar interview VC) untuk Hari 2
    public GameObject day2Container;

    [Header("Day 2: Interview Settings")]
    public GameObject[] day2ProfileButtons; 
    public InterviewData[] interviewDatabase; 
    
    [Header("Day 2: Interview UI References")]
    public GameObject interviewPanel;       // Panel utama wawancara
    public TMP_Text headerCandidateName;    // Teks nama di atas chat
    public SpriteRenderer headerProfileImage;       // Gambar profil di atas chat
    public TMP_Text dialogText;             // Teks jawaban/dialog
    public GameObject[] questionButtons;    // Tombol 5 pertanyaan

    // Untuk melacak jumlah pertanyaan per kandidat
    private int[] questionsAskedCount = new int[5]; 
    private int currentIntervieweeId = -1;
    public int finalHiredId = -1;
    

    void Awake()
    {
        acceptedId = new List<int>();
        chatPanels[0].SetActive(true);
        // Pastikan semua chat panel di-assign di Inspector
        if (chatPanels.Length == 0)
        {
            Debug.LogWarning("Chat panels belum di-assign di Inspector!");
        }
    }

    void Start()
    {
        inputManager = gameObject.GetComponent<InputManager>();

        // Saat Meawcord pertama kali dibuka, sembunyikan semua chat panel
        // (Atau kamu bisa set chatPanels[0] jadi true jika ingin ada chat default yang terbuka)
        // chatPanels[0].SetActive(true);
        
        foreach (GameObject chat in chatPanels)
        {
            if (chat != null) chat.SetActive(false);
        }
    }

    void Update()
    {
        if(inputManager.newClick)
        {
            GameObject clickedObj = inputManager.clickedObj;

            // Buka isi Chat berdasarkan Profil yang diklik
            if(clickedObj.CompareTag("DiscordProfile"))
            {
                int clickedId = clickedObj.GetComponentInParent<ProfileId>().Id;
                Debug.Log($"Membuka isi chat untuk ID: {clickedId}");
                
                OpenChat(clickedId);
            }

            if (clickedObj.CompareTag("DownloadCV"))
            {
                int clickedId = clickedObj.GetComponentInParent<ProfileId>().Id;
                explorerScript.AddFile(clickedId);
                artInspectorScript.AddFile(clickedId);
            }

            
            if (clickedObj.CompareTag("MeawcordReject"))
            {
                int clickedId = clickedObj.GetComponentInParent<ProfileId>().Id;
            }
            
            if (clickedObj.CompareTag("MeawcordAccept") && acceptedId.Count < 2)
            {
                int clickedId = clickedObj.GetComponentInParent<ProfileId>().Id;
                if (ValidateId(clickedId))
                {
                    clickedObj.SetActive(false);
                    acceptedId.Add(clickedId);
                }
            }
        }
    }
    
    public void OpenChat(int chatId)
    {
        // Loop semua chat panel: yang ID-nya sama di-True, yang lain di-False
        for(int i = 0; i < chatPanels.Length; i++)
        {
            if (chatPanels[i] != null)
            {
                if (i == chatId)
                {
                    chatPanels[i].SetActive(true); // Tampilkan chat yang dipilih
                }
                else
                {
                    chatPanels[i].SetActive(false); // Sembunyikan chat yang lain
                }
            }
        }
    }

    private bool ValidateId(int id)
    {
        foreach (int _id in acceptedId)
        {
            if(id == _id)return false;
        }
        return true;
    }

    public void UpdateDayUI(int day)
    {
        if (day == 1)
        {
            if (day1Container != null) day1Container.SetActive(true);
            if (day2Container != null) day2Container.SetActive(false);
            Debug.Log("Meawcord: Memuat UI Hari 1 (Review CV)");
        }
        else if (day == 2)
        {
            if (day1Container != null) day1Container.SetActive(false);
            if (day2Container != null) day2Container.SetActive(true);
            SetupDay2Profiles();
            Debug.Log("Meawcord: Memuat UI Hari 2 (Interview Mode)");
        }
    }

    public void SetupDay2Profiles()
    {
        // Reset hitungan pertanyaan
        for (int i = 0; i < questionsAskedCount.Length; i++) questionsAskedCount[i] = 0;
        
        // Kondisi awal (Default/Blank state)
        currentIntervieweeId = -1;
        headerCandidateName.text = "Pilih Kandidat";
        dialogText.text = "Pilih kandidat di sebelah kiri untuk memulai interview.";
        // Opsional: headerProfileImage.sprite = defaultBlankSprite;

        // Loop semua tombol profil di Day 2
        for (int i = 0; i < day2ProfileButtons.Length; i++)
        {
            if (day2ProfileButtons[i] != null)
            {
                int id = day2ProfileButtons[i].GetComponent<ProfileId>().Id;
                
                if (acceptedId.Contains(id))
                {
                    day2ProfileButtons[i].SetActive(true); // Tampilkan yang lolos
                }
                else
                {
                    day2ProfileButtons[i].SetActive(false); // Sembunyikan yang ditolak
                }
            }
        }
    }

    // 1. Dipanggil saat player klik ikon profil di Day 2
    // Dipanggil saat player klik ikon profil di Day 2 (kiri layar)
    public void SelectInterviewee(int id)
    {
        currentIntervieweeId = id;
        
        // Cari data kandidat di database berdasarkan ID yang diklik
        foreach (InterviewData data in interviewDatabase)
        {
            if (data.candidateId == id)
            {
                // Ubah UI sesuai data kandidat
                headerCandidateName.text = data.candidateName;
                
                // Cek agar tidak error jika sprite lupa dimasukkan
                if (data.profilePicture != null) 
                {
                    headerProfileImage.sprite = data.profilePicture;
                }
                
                // Tampilkan sapaan awal
                dialogText.text = data.defaultGreeting;
                
                break; // Hentikan pencarian jika sudah ketemu
            }
        }
        
        // Update status tombol pertanyaan (apakah masih bisa tanya atau jatahnya habis)
        UpdateQuestionButtonsState();
    }

    // 2. Dipanggil saat player menekan salah satu dari 5 tombol pertanyaan
    // Pertanyaan ke-1 indexnya 0, ke-2 indexnya 1, dst.
    public void AskQuestion(int questionIndex)
    {
        if (currentIntervieweeId == -1) return; // Cegah error jika belum pilih orang

        // Cek batasan 3 pertanyaan per kandidat
        if (questionsAskedCount[currentIntervieweeId] >= 3)
        {
            dialogText.text = "Kamu sudah menanyakan maksimal 3 pertanyaan kepada kandidat ini.";
            return;
        }

        // Cari jawaban yang sesuai di database
        foreach (InterviewData data in interviewDatabase)
        {
            if (data.candidateId == currentIntervieweeId)
            {
                // Tampilkan jawaban ke UI text
                dialogText.text = data.answers[questionIndex];
                
                // Tambah hitungan pertanyaan untuk kandidat ini
                questionsAskedCount[currentIntervieweeId]++;
                
                UpdateQuestionButtonsState();
                break;
            }
        }
    }

    // 3. Fungsi bantuan untuk mengunci tombol jika limit tercapai
    private void UpdateQuestionButtonsState()
    {
        bool canStillAsk = questionsAskedCount[currentIntervieweeId] < 3;
        
        foreach (GameObject btn in questionButtons)
        {
            // Set interactable tombol (jika menggunakan komponen Button bawaan Unity)
            // Atau setActive(false) jika kamu ingin menyembunyikannya
            btn.SetActive(canStillAsk); 
        }
    }

    // Sambungkan fungsi ini ke event On Click pada tombol "Accept" warna hijau
    public void HireCandidate()
    {
        // Pastikan player sudah mengklik salah satu profil (bukan blank state)
        if (currentIntervieweeId != -1)
        {
            finalHiredId = currentIntervieweeId;
            Debug.Log($"Kandidat {finalHiredId} resmi diterima! Lanjut ke Day 3.");
            
            // Masukkan logika lanjut hari di sini
            // GameManager.Instance.AdvanceDay();
        }
        else
        {
            Debug.LogWarning("Player menekan Accept tapi belum memilih profil siapapun!");
        }
    }
}