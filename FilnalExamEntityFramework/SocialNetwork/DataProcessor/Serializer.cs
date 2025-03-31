using System.Globalization;
using System.Text.Json;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SocialNetwork.Data;
using SocialNetwork.DataProcessor.ExportDTOs;
using SocialNetwork.DataProcessor.ImportDTOs;
using SocialNetwork.Utilities;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;


namespace SocialNetwork.DataProcessor
{
    public class Serializer
    {
        public static string ExportUsersWithFriendShipsCountAndTheirPosts(SocialNetworkDbContext dbContext)
        {
            var users = dbContext.Users
                .Select(u => new ExportUserDto
                {
                    Username = u.Username,
                    Friendships = dbContext.Friendships
                        .Count(f => f.UserOneId == u.Id || f.UserTwoId == u.Id),
                    Posts = u.Posts
                        .OrderBy(p => p.Id)
                        .Select(p => new ExportPostDto
                        {
                            Content = p.Content,
                            CreatedAt = p.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)
                        })
                        .ToList()
                })
                .OrderBy(u => u.Username)
                .ToList();

            return HelpClass.Serialize(users, "Users");
        }

        public static string ExportConversationsWithMessagesChronologically(SocialNetworkDbContext dbContext)
        {
            string result = string.Empty;
            var conversations = dbContext.Conversations
                .Include(c => c.Messages)
                .ThenInclude(m => m.Sender)
                .OrderBy(c => c.StartedAt)
                .Select(c => new ExportConversationDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    StartedAt = c.StartedAt.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                    Messages = c.Messages
                        .OrderBy(m => m.SentAt)
                        .Select(m => new ExportMessageDto
                        {
                            Content = m.Content,
                            SentAt = m.SentAt.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                            Status = (int)m.Status,
                            SenderUsername = m.Sender.Username
                        })
                        .ToList()
                })
                .ToList();

            result = JsonConvert.SerializeObject(conversations, Formatting.Indented);
            return result;
        }
    }
}
