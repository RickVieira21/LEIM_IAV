using UnityEngine;
using extOSC;

public class Tabuleiro : MonoBehaviour
{
    OSCReceiver OSCreceiver;
    float alpha, beta, gamma;
    public Vector3 initialPosition;
    public GameObject[] saidas;
    public Bola bola;


    void Start()
    {
        bola.ReiniciarJogo();

        OSCreceiver = gameObject.AddComponent<OSCReceiver>();
        OSCreceiver.LocalPort = 8000;

        OSCreceiver.Bind("/orientation/alpha", TabuleiroAlpha);
        OSCreceiver.Bind("/orientation/beta", TabuleiroBeta);
        OSCreceiver.Bind("/orientation/gamma", TabuleiroGamma);
    }

    void TabuleiroAlpha(OSCMessage message) //Rotação Y
    {
        if (message.ToFloat(out float value))
        {
            alpha = value;
        }
    }

    void TabuleiroBeta(OSCMessage message) // Rotação X
    {
        if (message.ToFloat(out float value))
        {
            beta = value;
        }
    }

    void TabuleiroGamma(OSCMessage message) //Rotação Z
    {
        if (message.ToFloat(out float value))
        {
            gamma = value;
        }
    }


    public void ConfigurarSaidas()
    {
        // Ativar todas as saídas inicialmente
        foreach (GameObject saida in saidas)
        {
            saida.SetActive(true);
        }

        // Desativar uma saída das 3
        int indiceAleatorio = Random.Range(0, saidas.Length);
        saidas[indiceAleatorio].SetActive(false);
    }

    void FixedUpdate()
    {

       //Debug.Log($"Alpha: {alpha}, Beta: {beta}, Gamma: {gamma}");

       //Ignorar Y, só deve rodar em X e Z 
       transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(beta, 0, gamma), 0.1f);
    }
}
