using UnityEngine;

public class PlayerController : MonoBehaviour
{
    InputsControl playerInputs;

    private void Awake()
    {
        playerInputs = GetComponent<InputsControl>();
    }
    private void Update()
    {
        //Debuging Input Value.
        if(playerInputs.MoveInput.magnitude>0f)
        {
            Debug.Log(playerInputs.MoveInput);

        }
        Debug.Log(playerInputs.Jumping);
    }
}
