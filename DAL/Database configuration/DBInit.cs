using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Database_configuration
{
    public static class DBInit
    {
        public static void Initialize(DBContext context)
        {
            // Se om noe eksisterer i databasen
            if (context.Topics.Any() |
                context.SubTopics.Any() |
                context.InfoTopics.Any() |
                context.Users.Any())
            {
                return;
            }

            // Opprette nye temaer (TOPICS)
            var topics = new Topic[]
            {
                new Topic{Title="Konkurranse",Description="Informasjon om Konkurranse.",ImageUrl="images/kategori.konkurranse.jpg"},
                new Topic{Title="Kompetanse",Description="Informasjon om Kompetanse.",ImageUrl="images/kategori.kompetanse.jpg"},
                new Topic{Title="Utvikling",Description="Informasjon om Utvikling.",ImageUrl="images/kategori.utvikling.jpg"},
                new Topic{Title="Toppidrett",Description="Informasjon om Toppidrett.",ImageUrl="images/kategori.toppidrett.jpg"},
                new Topic{Title="Kunnskapsportalen",Description="Informasjon om Kunnskapsportalen.",ImageUrl="images/kategori.kunnskapsportalen.jpg"}
            };
            foreach (Topic topic in topics)
            {
                context.Topics.Add(topic);
                context.SaveChanges();
            }

            // Opprette nye undertemaer (SUBTOPICS)
            var subtopics = new SubTopic[]
            {
                new SubTopic{Title="Dommer / oppmann",Description="Her kan du slå av en prat om dommer / oppmann",TopicId=1},
                new SubTopic{Title="Seriespill",Description="Her kan du slå av en prat om seriespill",TopicId=1},
                new SubTopic{Title="Lokale turneringer",Description="Her kan du slå av en prat om lokale turneringer",TopicId=1},
                new SubTopic{Title="Rankingturneringer",Description="Her kan du slå av en prat om rankingturneringer",TopicId=1},
                new SubTopic{Title="Mesterskap",Description="Her kan du slå av en prat om mesterskap",TopicId=1},
                new SubTopic{Title="Trening bredde",Description="Her kan du slå av en prat om trening bredde",TopicId=2},
                new SubTopic{Title="Trenere",Description="Her kan du slå av en prat om trenere",TopicId=2},
                new SubTopic{Title="Skolebadminton",Description="Her kan du slå av en prat om skolebadminton",TopicId=2},
                new SubTopic{Title="Klubbutvikling",Description="Her kan du slå av en prat om klubbutvikling",TopicId=3},
                new SubTopic{Title="Rekruttering",Description="Her kan du slå av en prat om rekruttering",TopicId=3},
                new SubTopic{Title="Parabadminton",Description="Her kan du slå av en prat om parabadminton",TopicId=3},
                new SubTopic{Title="Anlegg",Description="Her kan du slå av en prat om anlegg",TopicId=3},
                new SubTopic{Title="Junior",Description="Her kan du slå av en prat om junior",TopicId=4},
                new SubTopic{Title="Senior",Description="Her kan du slå av en prat om senior",TopicId=4},
                new SubTopic{Title="Para",Description="Her kan du slå av en prat om para",TopicId=4},
                new SubTopic{Title="Trening",Description="Her kan du slå av en prat om trening",TopicId=4},
                new SubTopic{Title="Klubbutvikling",Description="Her kan du slå av en prat om klubbutvikling",TopicId=5},
                new SubTopic{Title="Trener",Description="Her kan du slå av en prat om trener",TopicId=5},
                new SubTopic{Title="Spiller",Description="Her kan du slå av en prat om spiller",TopicId=5},
                new SubTopic{Title="Dommer",Description="Her kan du slå av en prat om dommer",TopicId=5}
            };
            foreach (SubTopic subtopic in subtopics)
            {
                context.SubTopics.Add(subtopic);
                context.SaveChanges();
            }

            // Opprette nye temaer (INFOTOPICS)
            var infotopics = new InfoTopic[]
            {
                new InfoTopic{Title="Klubbutvikling",Description="Informasjon om Klubbutvikling."},
                new InfoTopic{Title="Trener",Description="Informasjon om Trener."},
                new InfoTopic{Title="Spiller",Description="Informasjon om Spiller."},
                new InfoTopic{Title="Dommer",Description="Informasjon om Dommer."}
            };
            foreach (InfoTopic infotopic in infotopics)
            {
                context.InfoTopics.Add(infotopic);
                context.SaveChanges();
            }

            // Opprette nye brukere (USERS)
            byte[] passwordSalt = AddSalt();
            byte[] passwordHash = AddHash("password", passwordSalt);
            var users = new User[]
            {
                new User{Username="sysadmin",FirstName="Superbruker",LastName="NFB",Email="admin@badminton.no",Admin=true,Password=passwordHash,Salt=passwordSalt}
            };
            foreach (User user in users)
            {
                context.Users.Add(user);
                context.SaveChanges();
            }

            // Instillinger for tidsone
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);

            // Instillinger for antall oppføringer
            int userCount = 0;
            int postCount = 0;
            int commentCount = 0;
            var random = new Random();

            // Tekst til tittler
            string[] postTitle = new string[5]
            {
                "Dette er en testpost!",
                "Post om et kjedelig tema...",
                "Til den det måtte gjelde",
                "Les dette!!!",
                "En dag på banen"
            };

            // Tekst til poster
            string[] postContent = new string[5]
            {
                "Hva syntes du om denne posten?",
                "Det var to tomater som skulle gå over en vei.. så ja, nei!",
                "Hei. Hvor mange likes får denne posten?",
                "Husk å ta med gymbaggen på fredag :)",
                "Nei forresten, bare glem det!"
            };

            // Tekst til kommentarer
            string[] commentContent = new string[5]
            {
                "Hva var det du sa fornoe?",
                "Dette testsvaret er ikke så viktig.",
                "Heihei, blablabla :P",
                "Ja neida, sååå... OKEY!!!",
                "Hei, ville bare si at jeg er helt enig"
            };

            // Brukere
            var testUsers = new List<User>();
            byte[] testPasswordSalt = AddSalt();
            byte[] testPasswordHash = AddHash("0000", passwordSalt);
            for (int i = 1; i <= userCount; i++)
            {
                var user = new User
                {
                    Username = "testbruker" + i,
                    FirstName = "Test",
                    LastName = "Bruker",
                    Email = "testbruker" + i + "@test.no",
                    Password = testPasswordHash,
                    Salt = testPasswordSalt
                };

                testUsers.Add(user);
            }
            foreach (var user in testUsers)
            {
                context.Users.Add(user);
            }
            context.SaveChanges();

            // Poster
            var testPosts = new List<Post>();
            for (int i = 1; i <= postCount; i++)
            {
                int t = random.Next(1, 5);
                int c = random.Next(1, 5);
                int userId = random.Next(2, userCount);
                int subTopicId = random.Next(1, 20);

                var post = new Post
                {
                    Title = postTitle[t],
                    Content = postContent[c],
                    Date = now,
                    UserId = userId,
                    SubTopicId = subTopicId
                };

                testPosts.Add(post);
            }
            foreach (var post in testPosts)
            {
                context.Posts.Add(post);
            }
            context.SaveChanges();

            // Kommentarer
            var testComments = new List<Comment>();
            for (int i = 1; i <= commentCount; i++)
            {
                int c = random.Next(1, 5);
                int userId = random.Next(2, userCount);
                int postId = random.Next(1, postCount);

                var comment = new Comment
                {
                    Content = commentContent[c],
                    Date = now,
                    UserId = userId,
                    PostId = postId
                };

                testComments.Add(comment);
            }
            foreach (var comment in testComments)
            {
                context.Comments.Add(comment);
            }
            context.SaveChanges();
        }

        public static byte[] AddHash(string password, byte[] salt)
        {
            const int keyLength = 24;
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
            return pbkdf2.GetBytes(keyLength);
        }

        public static byte[] AddSalt()
        {
            var csprng = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            csprng.GetBytes(salt);
            return salt;
        }
    }
}
