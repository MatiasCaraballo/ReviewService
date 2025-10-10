namespace Reviews.DTOs;
using System.ComponentModel.DataAnnotations;


public class ReviewCreateDto
{
    [Required]

    public int MovieId { get; set; }

    [Required]

    public string Text { get; set; } = string.Empty;

    [Required]

    public int Rating { get; set; }

}

public class ReviewReadDto
{
    public int ReviewId { get; set; }

    [Required]
    public string UserId { get; set; }

    public int MovieId { get; set; }

    public string Text { get; set; } = string.Empty;

    public int Rating { get; set; } 

}