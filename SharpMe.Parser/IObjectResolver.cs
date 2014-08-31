using System.IO;

namespace SharpMe.Parser
{
  public interface IObjectResolver
  {
    string Parse(string fileName, string className);
    string Parse(StreamReader stream, string className);
    string ParseContent(string content, string className);
  }
}
