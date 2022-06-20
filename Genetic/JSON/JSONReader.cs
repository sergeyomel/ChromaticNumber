using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Genetic.JSON
{
    public static class JSONReader
    {
        // private static string path = Path.Combine(Environment.CurrentDirectory+"\\JSONINFO.txt");
        public static string path = @"C:\Users\myacc\Desktop\JSONEvristicAlgorithm.txt";

        public static List<ModelJSON> Read()
        {
            if (!File.Exists(path))
                throw new Exception("File not found.");

            var lModel = new List<ModelJSON>();

            using(var stream = new StreamReader(path))
            {
                while (!stream.EndOfStream)
                {
                    try
                    {
                        ModelJSON model = Newtonsoft.Json.JsonConvert.DeserializeObject<ModelJSON>(stream.ReadLine());
                        if (JSONValidator.IsCorrectModel(model))
                            lModel.Add(model);
                    }
                    catch (Exception e) 
                    {
                        Logger.PushMessage(e);
                    }
                }
            }

            return lModel;
        }
    }
}
