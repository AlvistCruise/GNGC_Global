using UnityEngine;
using UnityEngine.Events; // Kita pakai ini agar tombol bisa menjalankan perintah/fungsi lain dari Inspector

public class CustomButton2D : MonoBehaviour
{
    [Header("Pengaturan Sprite")]
    public Sprite defaultSprite; // Tampilan normal
    public Sprite hoverSprite;   // Tampilan saat mouse di atas tombol
    public Sprite clickSprite;   // Tampilan saat tombol ditekan

    [Header("Aksi Tombol")]
    public UnityEvent onClick;   // Event yang akan dijalankan saat tombol diklik

    private SpriteRenderer spriteRenderer;
    private bool isHovering = false; // Deteksi apakah mouse sedang di atas tombol

    void Start()
    {
        // Mengambil komponen SpriteRenderer dari objek ini
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Set tampilan ke default saat mulai
        if (defaultSprite != null)
        {
            spriteRenderer.sprite = defaultSprite;
        }
    }

    // Terpanggil saat kursor mouse MASUK ke area BoxCollider2D
    void OnMouseEnter()
    {
        isHovering = true;
        if (hoverSprite != null) 
        {
            spriteRenderer.sprite = hoverSprite;
        }
    }

    // Terpanggil saat kursor mouse KELUAR dari area BoxCollider2D
    void OnMouseExit()
    {
        isHovering = false;
        if (defaultSprite != null) 
        {
            spriteRenderer.sprite = defaultSprite;
        }
    }

    // Terpanggil saat kursor mouse DITEKAN (Klik Kiri) di atas objek ini
    void OnMouseDown()
    {
        if (clickSprite != null) 
        {
            spriteRenderer.sprite = clickSprite;
        }
    }

    // Terpanggil saat klik mouse DILEPAS
    void OnMouseUp()
    {
        // Cek apakah saat dilepas, mouse masih berada di atas tombol
        if (isHovering)
        {
            // Kembali ke tampilan Hover
            if (hoverSprite != null) spriteRenderer.sprite = hoverSprite;
        }
        else
        {
            // Kembali ke tampilan normal jika mouse sudah bergeser keluar
            if (defaultSprite != null) spriteRenderer.sprite = defaultSprite;
        }
    }

    // Terpanggil HANYA JIKA klik ditekan DAN dilepas di atas objek yang sama (Klik Valid)
    void OnMouseUpAsButton()
    {
        Debug.Log($"Tombol {gameObject.name} berhasil diklik!");
        
        // Menjalankan semua fungsi yang kamu pasang di bagian "On Click" di Inspector
        onClick.Invoke(); 
    }
}