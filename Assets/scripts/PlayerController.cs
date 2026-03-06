using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // --- LES ÉTATS DU PERSONNAGE ---
    public enum Taille { Petit, Normal }
    public Taille tailleActuelle = Taille.Normal;

    // --- VARIABLES DE MOUVEMENT ---
    [Header("Paramètres de déplacement")]
    public float vitesseMarche = 5f;
    public float vitesseCourse = 8f; // Sera utilisée plus tard pour le boost
    public float forceSaut = 7f;
    
    private float vitesseActuelle;
    private bool estAuSol = false;

    // --- COMPOSANTS ---
    private Rigidbody2D rb;

    void Start()
    {
        // On récupère le composant physique de notre personnage
        rb = GetComponent<Rigidbody2D>();
        
        // Initialisation
        vitesseActuelle = vitesseMarche;
        tailleActuelle = Taille.Normal;
    }

    void Update()
    {
        Avancer();
        Sauter();
    }

    void Avancer()
    {
        // Input.GetAxis renvoie -1 (gauche), 0 (immobile) ou 1 (droite)
        float mouvementX = Input.GetAxis("Horizontal");
        
        // On modifie la vélocité (vitesse) sur l'axe X, mais on garde la vélocité Y actuelle (pour la gravité)
        rb.linearVelocity = new Vector2(mouvementX * vitesseActuelle, rb.linearVelocity.y);
    }

    void Sauter()
    {
        // Si on appuie sur la barre espace ET qu'on touche le sol
        if (Input.GetButtonDown("Jump") && estAuSol)
        {
            rb.AddForce(new Vector2(0f, forceSaut), ForceMode2D.Impulse);
            estAuSol = false; // On est en l'air maintenant
        }
    }

    // --- DÉTECTION DU SOL ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si le personnage touche un objet avec le tag "Sol"
        if (collision.gameObject.CompareTag("Sol"))
        {
            estAuSol = true;
        }
    }
}