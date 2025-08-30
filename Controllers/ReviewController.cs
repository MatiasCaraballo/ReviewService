using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Reviews.DTOs;

[ApiController]
[Route("[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<ReviewCreateDto>> PostReview(ReviewCreateDto reviewCreateDto)
    {
        try
        {
            var review = await _reviewService.PostReview(reviewCreateDto);

            if (review == null) { return NotFound(); }
            return Ok(review);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server error");
        }

    }
}