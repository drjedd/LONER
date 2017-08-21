using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* ZOMBIE MANAGER (Obsolete)
 * 
 * Simple code that keeps track of enemies in the Scene. You win if they're all dead.
 * 
 * */

public class ZombieManager : MonoBehaviour {

    public Text zombiesLeftUI;
    public Text statusTextUI;
	
	void Update () {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        int amountOfEnemiesLeft = enemies.Length;

        zombiesLeftUI.text = "Zombies left: " + amountOfEnemiesLeft;

        if (amountOfEnemiesLeft == 0)
        {
            statusTextUI.text = "YOU WIN\nPress [R] to play again";
        }

	}
}
