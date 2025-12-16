using AutoMapper;
using ProjectFinally.Models.DTOs.AdSense;
using ProjectFinally.Models.DTOs.Tasks;
using ProjectFinally.Models.DTOs.YouTube;
using ProjectFinally.Models.Entities;
using TaskEntity = ProjectFinally.Models.Entities.Task;

namespace ProjectFinally.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Video mappings
        CreateMap<Video, VideoDto>()
            .ForMember(dest => dest.ChannelName, opt => opt.MapFrom(src => src.Channel.ChannelName))
            .ForMember(dest => dest.ChannelOwnerId, opt => opt.MapFrom(src => src.Channel.OwnerId))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null));

        CreateMap<CreateVideoDto, Video>();
        CreateMap<UpdateVideoDto, Video>();

        // VideoAnalytics mappings
        CreateMap<VideoAnalytics, VideoAnalyticsDto>();

        // YouTubeChannel mappings
        CreateMap<YouTubeChannel, YouTubeChannelDto>()
            .ForMember(dest => dest.TotalVideos, opt => opt.MapFrom(src => src.Videos != null ? src.Videos.Count : 0));
        CreateMap<CreateYouTubeChannelDto, YouTubeChannel>();
        CreateMap<UpdateYouTubeChannelDto, YouTubeChannel>();

        // VideoCategory mappings
        CreateMap<VideoCategory, VideoCategoryDto>();
        CreateMap<CreateVideoCategoryDto, VideoCategory>();
        CreateMap<UpdateVideoCategoryDto, VideoCategory>();

        // AdSenseCampaign mappings
        CreateMap<AdSenseCampaign, AdSenseCampaignDto>()
            .ForMember(dest => dest.ChannelName, opt => opt.MapFrom(src => src.Channel.ChannelName))
            .ForMember(dest => dest.ChannelOwnerId, opt => opt.MapFrom(src => src.Channel.OwnerId));
        CreateMap<CreateAdSenseCampaignDto, AdSenseCampaign>();
        CreateMap<UpdateAdSenseCampaignDto, AdSenseCampaign>();

        // AdRevenue mappings
        CreateMap<AdRevenue, AdRevenueDto>()
            .ForMember(dest => dest.VideoTitle, opt => opt.MapFrom(src => src.Video != null ? src.Video.Title : null))
            .ForMember(dest => dest.CampaignName, opt => opt.MapFrom(src => src.Campaign != null ? src.Campaign.CampaignName : null));
        CreateMap<CreateAdRevenueDto, AdRevenue>();
        CreateMap<UpdateAdRevenueDto, AdRevenue>();

        // Task mappings
        CreateMap<TaskEntity, TaskDto>()
            .ForMember(dest => dest.CreatedByUsername, opt => opt.MapFrom(src => src.CreatedByUser.Username))
            .ForMember(dest => dest.CreatedByFullName, opt => opt.MapFrom(src => src.CreatedByUser.FirstName + " " + src.CreatedByUser.LastName))
            .ForMember(dest => dest.AssignedToEmployeeName, opt => opt.MapFrom(src => src.AssignedToEmployee != null ? src.AssignedToEmployee.User.FirstName + " " + src.AssignedToEmployee.User.LastName : null))
            .ForMember(dest => dest.AssignedToEmployeeCode, opt => opt.MapFrom(src => src.AssignedToEmployee != null ? src.AssignedToEmployee.EmployeeCode : null));
        CreateMap<CreateTaskDto, TaskEntity>();
        CreateMap<UpdateTaskDto, TaskEntity>();

        // TaskComment mappings
        CreateMap<TaskComment, TaskCommentDto>()
            .ForMember(dest => dest.TaskTitle, opt => opt.MapFrom(src => src.Task.Title))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName));
        CreateMap<CreateTaskCommentDto, TaskComment>();
    }
}
