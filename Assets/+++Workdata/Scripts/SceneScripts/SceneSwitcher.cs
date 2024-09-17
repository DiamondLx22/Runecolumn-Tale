using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


public class SceneSwitcher : MonoBehaviour
{
    [FormerlySerializedAs("animDoor")] public Animator animSceneblend;
    public int sceneBuildIndex;

    private void Start()
    {
        animSceneblend = GameObject.Find("Blackscreen").GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            StartCoroutine(InitiateTeleport(other));
        }
    }

    IEnumerator InitiateTeleport(Collider2D other)
    {
        animSceneblend.Play("BlackscreenFadeIn");
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        animSceneblend = GameObject.Find("Blackscreen").GetComponent<Animator>();

        yield return new WaitForSeconds(.3f);
        animSceneblend.Play("BlackscreenFadeOut");
    }
    
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
}

