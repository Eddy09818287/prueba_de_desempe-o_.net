using SkillHubeduardo.Domain.Entities;

namespace SkillHubeduardo.Application.Interfaces;

public interface IPdfGenerator
{
    byte[] GenerateEnrollmentCertificate(Enrollment enrollment, DateTime generationDate);
}