using Reviews.DTOs;

public interface IReviewService
{
    Task<IResult> PostReview(ReviewCreateDto reviewCreateDto);

}