using Reviews.DTOs;

public interface IReviewService
{
    Task<IResult> PostReview(ReviewCreateDto reviewCreateDto, string userId);

    Task<IEnumerable<ReviewReadDto>> GetReviewsByUser(string userId);

    Task<IEnumerable<ReviewReadDto>> GetReviewsByMovie(int MovieId);

    Task<ReviewReadDto> GetReviewById(int reviewId);

    Task<IEnumerable<ReviewReadRankingDto>> GetMostReviewedMovies(int page, int perPage);
    
    Task<IEnumerable<ReviewRankingRatingDto>> GetBestReviewedMovies(int page, int perPage);

    Task<IEnumerable<ReviewRankingRatingDto>> GetWorstReviewedMovies(int page, int perPage);

    Task<IResult> UpdateReview(int reviewId, string userId, ReviewUpdateDto reviewUpdateDto);

    Task<IResult> DeleteReview(int reviewId,string userId,string userRole);





}