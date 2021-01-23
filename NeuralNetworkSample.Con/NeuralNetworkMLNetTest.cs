using NeuralNetworkSample2021ML.Model;
using System;

namespace NeuralNetworkSample.Con
{
    class NeuralNetworkMLNetTest
    {
        public static void Run()
        {
            var sampleData = new ModelInput { Latitude = 36.1119155f, Longitude = 136.2741973f };
            var predictResult = ConsumeModel.Predict(sampleData);
            Console.WriteLine($"Prediction: {predictResult.Prediction}, Score[{String.Join(", ", predictResult.Score)}]");
        }
    }
}
