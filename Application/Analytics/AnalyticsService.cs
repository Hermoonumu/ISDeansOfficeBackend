using DeanInfoSystem.API;
using DeanInfoSystem.Application.Common.Exceptions;
using DeanInfoSystem.Application.Common.Mappers;
using DeanInfoSystem.Application.DTO;
using DeanInfoSystem.Application.Programs;
using DeanInfoSystem.Application.StudentGrades;
using DeanInfoSystem.Application.Users;
using DeanInfoSystem.Domain;
using Microsoft.AspNetCore.Identity;

namespace DeanInfoSystem.Application.Analytics;



public class AnalyticsService(IAnalyticsRepository _analRepo,
                                IProgramRepository _progRepo,
                                IUserRepository _userRepo,
                                IStudentGradeRepository _sgRepo) : IAnalyticsService
{
    public async Task<GradeDistributionDTO> GetGradeDistInProgramAsync(Guid ProgramId)
    {
        EdProgram program = await _progRepo.GetProgramByGuidAsync(ProgramId) ??
        throw new ProgramDoesntExistException("No such program");
        List<GradeBucketsDTO> gradeBuckets = await _analRepo.GetProgramGradeBucketsAsync(ProgramId);
        GradeDistributionDTO gdDTO = new()
        {
            ProgramId = ProgramId,
            Program = program,
            TotalGradeCount = gradeBuckets.Sum(gb => gb.Count),
            Distribution = gradeBuckets
        };

        foreach (GradeBucketsDTO gb in gradeBuckets)
        {
            gb.Percentage = (double)gb.Count / gdDTO.TotalGradeCount;
        }

        return gdDTO;
    }

    public async Task<RankingDTO> GetStudentRankingInProgramAsync(Guid ProgramId)
    {
        EdProgram program = await _progRepo.GetProgramByGuidAsync(ProgramId)
        ?? throw new ProgramDoesntExistException("No such program");
        List<User> StudentsEnrolled = await _userRepo.GetAllUsersInProgramAsync(ProgramId);
        List<StudentGrade> sgs = await _sgRepo.GetStudentGradeByStudentIdRangeAsync(
            StudentsEnrolled.Select(u => u.Id).ToList()
        );

        Dictionary<Guid, List<StudentGrade>> sgByGuid = sgs.GroupBy(e => e.StudentId)
                                                            .ToDictionary(e => (Guid)e.Key, l => l.ToList());


        RankingDTO rDTO = new()
        {
            ProgramId = ProgramId,
            Program = program
        };
        List<RankingEntryDTO> reDTOList = [];
        foreach (User user in StudentsEnrolled)
        {
            List<StudentGrade> UserGrades = sgByGuid.GetValueOrDefault(user.Id, []);
            int passed = 0;
            int failed = 0;
            int sum = 0;
            foreach (StudentGrade sg in UserGrades)
            {
                if (sg.Status != Status.Passed)
                {
                    failed++; continue;
                }
                passed++;
                sum += (int)sg.Grade;
            }

            if (passed <= 0) continue;

            double AvgGrade = (double)sum / passed;

            reDTOList.Add(new()
            {
                StudentId = user.Id,
                Student = UserMapper.UserToDTO(user),
                AverageGrade = AvgGrade,
                HasAcademicDebt = failed > 0
            });
        }
        reDTOList = reDTOList.OrderBy(r => r.AverageGrade).ToList();
        int ctr = 0;
        reDTOList.ForEach(r => r.Rank = ++ctr);
        rDTO.RankingList = reDTOList;
        return rDTO;
    }
}