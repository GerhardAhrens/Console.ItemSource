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
    using System.Data;

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
            mainMenu.AddItem("ItemSource Property", MenuPoint1);
            mainMenu.AddItem("Beenden", () => ApplicationExit());
            mainMenu.Show();
        }

        private static IEnumerable ItemsSource { get; set; }

        private static string DisplayMemberPath { get; set; }

        private static string SelectedValuePath { get; set; }

        private static void ApplicationExit()
        {
            Environment.Exit(0);
        }

        private static void MenuPoint1()
        {
            Console.Clear();

            /* Nur das Property 'Name' anzeigen */
            DisplayMemberPath = "Name";
            ItemsSource = LoadUsersFromDatabaseHS();

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
            var firstItem = items.First();

            // 2. Properties holen (WPF-Mechanismus!)
            var properties = TypeDescriptor.GetProperties(firstItem)
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
                    if (DisplayMemberPath != null && properties[col].Name != DisplayMemberPath)
                    {
                        continue;
                    }

                    PropertyDescriptor prop = properties[col];
                    var rawValue = prop.GetValue(item);
                    var displayValue = DisplayMemberPath != null ? GetPropertyValue(item, DisplayMemberPath) : rawValue;
                    Console.WriteLine($"{displayValue}|");
                }

                Console.Line();
            }

            Console.Wait();
        }

        private static object GetPropertyValue(object item, string path)
        {
            if (item == null || string.IsNullOrEmpty(path))
            {
                return item;
            }

            var props = TypeDescriptor.GetProperties(item);
            var prop = props[path];

            if (prop != null)
            {
                return prop.GetValue(item);
            }

            // Fallback für DataRowView
            if (item is DataRowView drv && drv.Row.Table.Columns.Contains(path))
            {
                return drv[path];
            }

            return null;
        }

        /* Demo Daten */
        private sealed record User(int Id, string Name, bool IsActive);

        private static List<User> LoadUsersFromDatabase()
        {
            Console.WriteLine("Lade Users aus der Datenbank als List<T>");

            return
            [
                new User(1, "Alice", false),
                new User(2, "Bob", false),
                new User(3, "Charlie", true),
                new User(4, "Donald Duck", true)
            ];
        }

        private static HashSet<User> LoadUsersFromDatabaseHS()
        {
            Console.WriteLine("Lade Users aus der Datenbank als HashSet<T>");

            return
            [
                new User(1, "Alice", false),
                new User(2, "Bob", false),
                new User(3, "Charlie", true),
                new User(4, "Donald Duck", true)
            ];
        }

        private static DataTable CreateStruktur()
        {
            DataTable dt = new DataTable("DataTableDemo");

            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("IsActive", typeof(bool));

            return dt;
        }

        private static DataTable LoadUsersFromDatabaseDT()
        {
            Console.WriteLine("Lade Users aus der Datenbank als DataTable");

            DataTable dt = CreateStruktur();
            DataRow dr = dt.NewRow();
            dr["Id"] = 1;
            dr["Name"] = "Alice";
            dr["IsActive"] = false;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 2;
            dr["Name"] = "Bob";
            dr["IsActive"] = false;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 3;
            dr["Name"] = "Charlie";
            dr["IsActive"] = true;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Id"] = 4;
            dr["Name"] = "Donald Duck";
            dr["IsActive"] = true;
            dt.Rows.Add(dr);

            return dt;
        }
    }
}
