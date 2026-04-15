# Console ItemsSource

![NET](https://img.shields.io/badge/NET-10.0-green.svg)
![License](https://img.shields.io/badge/License-MIT-blue.svg)
![VS2026](https://img.shields.io/badge/Visual%20Studio-2026-white.svg)
![Version](https://img.shields.io/badge/Version-1.0.2026.0-yellow.svg)

## Projekt zur Funktionsweise von ItemsSource

Das Beispiel soll zeigen wie auch WPF intern mit ItemsSource arbeitet. Hierbei ist das Verständnis von `TypeDescriptor` und `PropertyDescriptor` wichtig.

## Hinweis
der Source ist soll auch einfache Art und Weise die Funktionen eines Features zeigen. Der Source ist so geschrieben, das so wenig wie möglich zusätzliche NuGet-Pakete benötigt werden.

## Beispielsource

Das Beispiel ist nur rudimentär und soll nur die Funktionsweise von ItemSource zeigen. Es ist nicht für den produktiven Einsatz gedacht.

Wichtig ist, das der ItemsSource als `IEnumerable` definiert ist, damit die Datenquelle flexibel ist und z.B. auch eine `DataTable` (das DataTable muß in dem Beispiel allerdings als DataView übergeben werden, da sonst die Komplexität steil ansteigt) oder `List<T>` sein kann. Das `DisplayMemberPath` ist optional und gibt an, welche Spalte angezeigt werden soll. Wenn es nicht gesetzt ist, werden alle Spalten angezeigt.
```csharp
private static IEnumerable ItemsSource { get; set; }
private static string DisplayMemberPath { get; set; }
```

Lesen der Daten z.B. aus einer Datenbank
```csharp
/* Nur das Property 'Name' anzeigen */
DisplayMemberPath = "Name";
ItemsSource = LoadUsersFromDatabase(); /* List<T> */
/* Oder */
ItemsSource = LoadUsersFromDatabaseHS(); /* HashSet<T> */
/* Oder */
ItemsSource = LoadUsersFromDatabaseDT().AsDataView(); /* DataTable als DataView*/
```

if (ItemsSource == null)
{
    return;
}

var items = ItemsSource.Cast<object>().ToList();
if (items.Any() == false)
{
    return;
}
```

Spaltennamen ermitteln und anzeigen
```csharp
// 1. Typ bestimmen
Type itemType = items.First().GetType();

// 2. Properties holen (WPF-Mechanismus!)
var properties = TypeDescriptor.GetProperties(itemType)
                                .Cast<PropertyDescriptor>()
                                .ToList();
foreach (PropertyDescriptor p in properties)
{
    Console.WriteLine(p.Name);
}
```
Datenzeilen ermitteln und anzeigen. Hier wird dann auch der `DisplayMemberPath` berücksichtigt, also nur die Spalte 'Name' angezeigt.
Wird der `DisplayMemberPath` nicht gesetzt, werden alle Spalten angezeigt.
```csharp
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
```

# Versionshistorie
![Version](https://img.shields.io/badge/Version-1.0.2026.0-yellow.svg)
- Migration auf NET 10
