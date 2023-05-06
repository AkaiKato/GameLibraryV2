using GameLibraryV2.Helper;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Models
{
    public class FilterParameters : Pagination
    {
        public int MinYearOfRelease { get; set; } = 1800;

        public int MaxYearOfRelease { get; set; } = 3000;

        public double MinPlayTime { get; set; } = 0;

        public double MaxPlayTime { get; set; } = 10000;

        public double MinRating { get; set; } = 0;

        public double MaxRating { get; set; } = 10;

        public string[]? Status { get; set; }

        public string[]? Type { get; set; }

        public string[]? Genre { get; set; }

        public string[]? Tag { get; set; }

        public string[]? Platform { get; set; }

        public string[]? Developer { get; set; }

        public string[]? Publisher { get; set; }

        public string[]? AgeRating { get; set; }

        public bool NSFW { get; set; } = false;

        public bool Rating { get; set; } = true;

        protected internal bool ValidYearRange => MaxYearOfRelease > MinYearOfRelease;

        protected internal bool ValidPlayTime => MaxPlayTime > MinPlayTime;

        protected internal bool ValidRating => MinRating >= 0 && MaxRating <= 10;

        protected internal bool ValidStatus => StatusIsValid(Status);

        protected internal bool ValidType => TypeIsValid(Type);

        private static bool StatusIsValid(string[]? status)
        {
            if (status == null)
                return true;

            var hash = new HashSet<string>(Enum.GetNames(typeof(Enums.Status)));

            for (int i = 0; i < status.Length; i++)
            {
                if (!hash.Contains(status[i].Trim().ToLower()))
                    return false;
            }

            return true;
        }

        private static bool TypeIsValid(string[]? type)
        {
            if (type == null)
                return true;

            var hash = new HashSet<string>(Enum.GetNames(typeof(Enums.Types)));

            for (int i = 0; i < type.Length; i++)
            {
                if (!hash.Contains(type[i].Trim().ToLower()))
                    return false;
            }

            return true;
        }

        protected internal bool TagEquals(IList<Tag> obj, string[] obj2)
        {
            foreach (var item in obj2)
            {
                if (item == null)
                    continue;
                if (obj.Any(x => x.Name.Trim().ToLower() == item.Trim().ToLower()))
                    continue;
                return false;
            }
            return true;
        }

        protected internal bool PlatformEquals(IList<Platform> obj, string[] obj2)
        {
            foreach (var item in obj2)
            {
                if (item == null)
                    continue;
                if (obj.Any(x => x.Name.Trim().ToLower() == item.Trim().ToLower()))
                    continue;
                return false;
            }
            return true;
        }

        protected internal bool DeveloperEquals(IList<Developer> obj, string[] obj2)
        {
            foreach (var item in obj)
            {
                if (obj2.Where(x => x != null).Any(x => x.Trim().ToLower() == item.Name.Trim().ToLower()))
                    continue;
                return false;
            }
            return true;
        }

        protected internal bool PublisherEquals(IList<Publisher> obj, string[] obj2)
        {
            foreach (var item in obj)
            {
                if (obj2.Where(x => x != null).Any(x => x.Trim().ToLower() == item.Name.Trim().ToLower()))
                    continue;
                return false;
            }
            return true;
        }
    }
}
