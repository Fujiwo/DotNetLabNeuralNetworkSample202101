using NeuralNetworkSample.WPF.Models;
using OxyPlot;
using System.Collections.Generic;
using System.Windows.Media;

namespace NeuralNetworkSample.WPF.ViewModels
{
    public class LineSeriesViewModel
    {
        public List<DataPoint> Coordinates { get; set; } = new List<DataPoint>();

        public string Title { get; set; } = "";

        public Color Color { get; set; } = Colors.Black;

        public MarkerType MarkerType { get; set; } = MarkerType.Plus;
    }

    public class SampleDataViewModel
    {
        public LineSeriesViewModel Data { get; } = new LineSeriesViewModel { Coordinates = new SampleDataModel().Coordinates, Title = "座標データ", Color = Colors.DarkBlue };
    }

    public class NeuralNetworkViewModel
    {
        NeuralNetworkModel neuralNetworkModel = new NeuralNetworkModel();

        public LineSeriesViewModel Fukui { get; private set; }
        public LineSeriesViewModel Others { get; private set; }

        public NeuralNetworkViewModel()
        {
            Fukui = new LineSeriesViewModel { Coordinates = neuralNetworkModel.FukuiCoordinates, Title = "福井", Color = Colors.Red };
            Others = new LineSeriesViewModel { Coordinates = neuralNetworkModel.OthersCoordinates, Title = "他都道府県", Color = Colors.Blue };
        }
    }

    public class TrainingDataViewModel
    {
        TrainingDataModel trainingDataModelModel = new TrainingDataModel();

        public LineSeriesViewModel Fukui { get; private set; }
        public LineSeriesViewModel Others { get; private set; }

        public TrainingDataViewModel()
        {
            Fukui  = new LineSeriesViewModel { Coordinates = trainingDataModelModel.LearningFukuiCoordinates , Title = "福井"      , Color = Colors.Red };
            Others = new LineSeriesViewModel { Coordinates = trainingDataModelModel.LearningOthersCoordinates, Title = "他都道府県", Color = Colors.Blue };
        }
    }

    public abstract class MLViewModel
    {
        internal abstract MLModel Model { get; }

        public LineSeriesViewModel TrainingFukui  { get; private set; }
        public LineSeriesViewModel TrainingOthers { get; private set; }
        public LineSeriesViewModel TestFukui      { get; private set; }
        public LineSeriesViewModel TestOthers     { get; private set; }

        public MLViewModel()
        {
            TrainingFukui  = new LineSeriesViewModel { Coordinates = Model.TrainingFukuiCoordinates , Title = "福井 教師データ"        , Color = Colors.Red     , MarkerType = MarkerType.Plus   };
            TrainingOthers = new LineSeriesViewModel { Coordinates = Model.TrainingOthersCoordinates, Title = "他都道府県 教師データ"  , Color = Colors.Blue    , MarkerType = MarkerType.Plus   };
            TestFukui      = new LineSeriesViewModel { Coordinates = Model.TestFukuiCoordinates     , Title = "福井 テストデータ"      , Color = Colors.DarkRed , MarkerType = MarkerType.Circle };
            TestOthers     = new LineSeriesViewModel { Coordinates = Model.TestOthersCoordinates    , Title = "他都道府県 テストデータ", Color = Colors.DarkBlue, MarkerType = MarkerType.Circle };
        }
    }

    public class MachineLearningViewModel : MLViewModel
    {
        readonly MachineLearningModel model = new MachineLearningModel();

        internal override MLModel Model => model;
    }

    public class MLDotNetViewModel : MLViewModel
    {
        readonly MLDotNetModel model = new MLDotNetModel();

        internal override MLModel Model => model;
    }

    public class AzureMLApiViewModel : MLViewModel
    {
        readonly AzureMLApiModel model = new AzureMLApiModel();

        internal override MLModel Model => model;
    }

    public class MainWindowViewModel
    {
        public SampleDataViewModel      SampleData      { get; } = new SampleDataViewModel     ();
        public NeuralNetworkViewModel   NeuralNetwork   { get; } = new NeuralNetworkViewModel  ();
        public TrainingDataViewModel    TrainingData    { get; } = new TrainingDataViewModel   ();
        public MachineLearningViewModel MachineLearning { get; } = new MachineLearningViewModel();
        public MLDotNetViewModel        MLDotNet        { get; } = new MLDotNetViewModel       ();
        public AzureMLApiViewModel      AzureMLApi      { get; } = new AzureMLApiViewModel     ();
    }
}
