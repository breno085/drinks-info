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
            var client = new RestClient("http://www.thecocktaildb.com/api/json/v1/1/");
            var request = new RestRequest("list.php?c=list");
            var response = client.ExecuteAsync(request);

            List<Category> categories = new();

            if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string rawResponse = response.Result.Content;
                var serialize = JsonConvert.DeserializeObject<Categories>(rawResponse);

                List<Category> returnedList = serialize.CategoriesList;

                TableVisualizationEngine.ShowTable(returnedList, "Categories Menu");
                return categories;
            }

            return categories;
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

                if (serialize?.DrinkDetailList != null && serialize.DrinkDetailList.Count > 0)
                {
                    List<DrinkDetail> returnedList = serialize.DrinkDetailList;

                    DrinkDetail drinkDetail = returnedList[0];

                    List<object> prepList = new();

                    foreach (PropertyInfo prop in drinkDetail.GetType().GetProperties())
                    {
                        if (prop.Name.Contains("str"))
                        {
                            string formattedName = prop.Name.Substring(3);

                            var value = prop.GetValue(drinkDetail);
                            if (!string.IsNullOrEmpty(value?.ToString()))
                            {
                                prepList.Add(new
                                {
                                    Key = formattedName,
                                    Value = value
                                });
                            }
                        }
                    }

                    TableVisualizationEngine.ShowTable(prepList, drinkDetail.strDrink);
                }
                else
                {
                    // Handle the case where the list is null or empty
                    Console.WriteLine("No drink details found.");
                }
            }
            else
            {
                // Handle non-OK status code
                Console.WriteLine("Failed to retrieve data. Status code: " + response.Result.StatusCode);
            }
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
    }
}