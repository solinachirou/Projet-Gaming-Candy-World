using UnityEngine;
using System.Collections; 

public class PlayerController : MonoBehaviour
{
    public enum Taille { Petit, Normal, Mort } 
    public Taille tailleActuelle = Taille.Normal;

    [Header("Paramètres de déplacement")]
    public float vitesseMarche = 5f;
    public float vitesseCourse = 8f; // Vitesse rapide quand on mange !
    public float forceSaut = 7f;
    
    private float vitesseActuelle;
    private bool estAuSol = false;
    private bool estInvincible = false; 

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Collider2D col; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>(); 
        col = GetComponent<Collider2D>(); 
        
        vitesseActuelle = vitesseMarche;
        tailleActuelle = Taille.Normal;
    }

    void Update()
    {
        if (tailleActuelle == Taille.Mort) return; 

        Avancer();
        Sauter();
    }

    void Avancer()
    {
        float mouvementX = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(mouvementX * vitesseActuelle, rb.linearVelocity.y);

        if (mouvementX > 0) spriteRenderer.flipX = false; 
        else if (mouvementX < 0) spriteRenderer.flipX = true; 

        anim.SetFloat("Vitesse", Mathf.Abs(mouvementX)); 
    }

    void Sauter()
    {
        if (Input.GetButtonDown("Jump") && estAuSol)
        {
            rb.AddForce(new Vector2(0f, forceSaut), ForceMode2D.Impulse);
            estAuSol = false; 
        }
    }

    public void PrendreDegat()
    {
        Debug.Log("OUCH ! Le jeu m'a fait prendre un dégât !"); // AJOUTE CETTE LIGNE

        if (estInvincible || tailleActuelle == Taille.Mort) return; 
        // ... la suite de ton code

        if (tailleActuelle == Taille.Normal)
        {
            tailleActuelle = Taille.Petit;
            anim.SetBool("EstPetit", true); 
            StartCoroutine(EffetDegatMario());
        }
        else if (tailleActuelle == Taille.Petit)
        {
            Mourir();
        }
    }

    // --- NOUVEAU : Fonction pour manger et grandir ---
    // --- FONCTION MODIFIÉE SELON TES RÈGLES ---
    public void Manger()
    {
        if (tailleActuelle == Taille.Petit)
        {
            // 1. S'il est petit : il redevient JUSTE grand (pas de vitesse)
            tailleActuelle = Taille.Normal;
            anim.SetBool("EstPetit", false); // Déclenche les nouvelles flèches de l'Animator
            Debug.Log("Miam ! Le personnage a grandi !");
        }
        else if (tailleActuelle == Taille.Normal)
        {
            // 2. S'il est DÉJÀ grand : il gagne le boost de vitesse
            StartCoroutine(BonusVitesse());
            Debug.Log("Boost de vitesse activé !");
        }
    }

    // --- NOUVEAU : Coroutine pour le boost de vitesse ---
    private IEnumerator BonusVitesse()
    {
        vitesseActuelle = vitesseCourse; // On passe à 8 de vitesse
        spriteRenderer.color = new Color(1f, 0.8f, 0.2f); // Optionnel : le joueur devient un peu doré !
        
        yield return new WaitForSeconds(4f); // Le boost dure 4 secondes
        
        vitesseActuelle = vitesseMarche; // Retour à la vitesse normale
        spriteRenderer.color = Color.white; // Retour à la couleur normale
    }

    private IEnumerator EffetDegatMario()
    {
        estInvincible = true;
        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.4f); 
            yield return new WaitForSeconds(0.15f); 
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f); 
            yield return new WaitForSeconds(0.15f);
        }
        estInvincible = false; 
    }

    private void Mourir()
    {
        tailleActuelle = Taille.Mort;
        rb.linearVelocity = Vector2.zero; 
        col.enabled = false; 
        spriteRenderer.sortingOrder = 20; 
        rb.AddForce(new Vector2(0f, 4f), ForceMode2D.Impulse); 
        Destroy(gameObject, 3f); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tailleActuelle == Taille.Mort) return; 

        if (collision.gameObject.CompareTag("Sol"))
        {
            estAuSol = true;
        }

        if (collision.gameObject.CompareTag("Monstre"))
        {
            PrendreDegat();
        }
    }

    // --- NOUVEAU : Détection des objets à ramasser (Triggers) ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tailleActuelle == Taille.Mort) return;

        // Si on traverse un objet avec le tag "Nourriture"
        if (collision.gameObject.CompareTag("Nourriture"))
        {
            Manger(); // On appelle la fonction pour grandir/accélérer
            Destroy(collision.gameObject); // L'objet disparaît de la carte
        }
    }
}