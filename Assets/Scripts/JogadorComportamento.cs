using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public class JogadorComportamento : MonoBehaviour
{
    public enum TipoMovimentoHorizontal
    {
        Acelerometro,
        Touch
    }

    public TipoMovimentoHorizontal movimentoHorizontal = TipoMovimentoHorizontal.Acelerometro;

    /// <summary>
    /// Uma referencia para o componente RigidBody
    /// </summary>
    private Rigidbody rb;

    [Tooltip("A velocidade que o jogador irá se esquivar para os lados")]
    [Range(0, 10)]
    public float velocidadeEsquiva = 5.0f;

    [Tooltip("Velocidade que o jogador irá se deslocar para a frente")]
    [Range(0, 10)]
    public float velocidadeRolamento = 5.0f;

    [Header("Atributos responsaveis pelo swipe")]
    [Tooltip("Determina qual a distancia que o dedo do jogador deve deslocar pela tela para ser considerado um swipe")]
    public float minDisSwipe = 2.0f;

    [Tooltip("Distancia que o jogaor ira perorrer atras do swipe")]
    public float swipeMove = 2.0f;

    /// <summary>
    /// Ponto inicial onde o swipe ocorreu
    /// </summary>
    private Vector2 toqueInicio;

    // Start is called before the first frame update
    void Start()
    {
        //Obter acesso ao componente RigidBody associado a esse GO (GameObject)
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (MenuPauseComp.pausado)
            return;

        //Verificar para qual lado o jogador deseja esquivar
        var velocidadeHorizontal = Input.GetAxis("Horizontal") * velocidadeEsquiva;

#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBPLAYER

        // 0 - Botão Esquerdo - ou touch
        // 1 - Botão Direito
        // 2 - Botão do meio
        //Detectando se houve clique com o botao do mouse (op 0) ou toque na tela
        if (Input.GetMouseButton(0))
        {
            velocidadeHorizontal = CalculaMovimento(Input.mousePosition);
        }

#elif UNITY_IOS || UNITY_ANDROID

        if (movimentoHorizontal == TipoMovimentoHorizontal.Acelerometro)
        {
            velocidadeHorizontal = Input.acceleration.x * velocidadeRolamento;
        }
        else
        {
            // Detectando exclusivamente via touch
            if (Input.touchCount > 0)
            {
                //Obtendo o primeiro touch na tela dentro do frame
                Touch toque = Input.touches[0];
                velocidadeHorizontal = CalculaMovimento(toque.position);
                SwipeTeleport(toque);
            }
        }
#endif

        var forcaMovimento = new Vector3(velocidadeHorizontal, 0, velocidadeRolamento);
        //Time.delta nos retorna o tempo gasto no frame anterior
        //Algo em torno de 1/60fds
        //Usamos esse valor para garantir que o nosso jogador se desloque com a mesma velocidade
        //Não importa o hardware
        forcaMovimento *= (Time.deltaTime * 60);

        //Aplicar uma força para que a bola se desloque
        //rb.AddForce(velocidadeHorizontal, 0, velocidadeRolamento);
        rb.AddForce(forcaMovimento);

    }

    /// <summary>
    /// Metodo para calcular para onde o jogador se deslocara na horizontal
    /// </summary>
    /// <param name="screenSpaceCoord">As coordenadas no Screen Space</param>
    /// <returns></returns>
    private float CalculaMovimento(Vector2 screenSpaceCoord)
    {
        var pos = Camera.main.ScreenToViewportPoint(screenSpaceCoord);
        float direcaoX = 0;

        if (pos.x < 0.5)
            direcaoX = -1;
        else
            direcaoX = 1;

        return direcaoX * velocidadeEsquiva;

    }


    private void SwipeTeleport(Touch toque)
    {
        //Verifica se é o ponto onde o swipe comecou
        if (toque.phase == TouchPhase.Began)
        {
            toqueInicio = toque.position;
        }

        //verifica se o swipe acabou
        else if (toque.phase == TouchPhase.Ended)
        {
            Vector2 toqueFim = toque.position;
            Vector3 direcaoMov;

            //Faz a direnca entre o ponto final e o inicial do swipe
            float dif = toqueFim.x - toqueInicio.x;

            //Verifica se o swipe percorreu uma distancia suficiente para
            //ser reconhecido como swipe
            if (Mathf.Abs(dif) >= minDisSwipe)
            {
                //Determina a direcao do swipe
                if (dif < 0)
                {
                    direcaoMov = Vector3.left;
                }
                else
                {
                    direcaoMov = Vector3.right;
                }
            }
            else
                return;

            //Raycast é outra forma de detercar colisao
            RaycastHit hit;

            //Verifica se o swipe nao vai causar colisao
            if (!rb.SweepTest(direcaoMov, out hit, swipeMove))
                rb.MovePosition(rb.position + (direcaoMov * swipeMove));

        }

    }
}
