using System;
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
    [SerializeField]
    [NotNull]
    Vector3 Select_dist_f_char;

    //privates
    Vector2 dir_Right;
    Vector2 dir_Left;
    bool right_select;
    bool left_select;
    bool right_back;
    bool left_back;
    bool start_button;

    [SerializeField]
    float delay_movement = 0.2f;
    float right_delay = 0f;
    float left_delay = 0f;

    [SerializeField]
    float movement_range = 0.2f;


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
        if (!(left_delay > 0) && player_left_block == -1 && Mathf.Abs(dir_Left.x) > movement_range)
        {
            if (dir_Left.x > 0)
            {

                player_left_index++;
                //derecha
                Debug.Log("p1_right");
            }
            else
            {
                //izquierda
                player_left_index--;
                Debug.Log("p1_left");

            }

            int next = player_left_index % Characters_masks.Length;
            if (next == -1)
            {
                next = Characters_masks.Length - 1;
            }
                player_left_index = next;
            //posicionar el indicador en el character seleccionado
            selector_p1.transform.position = Characters_masks[player_left_index].transform.position + Select_dist_f_char;

            left_delay = delay_movement;
        }

        if (!(right_delay > 0) && player_right_block == -1 && MathF.Abs(dir_Right.x) > movement_range)
        {
            if (dir_Right.x > 0)
            {
                player_right_index++;
                Debug.Log("p2_right");

                //derecha
            }
            else
            {
                Debug.Log("p2_left");
                player_right_index--;
            }


            int next = player_right_index % Characters_masks.Length;
            if (next  == -1)
            {
                next = Characters_masks.Length - 1;
            }
            player_right_index = next;
            //player_right_index = player_right_index % Characters_masks.Length;
            selector_p2.transform.position = Characters_masks[player_right_index].transform.position + Select_dist_f_char;

            right_delay = delay_movement;
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
      

        //leftAxes = context.ReadValue<Vector2>();
        dir_Left = context.ReadValue<Vector2>();

    }

    public void OnRightFingerMove(CallbackContext context)
    {
       

        //rightAxes = context.ReadValue<Vector2>();
        dir_Right = context.ReadValue<Vector2>();

    }

    public void OnSelect_Right(CallbackContext context)
    {
        Debug.Log("select_right");

        right_select = true;
        //right_select = context.ReadValue<bool>();
    }
    public void OnSelect_Left(CallbackContext context)
    {
        Debug.Log("select_left");

        left_select = true;
    }

    public void OnBack_Left(CallbackContext context)
    {
        Debug.Log("back_left");

        left_back = true;
    }

    public void OnBack_Right(CallbackContext context)
    {
        Debug.Log("back_right");

        right_back = true;
    }
    
    public void OnStart_Buttom(CallbackContext context)
    {
        Debug.Log("0");

        start_button = true;
        //si amabos jugadores tienes elegidos characters, permitir
    }
}
