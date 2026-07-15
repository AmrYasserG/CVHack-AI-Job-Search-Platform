namespace CVHack.BLL.DTOs;

public class GenerateCvRequestDto
{
    public int JobId { get; set; }
}

public class GenerateCvResponseDto
{
    public string CvText { get; set; } = null!;
}