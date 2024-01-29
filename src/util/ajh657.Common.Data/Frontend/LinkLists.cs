using ajh657.Common.Data.Records;

namespace ajh657.Common.Data.Frontend
{
    public static class LinkLists
    {
        public static class Frontend
        {
            public static List<Link> GetMainLinks()
            {
                return
                [
                    new Link
                    {
                        IconClass = "fa-brands fa-github",
                        Name = "GitHub",
                        URL = "https://github.com/ajh657"
                    }
                ];
            }

            public class Furry
            {
                public static List<Link> GetHobbyLinks()
                {
                    return
                    [
                        new Link
                        {
                            IconClass = "fa-solid fa-book-bookmark",
                            Name = "FurAffinity",
                            URL = "/Furry/FFXIV"
                        }
                    ];
                }

                public static List<Link> GetSocialLinks()
                {
                    return
                    [
                        new Link
                        {
                            IconClass = "fa-brands fa-steam",
                            Name = "Steam",
                            URL = "https://steamcommunity.com/id/ajh657/"
                        },
                        new Link
                        {
                            IconClass = "fa-brands fa-mastodon",
                            Name = "Mastodon: @ajh657@meow.social",
                            URL = "https://meow.social/@ajh657"
                        },
                        new Link
                        {
                            IconClass = "fa-brands fa-discord",
                            Name = "Discord: AJH657"
                        }
                    ];
                }
            }
        }
    }
}
