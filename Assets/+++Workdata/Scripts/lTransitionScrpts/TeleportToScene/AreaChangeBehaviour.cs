using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainAreaChangeBehaviour : MonoBehaviour
{
    public Animator animDoor;
    public int sceneBuildIndex;

    private void Start()
    {
        animDoor = GameObject.Find("Blackscreen").GetComponent<Animator>();
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
        animDoor.Play("BlackscreenFadeIn");
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        animDoor = GameObject.Find("Blackscreen").GetComponent<Animator>();

        yield return new WaitForSeconds(.3f);
        animDoor.Play("BlackscreenFadeOut");
    }
    
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
}
