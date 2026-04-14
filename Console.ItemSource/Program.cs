//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2026
// </copyright>
// <Template>
// 	Version 3.0.2026.1, 08.1.2026
// </Template>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>03.03.2026 14:26:39</date>
//
// <summary>
// Konsolen Applikation mit Menü
// </summary>
//-----------------------------------------------------------------------

namespace Console.ItemSource
{
    /* Imports from NET Framework */
    using System;
    using System.Collections;
    using System.ComponentModel;

    public class Program
    {
        public Program()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
        }
        private static void Main(string[] args)
        {
            CMenu mainMenu = new CMenu("Test ItemSource Property");
            mainMenu.AddItem("Auswahl Menüpunkt 1", MenuPoint1);
            mainMenu.AddItem("Beenden", () => ApplicationExit());
            mainMenu.Show();
        }

        public static IEnumerable ItemsSource { get; set; }

        private static void ApplicationExit()
        {
            Environment.Exit(0);
        }

        private static void MenuPoint1()
        {
            Console.Clear();

            ItemsSource = LoadUsersFromDatabase();

            if (ItemsSource == null)
            {
                return;
            }

            var items = ItemsSource.Cast<object>().ToList();
            if (items.Any() == false)
            {
                return;
            }

            // 1. Typ bestimmen
            var itemType = items.First().GetType();

            // 2. Properties holen (WPF-Mechanismus!)
            var properties = TypeDescriptor.GetProperties(itemType)
                                           .Cast<PropertyDescriptor>()
                                           .ToList();

            foreach (PropertyDescriptor p in properties)
            {
                Console.WriteLine(p.Name);
            }

            Console.Line();

            foreach (var item in items)
            {
                for (int col = 0; col < properties.Count; col++)
                {
                    PropertyDescriptor prop = properties[col];
                    string value = prop.GetValue(item)?.ToString();
                    Console.WriteLine($"{value}|");
                }

                Console.Line();
            }

            Console.Wait();
        }

        private sealed record User(int Id, string Name, bool IsActive);

        private static List<User> LoadUsersFromDatabase()
        {
            Console.WriteLine("Lade Users aus der Datenbank …");

            return
            [
                new User(1, "Alice", false),
                new User(2, "Bob", false),
                new User(3, "Charlie", true),
                new User(4, "Donald Duck", true)
            ];
        }
    }
}
