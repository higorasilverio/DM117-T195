using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JogadorComportamento : MonoBehaviour
{
    /// <summary>
    /// Uma referência para o componente Rigidbody
    /// </summary>
    private Rigidbody rb;

    [Tooltip("Velocidade que o jogador irá se esquivar para os lados")]
    [Range(0, 10)]
    public float velocidadeEsquiva = 5.0f;

    [Tooltip("Velocidade que o jogador irá se deslocar para frente")]
    [Range(0, 10)]
    public float velocidadeRolamento = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        //Obter acesso ao componente Rigidibody associado a esse GO (GameObject)
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Verificar para qual lado o jogador se desloca
        var velocidadeHorizontal = Input.GetAxis("Horizontal") * velocidadeEsquiva;

        //Aplicar uma força para que a bola se desloque
        rb.AddForce(velocidadeHorizontal, 0, velocidadeRolamento);
    }
}
