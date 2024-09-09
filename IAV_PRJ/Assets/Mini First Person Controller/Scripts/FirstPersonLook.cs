using UnityEngine;
using extOSC;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField]
    Transform character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;

    OSCReceiver OSCreceiver;
    float alpha, beta, gamma;
    Quaternion initialCharacterRotation;
    Quaternion initialCameraRotation;

    void Reset()
    {
        character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        OSCreceiver = gameObject.AddComponent<OSCReceiver>();
        OSCreceiver.LocalPort = 8000;

        OSCreceiver.Bind("/orientation/alpha", HandleAlpha);
        OSCreceiver.Bind("/orientation/beta", HandleBeta);
        OSCreceiver.Bind("/orientation/gamma", HandleGamma);

        initialCharacterRotation = character.localRotation;
        initialCameraRotation = transform.localRotation;
    }

    void HandleAlpha(OSCMessage message) // Rotação Y
    {
        if (message.ToFloat(out float value))
        {
            alpha = value;
        }
    }

    void HandleBeta(OSCMessage message) // Rotação X
    {
        if (message.ToFloat(out float value))
        {
            beta = value;
        }
    }

    void HandleGamma(OSCMessage message) // Rotação Z
    {
        if (message.ToFloat(out float value))
        {
            gamma = value;
        }
    }

    void Update()
    {
        // Normalizar angulos e aplicar sensividade
        float VerticalRotation = gamma * sensitivity; 
        float HorizontalRotation = beta * sensitivity; 

        // Clamp VerticalRotation a 90º para não poder olhar demasiado para baixo/cima
        VerticalRotation = Mathf.Clamp(VerticalRotation, -90f, 90f);

        // Clamp HorizontalRotation (não é necessário ser usado)
        //HorizontalRotation = Mathf.Clamp(HorizontalRotation, -180f, 180f);

        //VerticalRotation no character (No character para ele rodar para os lados)
        character.localRotation = Quaternion.Euler(0, HorizontalRotation, 0) * initialCharacterRotation;

        //HorizontalRotation na camera (Na camara porque não é suposto o character rodar para cima/baixo, só a camara)
        transform.localRotation = Quaternion.Euler(VerticalRotation, 0, 0) * initialCameraRotation;
    }
}









/*using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField]
    Transform character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;

    Vector2 velocity;
    Vector2 frameVelocity;


    void Reset()
    {
        // Get the character from the FirstPersonMovement in parents.
        character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    void Start()
    {
        // Lock the mouse cursor to the game screen.
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get smooth velocity.
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        // Rotate camera up-down and controller left-right from velocity.
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
    }
}
*/