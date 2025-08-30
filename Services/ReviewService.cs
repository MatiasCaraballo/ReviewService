using data.ApplicationDbContext;
using Reviews.Models;
using Reviews.DTOs;

public class ReviewService : IReviewService
{
    private readonly ReviewsDbContext _context;

    public ReviewService(ReviewsDbContext context)
    {
        _context = context;
    } 

    public async Task<IResult> PostReview(ReviewCreateDto reviewCreateDto)
    {
        var review = new Review
        {
            UserId = reviewCreateDto.UserId,
            MovieId = reviewCreateDto.MovieId,
            Text = reviewCreateDto.Text,
            Rating = reviewCreateDto.Rating,
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return TypedResults.Created($"/reviews/{review.ReviewId}", review);
    }
}