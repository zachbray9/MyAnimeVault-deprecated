﻿using MyAnimeVault.Domain.Models;
using MyAnimeVault.Domain.Services;
using MyAnimeVault.MyAnimeListApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MyAnimeVault.MyAnimeListApi.Services
{
    public class AnimeApiService : IAnimeApiService
    {
        private readonly IHttpClientFactory HttpClientFactory;

        public AnimeApiService(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }

        public async Task<List<AnimeListNode>> GetAllAnime()
        {
            HttpClient client = HttpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("X-MAL-CLIENT-ID", "ce20b660a7716a612c5523c38e3d7209");

            List<AnimeListNode> AnimeList = new List<AnimeListNode>();

            HttpResponseMessage response = await client.GetAsync("https://api.myanimelist.net/v2/anime?q=one&limit=4&offset=0&fields=id,title,main_picture,start_date,end_date,synopsis,mean,rank,popularity,nsfw,genres,created_at,updated_at,media_type,status,num_episodes,source,broadcast,average_episode_duration,rating,studios");
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                AnimeListEndpointResponse endpointResponse = JsonConvert.DeserializeObject<AnimeListEndpointResponse>(jsonString);
                AnimeList = endpointResponse.Data;
            }
            
            return AnimeList;
        }
    }
}