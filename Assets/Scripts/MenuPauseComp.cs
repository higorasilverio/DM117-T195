using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPauseComp : MonoBehaviour
{

    public static bool pausado;

    [SerializeField]
    private GameObject menuPausePanel;

    /// <summary>
    /// Metodo para reiniciar a scene
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Metodo utilizado para pausar o jogo
    /// </summary>
    /// <param name="isPausado">Valor booleano recebido do Unity, dizendo se o jogo está pausado ou não</param>
    public void Pause(bool isPausado)
    {
        pausado = isPausado;

        Time.timeScale = (pausado) ? 0 : 1;

        menuPausePanel.SetActive(pausado);
    }

    /// <summary>
    /// Metodo para carregar uma scene
    /// </summary>
    /// <param name="nomeScene">Nome da scene que será carregada</param>
    public void CarregaScene(string nomeScene)
    {
        SceneManager.LoadScene(nomeScene);
    }

    // Start is called before the first frame update
    void Start()
    {
        //pausado = false;
        Pause(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
