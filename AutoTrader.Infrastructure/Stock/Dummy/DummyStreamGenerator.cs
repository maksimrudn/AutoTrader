using AutoTrader.Application.Helpers;

namespace AutoTrader.Infrastructure.Stock.Dummy
{
    // Todo: remove after refactoring of StockClient
    [Obsolete]
    public static class DummyStreamGenerator
    {
        static string _dummy_data_folder = @"Stock\Dummy\Data\";

        public static async Task Generate(string filename, Action<string> handleData)
        {
            string filepath = @$"{MainHelper.GetWorkFolder()}{_dummy_data_folder}{filename}";

            using (StreamReader reader = new StreamReader(filepath))
            {
                string? line;
                while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
                {
                    handleData(line);
                }
            }
        }
    }
}
