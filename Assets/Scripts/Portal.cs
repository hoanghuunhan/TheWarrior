
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string sceneName = "Dungeon";
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.isTeleport = true;
            GameManager.instance.SaveState();
            SceneManager.LoadScene("Dungeon");
        }
    }
}
