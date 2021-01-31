using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 10.0f;

    private CharacterController character;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 x = transform.forward * Input.GetAxisRaw("Vertical") * maxSpeed * Time.deltaTime;
        Vector3 z = transform.right * Input.GetAxisRaw("Horizontal") * maxSpeed * Time.deltaTime;
        Vector3 move = x + z;

        character.Move(move);
    }
}
