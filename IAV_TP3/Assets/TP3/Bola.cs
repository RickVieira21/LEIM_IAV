using UnityEngine;
using UnityEngine.UI; 

public class Bola : MonoBehaviour
{
    public Vector3 initialPosition;
    private int nivel = 1;
    private int vidas = 3;

    public Tabuleiro tabuleiro;

    public TMPro.TextMeshProUGUI nivelText; 
    public TMPro.TextMeshProUGUI vidasText;

    void Start()
    {
        AtualizarUI();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Limite"))
        {
            Debug.Log("passei limite");
            vidas--;
            if(vidas <= 0)
            {
              Debug.Log("Game Over");
              Application.Quit();
            }
            ReiniciarJogo();
            AtualizarUI();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fim"))
        {
            Debug.Log("passei nivel");
            nivel++;
            ReiniciarJogo();
            AtualizarUI();
        }
    }

    public void ReiniciarJogo()
    {
        Debug.Log("Reiniciar tabuleiro");
        tabuleiro.gameObject.transform.rotation = Quaternion.identity; //Rotação inicial tabuleiro
        tabuleiro.ConfigurarSaidas();

        transform.position = new Vector3(12.5f, 1.0f, -12.2f); //Posição inicial bola
    }

    void AtualizarUI()
    {      
        nivelText.text = "Nível: " + nivel.ToString();
        vidasText.text = "Vidas: " + vidas.ToString();
    }

}
