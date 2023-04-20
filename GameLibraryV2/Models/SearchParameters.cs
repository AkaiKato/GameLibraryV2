using GameLibraryV2.Helper;
using GameLibraryV2.Interfaces;

namespace GameLibraryV2.Models
{
    public class SearchParameters
    {
        //-----------------------------------------------
        const int maxPageSize = 50;

        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize 
        { 
            get 
            { 
                return _pageSize; 
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        //-----------------------------------------------
        public int MinYearOfRelease { get; set; } = 1800;

        public int MaxYearOfRelease { get; set; } = 3000;

        protected internal bool ValidYearRange => MaxYearOfRelease > MinYearOfRelease;

        //-----------------------------------------------
        public double MinPlayTime { get; set; } = 0;

        public double MaxPlayTime { get; set; } = 10000;

        protected internal bool ValidPlayTime => MaxPlayTime > MinPlayTime;

        //-----------------------------------------------
        public double MinRating { get; set;} = 0;

        public double MaxRating { get; set;} = 10;

        protected internal bool ValidRating => MinRating >= 0 && MaxRating <= 10;

        //-----------------------------------------------
        public string[]? Status { get; set; }

        protected internal bool ValidStatus => StatusIsValid(Status);

        private static bool StatusIsValid(string[]? status)
        {
            if (status == null)
                return true;

            var hash = new HashSet<string>(Enum.GetNames(typeof(Enums.Status)));

            for (int i = 0; i < status.Length; i++)
            { 
                if(!hash.Contains(status[i].Trim().ToLower()))
                    return false;
            }

            return true;
        }

        //----------------------------------------------
        public string[]? Type { get; set; }

        protected internal bool ValidType => TypeIsValid(Type);

        private static bool TypeIsValid(string[]? type)
        {
            if(type == null) 
                return true;

            var hash = new HashSet<string>(Enum.GetNames(typeof(Enums.Types)));

            for (int i = 0; i < type.Length; i++)
            {
                if (!hash.Contains(type[i].Trim().ToLower()))
                    return false;
            }

            return true;
        }

        //----------------------------------------------
        public string[]? Genre { get; set; }

        protected internal bool GenreEquals(IList<Genre> obj, string[] obj2)
        {
            foreach (var item in obj2)
            {
                if (obj.Any(x => x.Name.Trim().ToLower() == item.Trim().ToLower()))
                    continue;
                return false;
            }
            return true;
        }

        //----------------------------------------------
        public string[]? Tag { get; set; }

        protected internal bool TagEquals(IList<Tag> obj, string[] obj2)
        {
            foreach (var item in obj2)
            {
                if (obj.Any(x => x.Name.Trim().ToLower() == item.Trim().ToLower()))
                    continue;
                return false;
            }
            return true;
        }

        //----------------------------------------------
        public string[]? Platform { get; set; }

        protected internal bool PlatformEquals(IList<Platform> obj, string[] obj2)
        {
            foreach (var item in obj2)
            {
                if (obj.Any(x => x.Name.Trim().ToLower() == item.Trim().ToLower()))
                    continue;
                return false;
            }
            return true;
        }

        //----------------------------------------------
        public string[]? Developer { get; set; }

        protected internal bool DeveloperEquals(IList<Developer> obj, string[] obj2)
        {
            foreach (var item in obj)
            {
                if (obj2.Any(x => x.Trim().ToLower() == item.Name.Trim().ToLower()))
                    continue;
                return false;
            }
            return true;
        }

        //----------------------------------------------
        public string[]? Publisher { get; set; }

        protected internal bool PublisherEquals(IList<Publisher> obj, string[] obj2)
        {
            foreach (var item in obj)
            {
                if (obj2.Any(x => x.Trim().ToLower() == item.Name.Trim().ToLower()))
                    continue;
                return false;
            }
            return true;
        }

        //----------------------------------------------
        public string[]? AgeRating { get; set; }

        //----------------------------------------------
        public bool NSFW { get; set; } = false;

    }
}
