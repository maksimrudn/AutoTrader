using AutoTrader.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Infrastructure.Stock.Dummy
{
    public static class DummyStreamGenerator
    {
        static string _dummy_data_folder = @"Stock\Dummy\Data\";

        public static void Generate(string filename, Action<string> handleData)
        {
            string filepath = @$"{MainHelper.GetWorkFolder()}{_dummy_data_folder}{filename}";

            using (StreamReader reader = new StreamReader(filepath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    handleData(line);
                }
            }
        }
    }
}
