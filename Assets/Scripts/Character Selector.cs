using FMODUnity;
using System;
using System.Diagnostics.CodeAnalysis;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;
public class CharacterSelector : MonoBehaviour
{
    [SerializeField]
    GameObject[] Characters_masks;

    int player_left_index = -2;
    int player_right_index = -2;
    //personaje block
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
    float left_back_delay = 0f;

    [SerializeField]
    float movement_range = 0.2f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
    }
    void Start()
    {
     

    }
    void SetOutline(GameObject character, Color color, float thickness)
    {
            var renderer = character.GetComponentInChildren<Renderer>();
            if (renderer == null) return;

            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(mpb);

            mpb.SetColor("_LineColor", color);
            mpb.SetFloat("_LineThickness", thickness);

            renderer.SetPropertyBlock(mpb);
    }

    private void Update()
    {
        
        left_back_delay -= Time.deltaTime;
        right_delay -= Time.deltaTime;
        left_delay -= Time.deltaTime;
        //utilizando el player controller

        //hacer cosas
        //mover curosr derecho
        if (!(left_delay > 0) && player_left_block == -1 && Mathf.Abs(dir_Left.x) > movement_range)
        {
            if (player_left_index == -2)
            {
                player_left_index = 0;
            }
            else
            {

                if (player_left_index != -1 && player_left_index != player_right_index)
                {
                    Characters_masks[player_left_index].transform.GetComponentInChildren<Animator>().SetBool("select", false);
                }
                if (dir_Left.x > 0)
                {

                    player_left_index++;
                    //derecha
                }
                else
                {
                    //izquierda
                    player_left_index--;

                }

            }
                RuntimeManager.PlayOneShot("event:/Soft Select");

                int next = player_left_index % Characters_masks.Length;
                if (next == -1)
                {
                    next = Characters_masks.Length - 1;
                }
                player_left_index = next;
                //posicionar el indicador en el character seleccionado
                selector_p1.transform.position = Characters_masks[player_left_index].transform.position + Select_dist_f_char;
                if (player_right_index != player_left_index)
                {
                    Characters_masks[player_left_index].transform.GetComponentInChildren<Animator>().SetBool("select", true);
                }

                left_delay = delay_movement;
        }

        if (!(right_delay > 0) && player_right_block == -1 && MathF.Abs(dir_Right.x) > movement_range)
        {
            if (player_right_index == -2)
            {
                player_right_index = 1;
            }
            else
            {
                if (player_right_index != -1 && player_left_index != player_right_index)
                {
                    Characters_masks[player_right_index].transform.GetComponentInChildren<Animator>().SetBool("select", false);
                }
                if (dir_Right.x > 0)
                {
                    player_right_index++;

                    //derecha
                }
                else
                {
                    player_right_index--;
                }
            }

            RuntimeManager.PlayOneShot("event:/Soft Hit 3D");

            int next = player_right_index % Characters_masks.Length;
            if (next == -1)
            {
                next = Characters_masks.Length - 1;
            }
            player_right_index = next;
            //player_right_index = player_right_index % Characters_masks.Length;
            selector_p2.transform.position = Characters_masks[player_right_index].transform.position + Select_dist_f_char;
            if (player_left_index != player_right_index)
            {
                Characters_masks[player_right_index].transform.GetComponentInChildren<Animator>().SetBool("select", true);
            }


            right_delay = delay_movement;
        }

        //bloquear selección
        if (right_select)
        {
            if (player_right_index >= 0 && player_right_index != player_left_block)
            {
                player_right_block = player_right_index;
                Characters_masks[player_right_index].transform.GetComponentInChildren<Animator>().SetBool("superselect", true);
            }
              SetOutline(
            Characters_masks[player_right_index],
            Color.red,
            0.2f
        );
        }

        if (left_select)
        {
            if (player_left_index >= 0 && player_left_index != player_right_block)
            {
                player_left_block = player_left_index;
                Characters_masks[player_left_index].transform.GetComponentInChildren<Animator>().SetBool("superselect", true);
                       SetOutline(
                    Characters_masks[player_left_index],
                    Color.blue,
                    0.2f
                );
            
            }
        }

        //liberar seleccion e ir para atras, solo si eres jugador left.
        if (right_back)
        {
            if (player_right_block != -1)
            {
                SetOutline(
                    Characters_masks[player_right_block],
                    Color.clear,
                    0f
                );
                Characters_masks[player_right_index].transform.GetComponentInChildren<Animator>().SetBool("superselect", false);
            }
            player_right_block = -1;
            

        }
        if (left_back && left_back_delay < 0)
        {
            if (player_left_block != -1)
            {
                 
                 SetOutline(
            Characters_masks[player_left_block],
            Color.clear,
            0f
        );
                Characters_masks[player_left_index].transform.GetComponentInChildren<Animator>().SetBool("superselect", false);

            }
            if (player_left_block == -1)
            {
                //volver escena atrás (menu)
               GameManager.instance.Change_SceneAsync_name("MainTitle_Fin");
                
            }
            player_left_block = -1;
            left_back_delay = delay_movement;
        }


        if (start_button)
        {
            if (player_left_block != -1 && player_right_block != -1)
            {
                GameManager.instance.left_dedo_id = player_right_block;
                GameManager.instance.right_dedo_id = player_left_block;
                //ir a la pelea
                SceneManager.LoadScene("Final");

                GameManager.instance.StartCombatMusic();
            }
        }

        //reseteo de cosas
        dir_Left = new Vector2(0, 0);
        dir_Right = new Vector2(0, 0);
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
        if(!right_select)
        {
            RuntimeManager.PlayOneShot("event:/Hard Select R");
        }

        right_select = true;
    }
    public void OnSelect_Left(CallbackContext context)
    {
        if (!left_select)
        {
            RuntimeManager.PlayOneShot("event:/Hard Select L");
        }

        left_select = true;
    }

    public void OnBack_Left(CallbackContext context)
    {
        left_back = true;
    }

    public void OnBack_Right(CallbackContext context)
    {
        right_back = true;
    }

    public void OnStart_Buttom(CallbackContext context)
    {
        start_button = true;
        //si amabos jugadores tienes elegidos characters, permitir
    }
}
