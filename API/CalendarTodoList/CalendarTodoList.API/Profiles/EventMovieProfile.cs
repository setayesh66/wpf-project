using System;
using AutoMapper;
using BeetleMovies.API.Entities;
using BeetleMovies.API.DTOs;

namespace BeetleMovies.API.Profiles;
public class BeetleMovieProfile : Profile
{
    public BeetleMovieProfile()
    { 
        CreateMap<TaskItem, TaskDTO>().ReverseMap();
        CreateMap<TaskItem, TaskForCreatingDTO>().ReverseMap();
        CreateMap<TaskItem, TaskForUpdatingDTO>().ReverseMap();
    }
}
