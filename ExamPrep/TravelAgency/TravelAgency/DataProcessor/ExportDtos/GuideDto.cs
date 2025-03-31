using System.Xml.Serialization;

namespace TravelAgency.DataProcessor.ExportDtos;
[XmlType("Guide")]
public class GuideDto
{
    [XmlElement("FullName")]
    public string FullName { get; set; } = null!;

    [XmlArray("TourPackages")]
    [XmlArrayItem("TourPackage")]
    public ExportTourPackageDto[] TourPackages { get; set; } = null!;
}