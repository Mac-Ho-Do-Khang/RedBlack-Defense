using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuNavigate : MonoBehaviour
{
    [SerializeField] string main_page = "Main1";
    [SerializeField] string introduction_page = "Introduction";
    [SerializeField] string simulation_page = "Simulation";
    [SerializeField] string gameplay_page = "Gameplay";
    [SerializeField] string game_page = "Game";
    public void introduction()
    {
        SceneManager.LoadScene(introduction_page);
    }
    public void simulation()
    {
        SceneManager.LoadScene(simulation_page);
    }
    public void gameplay()
    {
        SceneManager.LoadScene(gameplay_page);
    }
    public void game()
    {
        SceneManager.LoadScene(game_page);
    }
    public void return_main()
    {
        SceneManager.LoadScene(main_page);
    }
}
