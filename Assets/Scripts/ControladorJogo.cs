using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Advertisements;
using UnityEngine.UI;

/// <summary>
/// Classe para controlar a parte principal do jogo
/// </summary>
public class ControladorJogo : MonoBehaviour
{
    [Tooltip("Referência para o TileBasico")]
    public Transform tile;

    [Tooltip("Referencia para os obstaculos do jogo")]
    public Transform obstaculo;

    [Tooltip("Ponto para se colocar o TileBasicoInicial")]
    public Vector3 pontoInicial = new Vector3(0, 0, -5);

    [Tooltip("Quantidade de tiles iniciais")]
    [Range(1, 20)]
    public int numSpawnIni;

    [Tooltip("Número de Tiles sem obstaculos")]
    [Range(1, 4)]
    public int numTileSemOBS = 4;

    [Tooltip("Maior pontuação conseguida no jogo")]
    public static int maiorPontuacao = 0;

    [Tooltip("Pontuação da partida corrente")]
    public static int pontuacaoAtual = 0;

    /// <summary>
    /// Local para spawn do próximo tile
    /// </summary>
    private Vector3 proxTilePos;

    /// <summary>
    /// Rotação do próximo tile
    /// </summary>
    private Quaternion proxTileRot;

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize("2586157");

        proxTilePos = pontoInicial;
        var textMaiorPontuacao = GameObject.Find("Canvas").transform.Find("ScoreGlobal").transform.Find("TextPontuaçãoGlobal").GetComponentInChildren<Text>();
        textMaiorPontuacao.text = System.String.Format("{0}", ControladorJogo.maiorPontuacao);

        proxTileRot = Quaternion.identity;

        for (int i = 0; i < numSpawnIni; i++)
        {
            SpawnProxTile(i >= numTileSemOBS);
        }

    }

    public void SpawnProxTile(bool spawnObstaculos = true)
    {
        var novoTile = Instantiate(tile, proxTilePos, proxTileRot);

        var proxTile = novoTile.Find("PontoSpawn");
        proxTilePos = proxTile.position;
        proxTileRot = proxTile.rotation;

        //Verifica se já podemos criar Tiles com obstaculos
        if (!spawnObstaculos)
            return;

        //Devemos buscar todos os locais possíveis
        var pontosObstaculos = new List<GameObject>();

        foreach (Transform filho in novoTile)
        {
            //Vamos verificar se possui a TAG
            if (filho.CompareTag("ObsSpawn"))
            {
                //Adiciona na lista como potência ponto de spawn de obstaculo
                pontosObstaculos.Add(filho.gameObject);

            }
        }

        if (pontosObstaculos.Count > 0)
        {
            //Vamos pegar um ponto aleatório
            var pontoSpawn = pontosObstaculos[Random.Range(0, pontosObstaculos.Count)];

            //Vamos guardar a posição desse ponto de spawn
            var obsSpawnPos = pontoSpawn.transform.position;

            //Criar um novo obstaculo
            var novoObs = Instantiate(obstaculo, obsSpawnPos, Quaternion.identity);

            //Faz esse obstaculo ser filho de TileBasico
            novoObs.SetParent(pontoSpawn.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
