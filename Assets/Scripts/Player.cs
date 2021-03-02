using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Player : MonoBehaviour
{
    CharacterController controller;
    public Camera camera;

    public int elementState = 0;
    public Vector3 velocity = new Vector3(); 
    
    public Vector3 facing;

    float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        controller = this.GetComponent<CharacterController>();
        facing = transform.forward;
    }

    // Update is called once per frame
    
    void Update()
    { 
        MoveCharacter();
    }

    void MoveCharacter()
    {
        Vector3 movedirection = new Vector3();
        if (elementState == 0)
            movedirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        Vector3 realmovedirection = new Vector3();
        if (movedirection.magnitude > 0)
        {
            facing = Quaternion.LookRotation(new Vector3(camera.transform.forward.x, 0, camera.transform.forward.z)) * movedirection;
            realmovedirection = facing;
        }
        controller.Move((realmovedirection * speed + velocity) * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(facing), 900f * Time.deltaTime);

    }
}