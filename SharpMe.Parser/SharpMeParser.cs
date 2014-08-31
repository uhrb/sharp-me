using System;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace SharpMe.Parser
{
  public class SharpMeParser : IObjectResolver
  {
    static SharpMeParser()
    {
      ClassHeaderTemplate = "public class {0}";
      PropertyTemplate = "  public {0} {1} {{ get; set; }}";
    }

    public static string ClassHeaderTemplate { get; set; }
    public static string PropertyTemplate { get; set; }

    public string Parse(string fileName, string className)
    {
      if (string.IsNullOrEmpty(fileName))
      {
        throw new ArgumentNullException("fileName");
      }

      using (var reader = File.OpenText(fileName))
      {
         return Parse(reader, className);
      }
    }

    public string Parse(StreamReader stream, string className)
    {
      if (stream == null)
      {
        throw new ArgumentNullException("stream");
      }

      return ParseContent(stream.ReadToEnd(), className);
    }

    public string ParseContent(string content, string className)
    {
      content = content ?? string.Empty;

      var jobject = JObject.Parse(content);
      var sb = new StringBuilder();
      sb.AppendFormat(ClassHeaderTemplate, CultureInfo.CurrentCulture.TextInfo.ToTitleCase(className));
      sb.Append("\n{");
      foreach (var prop in jobject.Properties())
      {
        var type = GetTypeName(prop.Value.Type);
        sb.Append("\n");
        sb.AppendFormat(PropertyTemplate, type, CultureInfo.CurrentCulture.TextInfo.ToTitleCase(prop.Name));
      }

      sb.Append("\n}");
      return sb.ToString();
    }

    private static string GetTypeName(JTokenType type)
    {
      switch (type)
      {
        case JTokenType.Boolean:
          return "bool";
        case JTokenType.Bytes:
          return "byte[]";
        case JTokenType.Date:
          return "DateTime";
        case JTokenType.Float:
          return "double";
        case JTokenType.Guid:
          return "Guid";
        case JTokenType.Integer:
          return "int";
        case JTokenType.String:
          return "string";
        case JTokenType.Uri:
          return "string";
        default:
          return "object";
      }
    }
  }
}
