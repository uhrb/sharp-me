using System;
using System.IO;
using SharpMe.Parser;

namespace SharpMe
{
  public class Program
  {
    public static void Main(string[] args)
    {
      if (args.Length == 0)
      {
        Usage();
        return;
      }

      var parser = new SharpMeParser();

      foreach (var s in args)
      {
        var newName = Path.GetFileNameWithoutExtension(s);
        var content = parser.Parse(s, newName);
        newName += ".cs";
        using (var fileWriter = File.CreateText(newName))
        {
          fileWriter.WriteLine(content);
        }
      }
    }

    private static void Usage()
    {
       Console.WriteLine("Usage: sharpme jsonObject1.json jsonObject2.json ...");
    }
  }
}
