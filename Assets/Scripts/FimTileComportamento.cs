using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class FimTileComportamento : MonoBehaviour
{
    [Tooltip("Tempo esperado antes de destruir o TileBasico")]
    public float tempoDestruir = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        ControladorJogo.pontuacaoAtual += 100;
        VerificaPontuacao(ControladorJogo.pontuacaoAtual);

        var textPontuacaoGame = GameObject.Find("Canvas").transform.Find("ScoreGame").transform.Find("TextPontuaçãoGame").GetComponentInChildren<Text>();
        textPontuacaoGame.text = System.String.Format("{0}", ControladorJogo.pontuacaoAtual);

        var textMaiorPontuacao = GameObject.Find("Canvas").transform.Find("ScoreGlobal").transform.Find("TextPontuaçãoGlobal").GetComponentInChildren<Text>();
        textMaiorPontuacao.text = System.String.Format("{0}", ControladorJogo.maiorPontuacao);

        //Vamos ver se foi o jogador que passou pelo fim do TileBasico
        if (other.GetComponent<JogadorComportamento>())
        {
            //Como foi o jogador que passou, vamos criar um TileBasico no próximo ponto
            //Mas esse próximo ponto está depois do último TileBasico presente na cena
            GameObject.FindObjectOfType<ControladorJogo>().SpawnProxTile();

            //Destroi o TileBasico
            Destroy(transform.parent.gameObject, tempoDestruir);
        }
    }

    public void VerificaPontuacao(int pontuacaoAtual)
    {
        if (pontuacaoAtual > ControladorJogo.maiorPontuacao)
        {
            ControladorJogo.maiorPontuacao = pontuacaoAtual;
        }
    }

}
