using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatidaParedes : MonoBehaviour
{
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<JogadorComportamento>())
        {

            collision.gameObject.GetComponent<AudioSource>().Play();

        }
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
