using AutoMapper.Configuration.Annotations;
using GameLibraryV2.Helper;

namespace GameLibraryV2.Models
{
    public class SearchParameters
    {
        const int maxPageSize = 50;

        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize { 
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
    }
}
