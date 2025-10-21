using Reviews.DTOs;

public interface IReviewService
{
    Task<IResult> PostReview(ReviewCreateDto reviewCreateDto, string userId);

    Task<IEnumerable<ReviewReadDto>> GetReviewsByUser(string userId);

    Task<IEnumerable<ReviewReadDto>> GetReviewsByMovie(int MovieId);

    Task<ReviewReadDto> GetReviewById(int reviewId);

    Task<IEnumerable<ReviewReadRankingDto>> GetMostReviewedMovies(int page,int perPage);




}