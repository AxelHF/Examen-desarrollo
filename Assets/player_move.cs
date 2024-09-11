using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_move : MonoBehaviour
{

    [Header("Componentes")]
    public Rigidbody2D theRB;

    [Header("Movimiento")]
    private float movimientoHorizontal = 0f;
    [SerializeField] private float velocidadDeMovimiento;
    [Range(0, 0.3f)][SerializeField] private float suavizadoDeMovimiento;
    private Vector2 velocidad;
    private float inputX;


    [Header("Salto")]
    [SerializeField] private float fuerzaDeSalto;
    [SerializeField] public Transform controladorSuelo;
    [SerializeField] public LayerMask queEsSuelo;
    [SerializeField] public Vector2 dimensionesCaja;
    [SerializeField] public bool enSuelo;
    private bool salto = false;

    [Header("Salto Regulable")]
    [Range(0, 1)][SerializeField] private float multiplicadorCancelarSalto;
    [SerializeField] private float multiplicadorGravedad;
    private float escalaGravedad;
    private bool botonSaltoArriba = true;

    [Header("Salto Regulable")]
    private int monedas = 0;

    [Header("Escena Créditos")]
    [SerializeField] private GameObject creditos;

    // Start is called before the first frame update
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        escalaGravedad = theRB.gravityScale;

    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        movimientoHorizontal = inputX * velocidadDeMovimiento;

        if (Input.GetButton("Jump") && enSuelo)
        {
            salto = true;
        }
        else
        {
            salto = false;
        }

    }

    private void FixedUpdate()
    {
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);
        Mover(movimientoHorizontal * Time.fixedDeltaTime, salto);

        if (theRB.velocity.y < 0 && !enSuelo)
        {
            theRB.gravityScale = escalaGravedad * multiplicadorGravedad;
        }
        else
        {
            theRB.gravityScale = escalaGravedad;
        }

    }

    private void Mover(float mover, bool saltar)
    {
        Vector2 velocidadObjetivo = new Vector2(mover, theRB.velocity.y);
        theRB.velocity = Vector2.SmoothDamp(theRB.velocity, velocidadObjetivo, ref velocidad, suavizadoDeMovimiento);

        if(saltar && enSuelo)
        {
            Salto();
        }
    }
    private void Salto()
    {
        enSuelo = false;
        //theRB.AddForce(new Vector2(0f, fuerzaDeSalto));
        theRB.velocity = new Vector2(0f, fuerzaDeSalto);
        salto = false;
        botonSaltoArriba = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TomarDano();
        }
        if (collision.gameObject.CompareTag("Moneda"))
        {
            SumarMoneda(collision.gameObject);
        }
    }

    private void TomarDano()
    {
        MostrarCreditos();
    }

    private void MostrarCreditos()
    {
        Destroy(gameObject);
        creditos.SetActive(true);
    }

    private void SumarMoneda(GameObject Moneda)
    {
        Destroy(Moneda);
        monedas++;
        if(monedas > 9)
        {
            MostrarCreditos();
        }
    }
}
