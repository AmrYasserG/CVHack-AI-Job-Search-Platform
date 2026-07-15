using System;
using System.Collections.Generic;
using System.Text;
using CVHack.BLL.DTOs;

namespace CVHack.BLL.Services.CvGenerator;

public interface ICvGeneratorService
{
    Task<GenerateCvResponseDto> GenerateAsync(string userId, int jobId);
}