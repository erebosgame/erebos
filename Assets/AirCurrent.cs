using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirCurrent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other) {
        if(other.CompareTag("Player"))
        {
            if (Player.gameObject.GetComponent<PlayerMovement>().isGliding)
            {
                float scaledRadius = this.GetComponent<CapsuleCollider>().radius * this.transform.localScale.x;
                float percentagePosition = (Player.gameObject.transform.position.y - this.GetComponent<CapsuleCollider>().bounds.min.y - scaledRadius) / ((this.GetComponent<CapsuleCollider>().bounds.extents.y - scaledRadius) * 2); 
                float steepness = 2F;
                float maxSpeed = 12F;
                float brakeStart = 0.9F;

                if(percentagePosition < brakeStart)
                {
                    Player.gameObject.GetComponent<PlayerMovement>().jumpVector.y = maxSpeed;
                }
                else if(percentagePosition < 1)
                {
                    Player.gameObject.GetComponent<PlayerMovement>().jumpVector.y = Sigmoid(steepness, maxSpeed, (1 - percentagePosition) / (1 - brakeStart));
                } 
                else
                {
                    Player.gameObject.GetComponent<PlayerMovement>().jumpVector.y = 0;
                }

                Debug.Log(Player.gameObject.GetComponent<PlayerMovement>().jumpVector.y + " - " + percentagePosition);
            }
        }


    }

    private float Sigmoid(float steepness, float maxSpeed, float x)
    {
        //Debug.Log(maxSpeed / (1 + Mathf.Exp(-steepness * (x - 0.5F))));
        return maxSpeed / (1 + Mathf.Pow(x / (1-x), -steepness)); 
    }
}
