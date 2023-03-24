﻿using GameLibraryV2.Models;

namespace GameLibraryV2.Interfaces
{
    public interface IReviewRepository 
    {
        public bool CreateReview(Review review);
    }
}
