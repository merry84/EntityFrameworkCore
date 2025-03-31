using System.Xml.Serialization;

namespace SocialNetwork.DataProcessor.ImportDTOs;

[XmlType("Post")]
public class ExportPostDto
{
    public string Content { get; set; }

    [XmlElement(DataType = "string")]
    public string CreatedAt { get; set; }
}