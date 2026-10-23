using System;

namespace IndependentWork1
{
    public class Playlist
    {
        private string _title;
        private int _trackCount;
        private double _totalDurationMinutes;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public int TrackCount
        {
            get { return _trackCount; }
        }

        public Playlist(string title, int trackCount, double totalDurationMinutes)
        {
            _title = title;
            _trackCount = trackCount;
            _totalDurationMinutes = totalDurationMinutes;
        }

        public double GetAverageTrackLength()
        {
            if (_trackCount == 0) return 0;
            return _totalDurationMinutes / _trackCount;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Плейліст: '{_title}' | Треків: {_trackCount} | Загальний час: {_totalDurationMinutes} хв.");
        }
    }

    public class Recipe
    {
        private string _name;
        private int _cookingTimeMinutes;
        private int _servings;

        public string Name
        {
            get { return _name; }
        }

        public int CookingTimeMinutes
        {
            get { return _cookingTimeMinutes; }
            private set { _cookingTimeMinutes = value; }
        }

        public Recipe(string name, int cookingTimeMinutes, int servings)
        {
            _name = name;
            _cookingTimeMinutes = cookingTimeMinutes;
            _servings = servings;
        }

        public bool IsLongToCook()
        {
            return _cookingTimeMinutes > 45;
        }

        public double GetTimePerServing()
        {
            if (_servings == 0) return 0;
            return (double)_cookingTimeMinutes / _servings;
        }
    }


    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("САМОСТІЙНА РОБОТА 1");
            

            Playlist myPlaylist = new Playlist("UFC Workout / Rock", 14, 56.0);

            myPlaylist.PrintInfo();
            double avgLength = myPlaylist.GetAverageTrackLength();
            Console.WriteLine($"Середня тривалість треку: {avgLength:F2} хв.\n");


            Recipe myRecipe = new Recipe("Борщ", 90, 6);

            Console.WriteLine($"Рецепт: {myRecipe.Name}");
            Console.WriteLine($"Загальний час приготування: {myRecipe.CookingTimeMinutes} хв.");

            bool isLong = myRecipe.IsLongToCook();
            double timePerServing = myRecipe.GetTimePerServing();

            Console.WriteLine($"Чи тривале приготування (>45 хв): {(isLong ? "Так" : "Ні")}");
            Console.WriteLine($"Час на 1 порцію: {timePerServing:F1} хв.");

        }
    }
}