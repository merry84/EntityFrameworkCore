namespace MusicHub
{
    using System;

    using Data;
    using Initializer;
    using System.Globalization;
    using System.Text;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
            //string result = ExportAlbumsInfo(context,9);
            string result = ExportSongsAboveDuration(context, 4);
            Console.WriteLine(result);

        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            //You need to write method string ExportAlbumsInfo(MusicHubDbContext context, int producerId) in the StartUp class that receives a ProducerId. Export all albums which are produced by the provided ProducerId. For each Album, get the Name, ReleaseDate in format the "MM/dd/yyyy", ProducerName, the Album Songs with each Song Name, Price (formatted to the second digit) and the Song WriterName. Sort the Songs by Song Name (descending) and by Writer (ascending). At the end export the Total Album Price with exactly two digits after the decimal place. Sort the Albums by their Total Price (descending).


            var selectAlbums = context.Albums
                .Where(a => a.ProducerId.HasValue && a.ProducerId == producerId)
                .ToArray()
                .OrderByDescending(a => a.Price)
                .Select(a => new
                {
                    a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = a.Producer.Name,
                    Song = a.Songs
                        .Select(s => new
                        {
                            SongName = s.Name,
                            Price = s.Price.ToString("f2"),
                            Writer = s.Writer.Name

                        })
                        .OrderByDescending(s => s.SongName)
                        .ThenBy(s => s.Writer)
                        .ToArray(),
                    AlbumsPrice = a.Price.ToString("f2")
                })
                .ToArray();

            StringBuilder sb = new();

          

            foreach (var album in selectAlbums)
            {
                sb.AppendLine($"-AlbumName: {album.Name}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                sb.AppendLine($"-Songs:");

                int songNumber = 1;

                foreach (var song in album.Song)
                {

                    sb.AppendLine($"---#{songNumber++}");
                    sb.AppendLine($"---SongName: {song.SongName}");
                    sb.AppendLine($"---Price: {song.Price}");
                    sb.AppendLine($"---Writer: {song.Writer}");

                }

                sb.AppendLine($"-AlbumPrice: {album.AlbumsPrice}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {

            //You need to write method string ExportSongsAboveDuration(MusicHubDbContext context, int duration) in the StartUp class that receives Song duration (integer, in seconds). Export the songs which are above the given duration. For each Song, export its Name, Performer Full Name, Writer Name, Album Producer and Duration (in format("c")). Sort the Songs by their Name (ascending), and then by Writer (ascending). If a Song has more than one Performer, export all performers and sort them (ascending, alphabetically). If there are no Performers for a given song, don't print the "---Performer" line at all.
           
            var songs = context.Songs
                .AsEnumerable() // Принуждаваме обработката да стане в паметта (след зареждането от БД)
                .Where(s => s.Duration.TotalSeconds > duration) 
                .Select(s => new
                {
                    s.Name,
                    Performers = s.SongPerformers
                        .Select(sp => sp.Performer)
                        .OrderBy(p => p.FirstName)
                        .ThenBy(p => p.LastName)
                        .ToList(),
                    Writer = s.Writer.Name,
                    Producer = s.Album!.Producer!.Name, // Проверяваме за null
                    Duration = s.Duration.ToString("c") 
                })
                .OrderBy(s => s.Name)  
                .ThenBy(s => s.Writer) 
                .ToList();

            var result = new StringBuilder();
            int songNumber = 1;

            foreach (var song in songs)
            {
                result.AppendLine($"-Song #{songNumber++}");
                result.AppendLine($"---SongName: {song.Name}");
                result.AppendLine($"---Writer: {song.Writer}");

                foreach (var performer in song.Performers)
                {
                    result
                        .AppendLine($"---Performer: {$"{performer.FirstName} {performer.LastName}"}");
                }

                result.AppendLine($"---AlbumProducer: {song.Producer}");
                result.AppendLine($"---Duration: {song.Duration}");
            }

            return result.ToString().TrimEnd();
        }
    }
}

