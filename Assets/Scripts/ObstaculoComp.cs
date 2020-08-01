using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstaculoComp : MonoBehaviour
{
    [Tooltip("Quanto tempo antes de reiniciar o jogo")]
    public float tempoEspera = 2.0f;

    private void OnCollisionEnter(Collision collision)
    {
        //Verificando se é o jogador
        if (collision.gameObject.GetComponent<JogadorComportamento>())
        {
            //Destroy o jogador, que nesse contexto é o collision
            Destroy(collision.gameObject);
            //Chama o método que reinicia o jogo
            Invoke("ResetaJogo", tempoEspera);

        }
    }

    /// <summary>
    /// Reseta o level (fase)
    /// </summary>
    void ResetaJogo()
    {
        //Reinicia o jogo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
