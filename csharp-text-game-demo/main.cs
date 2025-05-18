using System;

namespace TextAdventureGame
{
    class Program
    {
        static Player player = new Player();
        static Random random = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("=== DUNGEON EXPLORER ===");
            Console.WriteLine("What's your name, adventurer?");
            player.Name = Console.ReadLine();

            Console.WriteLine($"\nWelcome, {player.Name}! Let's begin...\n");
            GameLoop();
        }

        static void GameLoop()
        {
            while (player.IsAlive)
            {
                Console.WriteLine("\n----------------------------------");
                Console.WriteLine($"HP: {player.Health} | Gold: {player.Gold}");
                Console.WriteLine("1. Explore | 2. Shop | 3. Quit");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Explore();
                        break;
                    case "2":
                        Shop();
                        break;
                    case "3":
                        Console.WriteLine("Thanks for playing!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            }
            Console.WriteLine("GAME OVER!");
        }

        static void Explore()
        {
            int encounter = random.Next(1, 4); // Random event (1-3)

            switch (encounter)
            {
                case 1:
                    Console.WriteLine("You found a healing potion! (+10 HP)");
                    player.Health = Math.Min(player.Health + 10, 100);
                    break;
                case 2:
                    Console.WriteLine("You found 20 gold!");
                    player.Gold += 20;
                    break;
                case 3:
                    Combat();
                    break;
            }
        }

        static void Combat()
        {
            Enemy enemy = new Enemy();
            Console.WriteLine($"A wild {enemy.Name} (HP: {enemy.Health}) appears!");

            while (enemy.IsAlive && player.IsAlive)
            {
                Console.WriteLine("\n1. Attack | 2. Flee");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    // Player attacks
                    enemy.Health -= player.AttackPower;
                    Console.WriteLine($"You hit the {enemy.Name} for {player.AttackPower} damage!");

                    // Enemy attacks back (if alive)
                    if (enemy.IsAlive)
                    {
                        player.Health -= enemy.AttackPower;
                        Console.WriteLine($"{enemy.Name} hits you for {enemy.AttackPower} damage!");
                    }
                }
                else if (choice == "2")
                {
                    if (random.Next(0, 2) == 0) // 50% chance to flee
                    {
                        Console.WriteLine("You escaped safely!");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("You failed to flee!");
                        player.Health -= enemy.AttackPower;
                    }
                }

                Console.WriteLine($"Player HP: {player.Health} | {enemy.Name} HP: {enemy.Health}");
            }

            if (!enemy.IsAlive)
            {
                int reward = random.Next(10, 30);
                Console.WriteLine($"You defeated the {enemy.Name} and earned {reward} gold!");
                player.Gold += reward;
            }
        }

        static void Shop()
        {
            Console.WriteLine("\n=== SHOP ===");
            Console.WriteLine("1. Health Potion (+20 HP) - 15 gold");
            Console.WriteLine("2. Sword Upgrade (+5 ATK) - 30 gold");
            Console.WriteLine("3. Exit");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    if (player.Gold >= 15)
                    {
                        player.Health = Math.Min(player.Health + 20, 100);
                        player.Gold -= 15;
                        Console.WriteLine("You drank the potion!");
                    }
                    else
                        Console.WriteLine("Not enough gold!");
                    break;
                case "2":
                    if (player.Gold >= 30)
                    {
                        player.AttackPower += 5;
                        player.Gold -= 30;
                        Console.WriteLine("Your sword glows brighter!");
                    }
                    else
                        Console.WriteLine("Not enough gold!");
                    break;
            }
        }
    }

    class Player
    {
        public string Name { get; set; }
        public int Health { get; set; } = 100;
        public int AttackPower { get; set; } = 10;
        public int Gold { get; set; } = 20;
        public bool IsAlive => Health > 0;
    }

    class Enemy
    {
        public string Name { get; }
        public int Health { get; set; }
        public int AttackPower { get; }

        public Enemy()
        {
            string[] names = { "Goblin", "Skeleton", "Orc" };
            Name = names[new Random().Next(0, names.Length)];
            Health = new Random().Next(20, 40);
            AttackPower = new Random().Next(5, 15);
        }

        public bool IsAlive => Health > 0;
    }
}