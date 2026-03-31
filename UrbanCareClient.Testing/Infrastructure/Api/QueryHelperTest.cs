using UrbanCareClient.Infrastructure.Api;

namespace UrbanCareClient.Testing.Infrastructure.Api
{
    [TestClass]
    public class QueryHelperTest
    {
        [TestMethod]
        public void CheckAddingFromDict()
        {
            var dict = new Dictionary<string, string>() {
                {"id", "3" },
                {"role", "6" },
            };

            var res = QueryHelper.AddParamsFromDictionary(dict);

            Console.WriteLine(res);
            Xunit.Assert.EndsWith("6", res);
        }
    }
}
