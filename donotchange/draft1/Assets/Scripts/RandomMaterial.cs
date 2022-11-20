using UnityEngine;
using System.Collections;

public class RandomMaterial : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        GetComponent<Renderer>().material = GetMaterial();
    }
	
    public Material GetMaterial()
    {
        return Resources.Load("Materials/Rock3") as Material;
    }

}
