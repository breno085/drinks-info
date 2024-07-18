using System.Reflection;
using System.Web;
using drinks_info.Models;
using Newtonsoft.Json;
using RestSharp;

namespace drinks_info
{
    public class DrinksService
    {
        //2 passo
        //create the class that is responsible for interacting with the drinks api
        //(the user needs see and choose a drink from a list of categories of drinks available)

        public List<Category> GetCategories()
        {
            // 5 passo - call the api
            //For that you need to create an instance of the RestClient class
            //and pass api url as an argument

            //Criação da instancia da classe RestClient - esse classe é a responsável por gerenciar a comunicação com a API
            var client = new RestClient("http://www.thecocktaildb.com/api/json/v1/1/");
            
            //Criação da instancia da classe RestRequest - especifica o endpoint da API que quero chamar
            //nesse caso é "list.php?c=list" que recupera uma lista de categorias
            var request = new RestRequest("list.php?c=list");
            
            //Esta linha executa a solicitação de forma assíncrona.
            //O método ExecuteAsync envia a solicitação à API e aguarda a resposta. A resposta contém os dados retornados pela API.
            //RestResponse ('response') contém o código de status, conteúdo e outros metadados sobre a resposta.
            var response = client.ExecuteAsync(request);

            List<Category> categories = new();

            if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Recupera o conteúdo bruto da resposta JSON
                string rawResponse = response.Result.Content;
                var serialize = JsonConvert.DeserializeObject<Categories>(rawResponse);

                categories = serialize.CategoriesList;

                TableVisualizationEngine.ShowTable(categories, "Categories Menu");
                return categories;
            }

            return categories;
        }

        internal List<Drink> GetDrinksByCategory(string category)
        {
            var client = new RestClient("http://www.thecocktaildb.com/api/json/v1/1/");
            var request = new RestRequest($"filter.php?c={HttpUtility.UrlEncode(category)}");

            var response = client.ExecuteAsync(request);

            List<Drink> drinks = new();

            if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string rawResponse = response.Result.Content;

                var serialize = JsonConvert.DeserializeObject<Drinks>(rawResponse);

                drinks = serialize.DrinksList;

                TableVisualizationEngine.ShowTable(drinks, "Drinks Menu");

                return drinks;

            }

            return drinks;

        }

        internal void GetDrink(string drink)
        {
            var client = new RestClient("http://www.thecocktaildb.com/api/json/v1/1/");
            var request = new RestRequest($"lookup.php?i={drink}");
            var response = client.ExecuteAsync(request);

            if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string rawResponse = response.Result.Content;

                var serialize = JsonConvert.DeserializeObject<DrinkDetailObject>(rawResponse);

                List<DrinkDetail> returnedList = serialize.DrinkDetailList;

                DrinkDetail drinkDetail = returnedList[0];

                List<object> prepList = new();

                string formattedName = "";

                foreach (PropertyInfo prop in drinkDetail.GetType().GetProperties())
                {

                    if (prop.Name.Contains("str"))
                    {
                        formattedName = prop.Name.Substring(3);
                    }

                    if (!string.IsNullOrEmpty(prop.GetValue(drinkDetail)?.ToString()))
                    {
                        prepList.Add(new
                        {
                            Key = formattedName,
                            Value = prop.GetValue(drinkDetail)
                        });
                    }
                }

                TableVisualizationEngine.ShowTable(prepList, drinkDetail.strDrink);
            }
        }
    }
}