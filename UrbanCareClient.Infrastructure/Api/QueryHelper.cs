namespace UrbanCareClient.Infrastructure.Api
{
    public static class QueryHelper
    {
        public static string AddParamsFromDictionary(Dictionary<string, string> parametrs)
        {
            string paramsString = "?";

            foreach (var item in parametrs)
                paramsString += $"{item.Key}={item.Value}&";

            paramsString = paramsString.TrimEnd('&');

            return paramsString;
        }
    }
}
