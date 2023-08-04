using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;


public class ApiManager : MonoBehaviour
{
    //Api
    private string api_Pokemon = " https://pokeapi.co/api/v2/pokemon/"; //Pokemon Api para ID,Nombre Y tipo
    private string api_Pokemon_description = " https://pokeapi.co/api/v2/pokemon-species/"; //ApiParaLaDescripcion
    private string api_Sprites = "https://github.com/PokeAPI/sprites/blob/master/sprites/pokemon/other/official-artwork/"; //+id.png para imagen del pokemon
    
    //Card Info
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI type;
    [SerializeField] private SpriteRenderer pokemon_Art;

    private int numeroPokemon=1;
    
    void Start()
    {
        
    }

    IEnumerator PokemonSearch(int id)
    {
        string url = api_Pokemon + numeroPokemon;
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send(); //Espera a que se acabe la consulta
        
        if(www.result==UnityWebRequest.Result.ConnectionError) Debug.Log("Ese pokemon no existe");
        else
        {
            if (www.responseCode == 200)
            {
                
            }
            
        }
    }
    

 
}
[System.Serializable]
public class Pokemon
{
    public int id;
    public string name;
    public List<string> types;
    public string image;
}

[System.Serializable]
public class Trainer
{
    public int id;
    public string name;
    public int[] deck;
    public string img;
}

[System.Serializable]
public class FlavorText
{
    public string flavor_text;

}