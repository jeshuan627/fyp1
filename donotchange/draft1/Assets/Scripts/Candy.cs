using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Candy : MonoBehaviour
{
    //candy found in https://www.assetstore.unity3d.com/en/#!/content/12512

    [SerializeField] private AudioSource CandySoundEffect;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * rotateSpeed);
    }

    IEnumerator OnTriggerEnter(Collider col)
    {
        UIManager.Instance.IncreaseScore(ScorePoints);
        CandySoundEffect.Play();
        Debug.Log("Played");
        while (CandySoundEffect.isPlaying)
            yield return null;
        Destroy(this.gameObject);
    }

    /*IEnumerator playAudio()
    {
        CandySoundEffect.Play();
        Debug.Log("Played");
        while (CandySoundEffect.isPlaying)
            yield return null;
        Destroy(this.gameObject);
    }*/

    public int ScorePoints = 100;
    public float rotateSpeed = 50f;
}
