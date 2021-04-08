using Model.Domain_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Database_configuration
{
    public static class DBInit
    {
        public static void Initialize(DBContext context)
        {
            // Se om noe eksisterer i databasen
            if (context.Users.Any() |
                context.Posts.Any() |
                context.Comments.Any() |
                context.InfoTopics.Any() |
                context.Topics.Any() |
                context.SubTopics.Any())
            {
                return;
            }

            // Instillinger for tidssone
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);

            // Opprette nye temaer (TOPICS)
            var topics = new Topic[]
            {
                new Topic{Title="Konkurranse",Description="Informasjon om Konkurranse. Fylles ut senere!",ImageUrl="images/kategori.konkurranse.jpg"},
                new Topic{Title="Kompetanse",Description="Informasjon om Kompetanse. Fylles ut senere!",ImageUrl="images/kategori.kompetanse.jpg"},
                new Topic{Title="Utvikling",Description="Informasjon om Utvikling. Fylles ut senere!",ImageUrl="images/kategori.utvikling.jpg"},
                new Topic{Title="Toppidrett",Description="Informasjon om Toppidrett. Fylles ut senere!",ImageUrl="images/kategori.toppidrett.jpg"}
            };
            foreach (Topic topic in topics)
            {
                context.Topics.Add(topic);
                context.SaveChanges();
            }

            // Opprette nye undertemaer (SUBTOPICS)
            var subtopics = new SubTopic[]
            {
                new SubTopic{Title="Dommer / oppmann",Description="Informasjon om Dommer / oppmann. Fylles ut senere!",TopicId=1},
                new SubTopic{Title="Seriespill",Description="Informasjon om Seriespill. Fylles ut senere!",TopicId=1},
                new SubTopic{Title="Lokale turneringer",Description="Informasjon om Lokale turneringer. Fylles ut senere!",TopicId=1},
                new SubTopic{Title="Rankingturneringer",Description="Informasjon om Rankingturneringer. Fylles ut senere!",TopicId=1},
                new SubTopic{Title="Mesterskap",Description="Informasjon om Mesterskap. Fylles ut senere!",TopicId=1},
                new SubTopic{Title="Trening",Description="Informasjon om Trening. Fylles ut senere!",TopicId=2},
                new SubTopic{Title="Trenere",Description="Informasjon om Trenere. Fylles ut senere!",TopicId=2},
                new SubTopic{Title="Skolebadminton",Description="Informasjon om Skolebadminton. Fylles ut senere!",TopicId=2},
                new SubTopic{Title="Klubbutvikling",Description="Informasjon om Klubbutvikling. Fylles ut senere!",TopicId=3},
                new SubTopic{Title="Rekruttering",Description="Informasjon om Rekruttering. Fylles ut senere!",TopicId=3},
                new SubTopic{Title="Parabadminton",Description="Informasjon om Parabadminton. Fylles ut senere!",TopicId=3},
                new SubTopic{Title="Anlegg",Description="Informasjon om Anlegg. Fylles ut senere!",TopicId=3},
                new SubTopic{Title="Junior",Description="Informasjon om Junior. Fylles ut senere!",TopicId=4},
                new SubTopic{Title="Senior",Description="Informasjon om Senior. Fylles ut senere!",TopicId=4},
                new SubTopic{Title="Para",Description="Informasjon om Para. Fylles ut senere!",TopicId=4},
                new SubTopic{Title="Trening",Description="Informasjon om Trening. Fylles ut senere!",TopicId=4}
            };
            foreach (SubTopic subtopic in subtopics)
            {
                context.SubTopics.Add(subtopic);
                context.SaveChanges();
            }

            // Opprette nye brukere (USERS)
            var users = new User[]
            {
                new User{Username="tommy",FirstName="Tommy",LastName="Ø. Pedersen",Password="0000"},
                new User{Username="henrik",FirstName="Henrik",LastName="N. Hjellup",Password="0000"},
                new User{Username="erik",FirstName="Erik",LastName="S. Larsen",Password="0000"},
                new User{Username="pia",FirstName="Pia K.",LastName="Aamodt ",Password="0000"},
                new User{Username="sepita",FirstName="Sepideh",LastName="Tajik",Password="0000"},
                new User{Username="test",FirstName="Test",LastName="User",Password="0000"},
                new User{Username="charlotte",FirstName="Charlotte",LastName="Støelen",Password="0000"},
                new User{Username="admin",FirstName="Superbruker",LastName="Badminton",Password="1234"}
            };
            foreach (User user in users)
            {
                context.Users.Add(user);
                context.SaveChanges();
            }

            // Opprette nye poster (POSTS)
            var posts = new Post[]
            {
                new Post{Title="Test for tema: Konkurranse",Content="Dette er en test! Den skal ligge under Dommer / oppmann",Date=now,UserId=6,TopicId=1,SubTopicId=1},
                new Post{Title="Test for tema: Konkurranse",Content="Denne skal ligge under Seriespill",Date=now,UserId=6,TopicId=1,SubTopicId=2},
                new Post{Title="Test for tema: Kompetanse",Content="Denne testen skal ligge under Trening",Date=now,UserId=6,TopicId=2,SubTopicId=6},
                new Post{Title="Test for tema: Utvikling",Content="Dette er enda en test. Den skal ligge under Klubbutvikling",Date=now,UserId=6,TopicId=3,SubTopicId=9},
                new Post{Title="Test for tema: Toppidrett",Content="Dette er en test!!! Denne skal ligge under Junior",Date=now,UserId=6,TopicId=4,SubTopicId=13},
                new Post{Title="Test for tema: Toppidrett",Content="Denne testen skal man kunne se under Trening",Date=now,UserId=6,TopicId=4,SubTopicId=16}
            };
            foreach (Post post in posts)
            {
                context.Posts.Add(post);
                context.SaveChanges();
            }

            // Opprette nye kommentarer (COMMENTS)
            var comments = new Comment[]
            {
                new Comment{Content="Dette er et testsvar til en post :)",Date=now,UserId=6,PostId=1},
                new Comment{Content="Dette svaret er kun en test!!!",Date=now,UserId=6,PostId=2},
                new Comment{Content="Hei, det ser ut som posten ligger under riktig tema :)",Date=now,UserId=6,PostId=3},
                new Comment{Content="Dette testsvaret er ikke så viktig.",Date=now,UserId=6,PostId=3},
                new Comment{Content="Dette skal være det nyeste svaret på denne posten.",Date=now,UserId=6,PostId=3},
                new Comment{Content="Dette skal være det første svaret til posten under Toppidrett - > Trening",Date=now,UserId=6,PostId=6},
                new Comment{Content="I posten under Toppidrett og Trening, blir dette det neste svar",Date=now,UserId=6,PostId=6}
            };
            foreach (Comment comment in comments)
            {
                context.Comments.Add(comment);
                context.SaveChanges();
            }

            // Opprette nye temaer (INFOTOPICS)
            var infotopics = new InfoTopic[]
            {
                new InfoTopic{Title="Klubbutvikling",Description="Informasjon om Klubbutvikling. Fylles ut senere!",ImageUrl="..."},
                new InfoTopic{Title="Trener",Description="Informasjon om Trener. Fylles ut senere!",ImageUrl="..."},
                new InfoTopic{Title="Spiller",Description="Informasjon om Spiller. Fylles ut senere!",ImageUrl="..."},
                new InfoTopic{Title="Dommer",Description="Informasjon om Dommer. Fylles ut senere!",ImageUrl="..."}
            };
            foreach (InfoTopic infotopic in infotopics)
            {
                context.InfoTopics.Add(infotopic);
                context.SaveChanges();
            }

            // Opprette nye videoer (VIDEOS)
            var videos = new Video[]
            {
                new Video{Title="Video nummer 1",Description="Denne skal ligge i Klubbutvikling",YouTubeId="3NBPQ9RLOu0",UserId=6,InfoTopicId=1},
                new Video{Title="Video nummer 2",Description="Denne skal ligge i Trener",YouTubeId="3NBPQ9RLOu0",UserId=6,InfoTopicId=2},
                new Video{Title="Video nummer 3",Description="Denne skal ligge i Spiller",YouTubeId="3NBPQ9RLOu0",UserId=6,InfoTopicId=3},
                new Video{Title="Video nummer 4",Description="Denne skal ligge i Dommer",YouTubeId="3NBPQ9RLOu0",UserId=6,InfoTopicId=4}
            };
            foreach (Video video in videos)
            {
                context.Videos.Add(video);
                context.SaveChanges();
            }
        }
    }
}
