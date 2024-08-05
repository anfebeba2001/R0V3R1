
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTravelerController : MonoBehaviour
{
  public void World()
    {
        SceneManager.LoadScene("FirstScene");
    }
    public void Purgatory()
    {
        SceneManager.LoadScene("Purgatory");
    }
    public void mainScreenPlayButton()
    {
        
        string dataPath = Application.persistentDataPath + "/player.save";
        File.Delete(dataPath);
        if((File.Exists(dataPath)))
        { 
            SceneManager.LoadScene("FirstScene");
        }
        else
        {
            SceneManager.LoadScene("TutoScene");
        }
            
    }
}






