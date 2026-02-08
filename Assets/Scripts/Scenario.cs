using UnityEngine;

public class Scenario : MonoBehaviour
{

    [SerializeField]
    Material brazoBase;
    [SerializeField]
    Material brazoOscuro;
    [SerializeField]
    Material brazoRosa;
    [SerializeField]
    Material brazoSkeletor;
    [SerializeField]
    Material brazoPalido;

    [SerializeField]
    GameObject brazoIzquierda;
    [SerializeField]
    GameObject brazoDerecha;
    [SerializeField]
    GameObject hueso;

    void Start()
    {
        // Gracias gracias, de mis mejores codigos, lo se
        int IzID = GameManager.instance.right_dedo_id; // de enfermo mental lo que acabo de poner aqui
        int DeID = GameManager.instance.left_dedo_id;
        // 4 dedillo
        // 5 soldedo
        // 0 luchadedo
        // 1 divadedo
        // 2 dedofurro
        // 3 skeletor

        switch (IzID) 
        { 
            case 0:
                brazoIzquierda.GetComponent<Renderer>().material = brazoBase;
                break;
            case 1:
                brazoIzquierda.GetComponent<Renderer>().material = brazoBase;
                break;
            case 2:
                brazoIzquierda.GetComponent<Renderer>().material = brazoOscuro;
                break;
            case 3:
                brazoIzquierda.GetComponent<Renderer>().material = brazoSkeletor;
                hueso.SetActive(true);
                break;
            case 4:
                brazoIzquierda.GetComponent<Renderer>().material = brazoBase;
                break;
            case 5:
                brazoIzquierda.GetComponent<Renderer>().material = brazoPalido;
                break;
        }
        switch (DeID)
        {
            case 0:
                brazoDerecha.GetComponent<Renderer>().material = brazoBase;
                break;
            case 1:
                brazoDerecha.GetComponent<Renderer>().material = brazoBase;
                break;
            case 2:
                brazoDerecha.GetComponent<Renderer>().material = brazoOscuro;
                break;
            case 3:
                brazoDerecha.GetComponent<Renderer>().material = brazoOscuro;
                brazoDerecha.GetComponent<Renderer>().material = brazoSkeletor;
                hueso.SetActive(true);
                break;
            case 4:
                brazoDerecha.GetComponent<Renderer>().material = brazoBase;

                break;
            case 5:
                brazoDerecha.GetComponent<Renderer>().material = brazoPalido;

                break;
        }
    }

    void Update()
    {
        
    }
}
