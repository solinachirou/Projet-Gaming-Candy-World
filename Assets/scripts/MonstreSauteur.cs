using UnityEngine;
using System.Collections;

public class MonstreSauteur : MonoBehaviour
{
    [Header("Réglages du saut et avancée")]
    public float forceSautVertical = 8f; 
    public float vitesseHorizontale = 2f; 
    public float tempsEntreSauts = 2f; 
    public LayerMask layerSol; 

    private Rigidbody2D rb;
    private Animator anim; 
    private bool estAuSol = true; 
    private Transform groundCheck; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); 

        GameObject gc = new GameObject("GroundCheck");
        gc.transform.parent = this.transform;
        gc.transform.localPosition = new Vector3(0f, -0.45f, 0f); 
        groundCheck = gc.transform;

        StartCoroutine(RoutineSaut());
    }

    void Update()
    {
        estAuSol = Physics2D.OverlapCircle(groundCheck.position, 0.1f, layerSol);

        if (anim != null)
        {
            anim.SetBool("EstAuSol", estAuSol);
            anim.SetFloat("VitesseVerticale", rb.linearVelocity.y);
        }

        // --- LA CORRECTION EST ICI ---
        // On empêche de glisser SEULEMENT si la guimauve est au sol ET qu'elle ne monte pas (y <= 0.1f)
        if (estAuSol && rb.linearVelocity.y <= 0.1f)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
    }

    private IEnumerator RoutineSaut()
    {
        while (true)
        {
            yield return new WaitForSeconds(tempsEntreSauts);

            if (estAuSol)
            {
                rb.linearVelocity = Vector2.zero; 
                
                // BOING ! Il saute en avant
                rb.AddForce(new Vector2(vitesseHorizontale, forceSautVertical), ForceMode2D.Impulse);
                
                estAuSol = false; 
            }
        }
    }
}