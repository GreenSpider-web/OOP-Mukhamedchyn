using System;

namespace OOP_Mukhamedchyn
{
    public class SportTeam
    {
        private string name;
        private string coach;

        public int PlayersCount { get; set; }

        public SportTeam(string name, string coach, int playersCount)
        {
            this.name = name;
            this.coach = coach;
            this.PlayersCount = playersCount;
        }

        public void PlayMatch()
        {
            Console.WriteLine($"Команда «{name}» під керівництвом тренера {coach} (гравців у складі: {PlayersCount}) виходить на поле та грає матч!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            SportTeam team1 = new SportTeam("Динамо Київ", "Олександр Шовковський", 25);
            SportTeam team2 = new SportTeam("Шахтар Донецьк", "Маріно Пушич", 24);
            SportTeam team3 = new SportTeam("Реал Мадрид", "Карло Анчелотті", 23);

            team1.PlayMatch();
            team2.PlayMatch();
            team3.PlayMatch();
        }
    }
}