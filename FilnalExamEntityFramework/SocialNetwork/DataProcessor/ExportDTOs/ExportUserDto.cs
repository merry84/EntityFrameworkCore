using SocialNetwork.DataProcessor.ImportDTOs;
using System.Xml.Serialization;

namespace SocialNetwork.DataProcessor.ExportDTOs;

[XmlType("User")]
public class ExportUserDto
{
    [XmlAttribute("Friendships")]
    public int Friendships { get; set; }

    public string Username { get; set; }

    [XmlArray("Posts")]
    public List<ExportPostDto> Posts { get; set; }
}