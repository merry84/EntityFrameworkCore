using SocialNetwork.Data;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using SocialNetwork.Data.Enums;
using SocialNetwork.Data.Models;
using SocialNetwork.DataProcessor.ImportDTOs;
using SocialNetwork.Utilities;

namespace SocialNetwork.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format.";
        private const string DuplicatedDataMessage = "Duplicated data.";

        private const string SuccessfullyImportedMessageEntity =
            "Successfully imported message (Sent at: {0}, Status: {1})";

        private const string SuccessfullyImportedPostEntity =
            "Successfully imported post (Creator {0}, Created at: {1})";

        public static string ImportMessages(SocialNetworkDbContext dbContext, string xmlString)
        {

            StringBuilder sb = new StringBuilder();

            ImportMessagesDto[]? messagesDtos = HelpClass.Deserialize<ImportMessagesDto[]>(xmlString, "Messages");

            ICollection<Message> messages = new List<Message>();

            if (messagesDtos != null && messagesDtos.Length > 0)
            {
                foreach (ImportMessagesDto messageDto in messagesDtos)
                {
                    if (!IsValid(messageDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }


                    bool isSentAtValid = DateTime
                        .TryParseExact(messageDto.SentAt, "yyyy-MM-ddTHH:mm:ss", CultureInfo
                            .InvariantCulture, DateTimeStyles.None, out DateTime sentAtDate);


                    bool isStatusValid =
                        Enum.TryParse<MessageStatus>(messageDto.Status, out MessageStatus status);
                    bool existConversation =
                        dbContext.Conversations.Any(c => c.Id == messageDto.ConversationId);
                    bool existSenderId = dbContext.Users.Any(u => u.Id == messageDto.SenderId);

                    if (!isSentAtValid || !isStatusValid || !existConversation || !existSenderId)
                    {
                        sb.AppendLine(ErrorMessage);

                        continue;
                    }

                    bool messageExistsInBatch = messages.Any(m =>
                        m.Content == messageDto.Content &&
                        m.SentAt == sentAtDate &&
                        m.Status == status &&
                        m.SenderId == messageDto.SenderId &&
                        m.ConversationId == messageDto.ConversationId);

                    bool messageExistsInDb = dbContext.Messages.Any(m =>
                        m.Content == messageDto.Content &&
                        m.SentAt == sentAtDate &&
                        m.Status == status &&
                        m.SenderId == messageDto.SenderId &&
                        m.ConversationId == messageDto.ConversationId);
                    if (messageExistsInBatch || messageExistsInDb)
                    {
                        sb.AppendLine(DuplicatedDataMessage);
                        continue;
                    }

                    Message message = new Message
                    {
                        Content = messageDto.Content,
                        SentAt = sentAtDate,
                        Status = status,
                        ConversationId = messageDto.ConversationId,
                        SenderId = messageDto.SenderId
                    };

                    messages.Add(message);

                    sb.AppendLine(string.Format(SuccessfullyImportedMessageEntity, sentAtDate.ToString("yyyy-MM-ddTHH:mm:ss"), status));
                }

                dbContext.Messages.AddRange(messages);
                dbContext.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportPosts(SocialNetworkDbContext dbContext, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportPostDto[]? postsDtos = JsonConvert.DeserializeObject<ImportPostDto[]>(jsonString);

                ICollection<Post> posts = new List<Post>();
            if (postsDtos != null && postsDtos.Length > 0)
            {
                foreach (ImportPostDto postDto in postsDtos)
                {
                    if (!IsValid(postDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isDateValid = DateTime.TryParseExact(
                        postDto.CreatedAt,
                        "yyyy-MM-ddTHH:mm:ss",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime createdAt);
                    var creator = dbContext.Users.Find(postDto.CreatorId);

                    if (!isDateValid || creator==null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    

                    bool isPostExist = dbContext.Posts.Any(p => p.Content == postDto.Content
                                                               && p.CreatedAt == createdAt
                                                               && p.CreatorId == postDto.CreatorId);

                    bool isPostExistInBatch = posts.Any(p => p.Content == postDto.Content
                                                             && p.CreatedAt == createdAt
                                                             && p.CreatorId == postDto.CreatorId);  
                    if (isPostExistInBatch || isPostExist)
                    {
                        sb.AppendLine(DuplicatedDataMessage);
                        continue;
                    }

                    Post post = new Post
                    {
                        Content = postDto.Content,
                        CreatedAt = createdAt,
                        CreatorId = postDto.CreatorId
                    };
                    posts.Add(post);

                   
                        sb.AppendLine(string.Format(SuccessfullyImportedPostEntity, creator.Username,
                            createdAt.ToString("yyyy-MM-ddTHH:mm:ss")));


                }

                dbContext.Posts.AddRange(posts);
                dbContext.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }



        public static bool IsValid(object dto)
        {
            ValidationContext validationContext = new ValidationContext(dto);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            foreach (ValidationResult validationResult in validationResults)
            {
                if (validationResult.ErrorMessage != null)
                {
                    string currentMessage = validationResult.ErrorMessage;
                }

            }

            return isValid;
        }
    }
}
