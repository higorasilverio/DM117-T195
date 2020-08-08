using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstaculoComp : MonoBehaviour
{
    [Tooltip("Quanto tempo antes de reiniciar o jogo")]
    public float tempoEspera = 2.0f;

    [Tooltip("Particle System da explosão")]
    public GameObject explosao;

    [Tooltip("Acesso para o componente Mesh Renderer")]
    MeshRenderer mr = new MeshRenderer();

    [Tooltip("Acesso para o componente Box Collider")]
    BoxCollider bc = new BoxCollider();

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
        //As linhas abaixo são utilizadas para acessar  os componentes Mesh Renderer e Box Collider, respectivamente
        mr = GetComponent<MeshRenderer>();
        bc = GetComponent<BoxCollider>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            TocarObjetos(Input.mousePosition);
        }
    }

    /// <summary>
    /// Método para identificar se objetos foram tocados
    /// </summary>
    /// <param name="toque">O toque ocorrido nesse frame</param>
    private static void TocarObjetos(UnityEngine.Vector2 toque)
    {
        //Convertemos a posição do toque (Screen Space) para o Ray
        Ray toqueRay = Camera.main.ScreenPointToRay(toque);

        //Objeto que ira salvar informações de um possível objeto que foi tocado
        RaycastHit hit;

        if (Physics.Raycast(toqueRay, out hit))
        {
            hit.transform.SendMessage("ObjetoTocado", SendMessageOptions.DontRequireReceiver);
        }

    }

    /// <summary>
    /// Objeto invocado através do SendMessage(), para detectar que este objeto foi tocado
    /// </summary>
    public void ObjetoTocado()
    {

        if (explosao != null)
        {
            //Cria um efeito de explosão
            var particulas = Instantiate(explosao, transform.position, UnityEngine.Quaternion.identity);

            Destroy(particulas, 1.0f);

        }
        //Desabilitando o componente Mesh Renderer para não mostrar mais o componente Obstaculo
        mr.enabled = false;
        //Desabilitando a colisão do componente Obstaculo, para que não seja mais possivel a interação com o objeto
        bc.enabled = false;
        Destroy(this.gameObject);

    }
}
