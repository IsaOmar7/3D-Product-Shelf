using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Adds a continuous rotation animation to an object, simulating a spinning effect.
/// </summary>
public class AstroAnim : MonoBehaviour
{

    /// <summary>
    /// Updates the object's rotation to create a spinning effect around the forward axis.
    /// </summary>
    void Update()
    {
        transform.Rotate(Vector3.forward * 0.5f * Time.deltaTime);
    }
}
