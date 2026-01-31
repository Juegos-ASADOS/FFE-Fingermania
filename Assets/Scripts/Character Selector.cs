using System.Diagnostics.CodeAnalysis;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEngine.InputSystem.InputAction;
public class CharacterSelector : MonoBehaviour
{
    [SerializeField]
    GameObject[] Characters_masks;
    [SerializeField]
    int rows = 1;
    [SerializeField]
    int cols = 1;
    [SerializeField]
    float separation_hor = 1f;
    [SerializeField]
    float separation_ver = 1f;

    int player_left_index = 0;
    int player_right_index = 0;
    //personaje blocj
    int player_left_block = -1;
    int player_right_block = -1;

    [SerializeField]
    [NotNull]
    GameObject selector_p1;
    [SerializeField]
    [NotNull]
    GameObject selector_p2;

    //privates
    Vector2 dir_Right;
    Vector2 dir_Left;
    bool right_select;
    bool left_select;
    bool right_back;
    bool left_back;
    bool start_button;

    float right_delay = 0f;
    float left_delay = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        Debug.Log("0");
    }
    void Start()
    {
        int index = 0;
        foreach (var character in Characters_masks)
        {
            float x = (index % rows) * separation_hor;
            float y = -(index / rows) * separation_ver;

            character.transform.position = this.transform.position + new Vector3(x, y, 0);
            //Instantiate(character, this.transform.position + new Vector3(x, y, 0), this.transform.rotation);

            index++;
        }

        Debug.Log("1");

    }

    private void Update()
    {
        right_delay -= Time.deltaTime;
        left_delay -= Time.deltaTime;
        //utilizando el player controller

        //hacer cosas
        //mover curosr derecho
        if (!(left_delay > 0) && player_left_block == -1 && dir_Left.magnitude > 0)
        {
            if (dir_Left.y > 0)
            {
                //derecha
                player_left_index++;
            }
            else
            {
                player_left_index--;
                //izquierda
            }

            int next = player_left_index % Characters_masks.Length;
            if (next == -1)
            {
                next = Characters_masks.Length - 1;
            }
                player_left_index = next;
            //posicionar el indicador en el character seleccionado
            selector_p1.transform.position = Characters_masks[player_left_index].transform.position;

            left_delay = 0.1f;
        }

        if (!(right_delay > 0) && player_right_block == -1 && dir_Right.magnitude > 0)
        {
            if (dir_Right.y > 0)
            {
                player_right_index++;
                //derecha
            }
            int next = player_right_index % Characters_masks.Length;
            if (next  == -1)
            {
                next = Characters_masks.Length - 1;
            }
            player_right_index = next;
            //player_right_index = player_right_index % Characters_masks.Length;
            selector_p2.transform.position = Characters_masks[player_right_index].transform.position;

            right_delay = 0.1f;
        }

        //bloquear selección
        if (right_select)
        {
            if (player_right_index != player_left_block)
            {
                player_right_block = player_right_index;
            }
        }

        if (left_select)
        {
            if (player_left_index != player_right_block)
            {
                player_left_block = player_left_index;
            }
        }

        //liberar seleccion e ir para atras, solo si eres jugador left.
        if (right_back)
        {
            player_right_block = -1;
        }
        if (left_back)
        {
            if (player_left_block == -1)
            {
                //volver escena atrás (menu)
                Debug.LogWarning("Implementar volver hacia atras");
            }
            player_left_block = -1;
        }


        if (start_button)
        {
            if (player_left_block != -1 && player_right_block != -1)
            {
                //ir a la pelea
                Debug.LogWarning("Implementar llevado a la escena");

            }
        }

        //reseteo de cosas
        dir_Left = new Vector2(0,0);
        dir_Right = new Vector2(0,0);
        right_back = false;
        left_back = false;
        start_button = false;
        right_select = false;
        left_select = false;
    }

    public void OnLeftFingerMove(CallbackContext context)
    {
        Debug.Log("0");

        //leftAxes = context.ReadValue<Vector2>();
        dir_Left = context.ReadValue<Vector2>();

    }

    public void OnRightFingerMove(CallbackContext context)
    {
        Debug.Log("0");

        //rightAxes = context.ReadValue<Vector2>();
        dir_Right = context.ReadValue<Vector2>();

    }

    public void OnSelect_Right(CallbackContext context)
    {
        Debug.Log("0");

        right_select = true;
        //right_select = context.ReadValue<bool>();
    }
    public void OnSelect_Left(CallbackContext context)
    {
        Debug.Log("0");

        left_select = true;
    }

    public void OnBack_Left(CallbackContext context)
    {
        Debug.Log("0");

        left_back = true;
    }

    public void OnBack_Right(CallbackContext context)
    {
        Debug.Log("0");

        right_back = true;
    }
    
    public void OnStart_Buttom(CallbackContext context)
    {
        Debug.Log("0");

        start_button = true;
        //si amabos jugadores tienes elegidos characters, permitir
    }
}
