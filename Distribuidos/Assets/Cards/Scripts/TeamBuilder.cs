using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class TeamBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] CardManager[] pokemonTeam;
    
    string trainerApi= "https://my-json-server.typicode.com/Calcium-Carbonate/Cards/users/";
    [SerializeField] private SpriteRenderer trainerPhoto;
    [SerializeField] TextMeshProUGUI trainerNameTMP;
    
    // Update is called once per frame
    IEnumerator GetTrainerData(string trainerId)
    {
        string url = trainerApi + trainerId;
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();

        if(www.result==UnityWebRequest.Result.ConnectionError) Debug.Log("Ese maestro no existe");
        else
        {
            if (www.responseCode == 200)
            {
                Trainer trainer = JsonUtility.FromJson<Trainer>(www.downloadHandler.text);
                
                StartCoroutine(DownloadImage(trainer.img, trainerPhoto));
                trainerNameTMP.text = trainer.name;
                
                for (int i = 0; i < trainer.deck.Length; i++)
                {
                    pokemonTeam[i].SearchPokemon(trainer.deck[i]);
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else
            {
                string error_message = "Status: " + www.responseCode;
                error_message += "\nContent-Type: " + www.GetResponseHeader("content-type") + "\nError:" + www.error;
                Debug.Log(error_message);
            }
        }
    }

    IEnumerator DownloadImage(string url, SpriteRenderer pokemonPhoto)
    {
        UnityWebRequest request=UnityWebRequestTexture.GetTexture(url+"?raw=true"); //descargamos la imagen del entrenador
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError) Debug.Log(request.error);
        else
        {
            Texture2D art = ((DownloadHandlerTexture)request.downloadHandler).texture; //Creamos la textura
            Rect rect = new Rect(0, 0, art.width, art.height);
            pokemonPhoto.sprite = Sprite.Create(art,rect,new Vector2(0.5f,0.5f)); //Convertimos la textura en un sprite
        }
    }
    
    public void ShowTrainerData(string trainerId)
    {
        
        StartCoroutine(GetTrainerData(trainerId));
    }
}
[System.Serializable]
public class Trainer
{
    public int id;
    public string name;
    public int[] deck;
    public string img;
}

