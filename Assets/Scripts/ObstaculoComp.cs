using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObstaculoComp : MonoBehaviour
{
    [Tooltip("Quanto tempo antes de reiniciar o jogo")]
    public float tempoEspera = 2.0f;

    [Tooltip("Particle System da explosão")]
    public GameObject explosao;

    /// <summary>
    /// Variavel referencia para o jogador
    /// </summary>
    private GameObject jogador;

    [Tooltip("Acesso para o componente Mesh Renderer")]
    MeshRenderer mr = new MeshRenderer();

    [Tooltip("Acesso para o componente Box Collider")]
    BoxCollider bc = new BoxCollider();

    private void OnCollisionEnter(Collision collision)
    {
        //Verificando se é o jogador
        if (collision.gameObject.GetComponent<JogadorComportamento>())
        {

            collision.gameObject.SetActive(false);
            jogador = collision.gameObject;

            //Destroy o jogador, que nesse contexto é o collision
            //Destroy(collision.gameObject);
            //Chama o método que reinicia o jogo
            Invoke("ResetaJogo", tempoEspera);

        }
    }

    /// <summary>
    /// Reseta o level (fase)
    /// </summary>
    void ResetaJogo()
    {

        var gameOverMenu = GetGameOverMenu();
        gameOverMenu.SetActive(true);

        var botoes = gameOverMenu.transform.GetComponentsInChildren<Button>();

        Button botaoContinue = null;

        foreach (var botao in botoes)
        {
            if (botao.gameObject.name.Equals("BotaoContinuar"))
            {
                botaoContinue = botao;
                break;
            }
        }

        if (botaoContinue)
        {
            StartCoroutine(ShowContinue(botaoContinue));

            //botaoContinue.onClick.AddListener(UnityAdControle.ShowRewardAd);
            //UnityAdControle.obstaculo = this;
        }

        //Reinicia o jogo
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator ShowContinue(Button botaoContinue)
    {
        var btnText = botaoContinue.GetComponentInChildren<Text>();

        while (true)
        {

            if (UnityAdControle.proxTempReward.HasValue && (DateTime.Now < UnityAdControle.proxTempReward.Value))
            {
                botaoContinue.interactable = false;

                TimeSpan restante = UnityAdControle.proxTempReward.Value - DateTime.Now;

                var contagemRegressiva = String.Format("{0:D2}:{1:D2}", restante.Minutes, restante.Seconds);

                btnText.text = contagemRegressiva;

                yield return new WaitForSeconds(1f);
            }
            else
            {
                botaoContinue.interactable = true;
                botaoContinue.onClick.AddListener(UnityAdControle.ShowRewardAd);
                UnityAdControle.obstaculo = this;
                btnText.text = "Continuar (Ad)";
                break;
            }

        }
    }

    /// <summary>
    /// Faz o continue do jogo
    /// </summary>
    public void Continue()
    {
        var go = GetGameOverMenu();
        go.SetActive(false);
        jogador.SetActive(true);
        ObjetoTocado();
    }

    /// <summary>
    /// Busca o MenuGameOver
    /// </summary>
    /// <returns>Retorna o MenuGameOver</returns>
    GameObject GetGameOverMenu()
    {
        return GameObject.Find("Canvas").transform.Find("MenuGameOver").gameObject;
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

            explosao.GetComponent<AudioSource>().Play();

        }
        //Desabilitando o componente Mesh Renderer para não mostrar mais o componente Obstaculo
        mr.enabled = false;
        //Desabilitando a colisão do componente Obstaculo, para que não seja mais possivel a interação com o objeto
        bc.enabled = false;

        Destroy(this.gameObject);

    }
}
