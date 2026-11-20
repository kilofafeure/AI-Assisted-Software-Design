
using System;
using System.IO;

if (args.Length == 0)
{
    Console.WriteLine("Debes indicar la ruta.");
    return;
}

string path = args[0];

string[] files = Directory.GetFiles(
    path,
    "*.cs",
    SearchOption.AllDirectories
);


Console.WriteLine($"Archivos C#: {files.Length}");
foreach (string file in files)
{
    Console.WriteLine(Path.GetFileName(file));
}
