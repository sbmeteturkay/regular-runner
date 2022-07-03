using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 firstPos, endPos;
    [Range(0f,0.05f)] public float playerSpeed;
    Vector3 parentTransform;
    // Update is called once per frame
    private void Start()
    {
        parentTransform = transform.position;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstPos = Input.mousePosition;
        }
        else if(Input.GetMouseButton(0)){
            endPos = Input.mousePosition;
            float diff = endPos.x - firstPos.x;
            transform.Translate(diff * Time.deltaTime * playerSpeed, 0, 0);
            parentTransform.x = Mathf.Clamp(transform.position.x, -6.25f, 6.25f);
            transform.position = parentTransform;
            if (Input.GetMouseButtonUp(0))
            {
                firstPos = Vector3.zero;
                endPos = Vector3.zero;
            }
        }
    }
}
