using UnityEngine;
using System.Collections; 
using UnityEngine.SceneManagement;
using TMPro; 

public class PlayerController : MonoBehaviour
{
    public enum Taille { Petit, Normal, Mort } 
    public Taille tailleActuelle = Taille.Normal;

    [Header("Paramètres de déplacement")]
    public static int viesActuelles = 5;
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

    [Header("Interface (UI)")]
    public TMP_Text affichageVies;
    public GameObject ecranMort;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>(); 
        col = GetComponent<Collider2D>(); 
        
        vitesseActuelle = vitesseMarche;
        tailleActuelle = Taille.Normal;

        if (affichageVies != null) affichageVies.text = "Vies : " + viesActuelles;
        if (ecranMort != null) ecranMort.SetActive(false); // Cache le message de mort
    }

    void Update()
    {
        if (tailleActuelle == Taille.Mort) return; 

        Avancer();
        Sauter();

       
        // Si le personnage tombe trop bas (en dessous de -15 sur l'axe Y)
        if (transform.position.y < -15f)
        {
            Mourir(); 
        }
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
        // On vérifie si on n'est pas déjà en train de mourir pour éviter les bugs
        if (tailleActuelle == Taille.Mort) return; 
        
        tailleActuelle = Taille.Mort;
        viesActuelles = viesActuelles - 1; // On enlève une vie
        
        // On met à jour le texte à l'écran !
        if (affichageVies != null) affichageVies.text = "Vies : " + viesActuelles;

        // On lance la séquence de mort
        StartCoroutine(RoutineMort());
    }

    private IEnumerator RoutineMort()
    {
        // 1. On affiche le message "VOUS ÊTES MORT !"
        if (ecranMort != null) ecranMort.SetActive(true);

        // 2. On arrête le joueur (optionnel, mais ça fait propre)
        rb.linearVelocity = Vector2.zero; 

        // 3. LA PAUSE : On attend 2 secondes pour que le joueur lise le message
        yield return new WaitForSeconds(2f);

        // 4. Après la pause, on recharge le niveau ou le menu
        if (viesActuelles > 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            viesActuelles = 5; 
            SceneManager.LoadScene(0); // Retour au menu principal si 0 vies
        }
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

        // --- Pour la Nourriture ---
        if (collision.gameObject.CompareTag("Nourriture"))
        {
            Manger(); 
            Destroy(collision.gameObject); 
        }

        // --- NOUVEAU : Pour écraser le monstre ---
        if (collision.gameObject.CompareTag("PointFaible"))
        {
            // 1. On annule la vitesse de chute et on fait rebondir le joueur !
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); 
            rb.AddForce(new Vector2(0f, forceSaut * 0.8f), ForceMode2D.Impulse); // Un rebond un peu moins haut qu'un saut normal

            // 2. On détruit le monstre entier (qui est le "Parent" de ce point faible)
            Destroy(collision.transform.parent.gameObject);

            Debug.Log("YAY ! Monstre écrasé !");
        }
    }
}