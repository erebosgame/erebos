using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

    public float sens = 3f;
    public float rotationLerp = 0.5f;

    public Quaternion nextRotation;     

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        //Rotate the Follow Target transform based on the input
        this.transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X") * sens, Vector3.up);

        this.transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * 1.5f * -1, Vector3.right);

        var angles = this.transform.localEulerAngles;
        angles.z = 0;

        var angle = this.transform.localEulerAngles.x;

        //Clamp the Up/Down rotation
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if(angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        this.transform.localEulerAngles = angles;
        
        nextRotation = Quaternion.Lerp(this.transform.rotation, nextRotation, Time.deltaTime * rotationLerp);

        if (!Player.stats.weapon)
        {
            Player.gameObject.transform.rotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0);

            this.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
        }        

    }

}
