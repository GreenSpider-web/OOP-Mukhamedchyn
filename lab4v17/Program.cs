using System;

namespace Lab4
{
    public class Plant
    {
        private string _name;
        private double _height;

        public string Name => _name;
        public double Height => _height;

        public Plant(string name, double height)
        {
            _name = name;
            _height = height;
        }

        public virtual void Grow()
        {
            Console.WriteLine($"Рослина '{_name}' росте. Поточна висота: {_height:F1} м.");
        }

        public string GetPlantType()
        {
            return "Звичайна рослина";
        }
    }

    public class Tree : Plant
    {
        private double _trunkDiameter;

        public double TrunkDiameter => _trunkDiameter;

        public Tree(string name, double height, double trunkDiameter)
            : base(name, height)
        {
            _trunkDiameter = trunkDiameter;
        }

        public override void Grow()
        {
            Console.WriteLine($"Дерево '{Name}' росте вгору. Висота: {Height:F1} м, діаметр стовбура: {_trunkDiameter:F2} м.");
        }

        public void ShedLeaves()
        {
            Console.WriteLine($"Дерево '{Name}' скидає листя восени.");
        }

        public new string GetPlantType()
        {
            return "Дерево (метод приховано через new)";
        }
    }

    public class Flower : Plant
    {
        private string _petalColor;

        public string PetalColor => _petalColor;

        public Flower(string name, double height, string petalColor)
            : base(name, height)
        {
            _petalColor = petalColor;
        }

        public override void Grow()
        {
            Console.WriteLine($"Квітка '{Name}' росте та формує бутони. Висота: {Height:F1} м.");
        }

        public void Bloom()
        {
            Console.WriteLine($"Квітка '{Name}' розквітає. Колір пелюсток: {_petalColor}.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Демонстрація ієрархії Plant -> Tree / Flower\n");

            var plant = new Plant("Мох", 0.1);
            var tree = new Tree("Дуб", 12.5, 0.8);
            var flower = new Flower("Троянда", 0.6, "червоний");

            Console.WriteLine("Виклик virtual/override через посилання базового класу:");
            Plant[] plants = { plant, tree, flower };
            foreach (Plant currentPlant in plants)
            {
                currentPlant.Grow();
            }

            Console.WriteLine("\nВласні методи похідних класів:");
            tree.ShedLeaves();
            flower.Bloom();

            Console.WriteLine("\nРізниця між override та new:");
            Plant treeAsPlant = tree;
            Console.WriteLine($"Через Plant: {treeAsPlant.GetPlantType()}");
            Console.WriteLine($"Через Tree: {tree.GetPlantType()}");
        }
    }
}
