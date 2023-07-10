using AutoMapper;
using Practical18_Using_View.DataModels;
using Practical18_Using_View.ViewModels;

namespace Practical18_Using_View.Mappings;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<StudentViewModel, StudentModel>();
    }
}
