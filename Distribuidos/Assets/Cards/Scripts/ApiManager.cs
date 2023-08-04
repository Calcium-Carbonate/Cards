using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;


public class ApiManager : MonoBehaviour
{
    //Api
    private string api_Pokemon = "https://pokeapi.co/api/v2/pokemon/"; //Pokemon Api para ID,Nombre Y tipo
    private string api_Pokemon_description = "https://pokeapi.co/api/v2/pokemon-species/"; //ApiParaLaDescripcion
    private string api_Sprites = "https://github.com/PokeAPI/sprites/blob/master/sprites/pokemon/other/official-artwork/"; //+id.png para imagen del pokemon
    
    //Card Info
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private TextMeshProUGUI descriptionTMP;
    [SerializeField] private TextMeshProUGUI typeTMP; 
    [SerializeField] private SpriteRenderer pokemon_Art;

    [SerializeField] private int numeroPokemon=1;
    
    void Start()
    {
        StartCoroutine(PokemonSearch(numeroPokemon));
    }
    
    IEnumerator PokemonSearch(int id)
    {   //PokemonBasics
        string urlPokemon = api_Pokemon + id;
        UnityWebRequest wwwPokemon = UnityWebRequest.Get(urlPokemon);

        
        
        yield return wwwPokemon.Send(); //Espera a que se acabe la consulta
        
        if(wwwPokemon.result==UnityWebRequest.Result.ConnectionError) Debug.Log("Ese pokemon no existe");
        else
        {
            if (wwwPokemon.responseCode == 200)
            {   
                Pokemon pokemon = JsonUtility.FromJson<Pokemon>(wwwPokemon.downloadHandler.text);
                
                //PokemonDescription
                string urlPokemonDescription = api_Pokemon_description + numeroPokemon;
                UnityWebRequest wwwDescription = UnityWebRequest.Get(urlPokemonDescription);
                //En la Api se le conoce como Flavor Text a la descripcion del pokemon :D
                FlavorText description;
                if (wwwDescription.result == UnityWebRequest.Result.ConnectionError) Debug.Log("Ese pokemon no existe");

                else
                {
                    description = JsonUtility.FromJson<FlavorText>(wwwDescription.downloadHandler.text);
                    BuildCard(id,pokemon.name,description.flavor_text,pokemon.types);
                }
                
               
            }
            
        }
    }

    IEnumerator DownloadImage(string url,int id, SpriteRenderer pokemonPhoto)
    {
        
        UnityWebRequest request=UnityWebRequestTexture.GetTexture(url+id+".png"); //descargamos la imagen del pokemon
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError) Debug.Log(request.error);
        else
        {
            Texture2D art = ((DownloadHandlerTexture)request.downloadHandler).texture; //Creamos la textura
            Rect rect = new Rect(0, 0, art.width, art.height);
            pokemonPhoto.sprite = Sprite.Create(art,rect,Vector2.zero,0.01f); //Convertimos la textura en un sprite
        }
       
    }

    public void BuildCard(int id,string name,string description,List<string> types)
    {
        StartCoroutine(DownloadImage(api_Sprites, id, pokemon_Art));

        nameTMP.text = name;
        descriptionTMP.text = description;
        typeTMP.text = types[0];
        

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
