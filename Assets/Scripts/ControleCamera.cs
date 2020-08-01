using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class ControleCamera : MonoBehaviour
{
    [Tooltip("O alvo a ser acompanhado pela câmera")]
    public Transform alvo;

    [Tooltip("Offset da câmera em relação ao alvo")]
    public Vector3 offset = new Vector3(0, 3, -6);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (alvo != null){
            //Altera a posição da câmera
            transform.position = alvo.position + offset;

            //Altar a rotação da câmera em relação ao jogador
            transform.LookAt(alvo);
        }

    }
}
