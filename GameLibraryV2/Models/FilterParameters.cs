using GameLibraryV2.Helper;

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

        public string? AgeRating { get; set; }

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
                if (status[i] == null || !hash.Contains(status[i].Trim().ToLower()))
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
                if (type[i] == null || !hash.Contains(type[i].Trim().ToLower()))
                {
                        return false;
                }
            }

            return true;
        }
    }
}
