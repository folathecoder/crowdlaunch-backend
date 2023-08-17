using AutoMapper;
using MARKETPLACEAPI.dto;
using MARKETPLACEAPI.Models;

namespace MARKETPLACEAPI.Helpers;

public class MappingProfiles : Profile
{
  public MappingProfiles()
  {
    CreateMap<CategoryCreateDto, Category>();
    CreateMap<CategoryUpdateDto, Category>();
    CreateMap<Category, CategoryUpdateDto>();
    CreateMap<Category, CategoryCreateDto>();
    CreateMap<SignInRegisterDto, User>();
    CreateMap<User, SignInRegisterDto>();
    CreateMap<User, SignInResponseDto>();
    CreateMap<SignInResponseDto, User>();
    CreateMap<NftCreateDto, Nft>();
    CreateMap<NftUpdateDto, Nft>();
    CreateMap<Nft, NftUpdateDto>();
    CreateMap<Nft, NftCreateDto>();
    CreateMap<Nft, NftDto>();
    CreateMap<NftDto, Nft>();
    CreateMap<UpdateNftLikeDto, NftLike>();
    CreateMap<BuyNftDto, NftLike>();
    CreateMap<NftLike, BuyNftDto>();
    CreateMap<NftLike, UpdateNftLikeDto>();
    CreateMap<UpdateNftLikeDto, NftLike>();
    CreateMap<PortfolioCreateDto, Portfolio>();
    CreateMap<Portfolio, PortfolioCreateDto>();
    CreateMap<Project, ProjectCreateDto>();
    CreateMap<ProjectCreateDto, Project>();
    CreateMap<Project, UpdateProjectDto>();
    CreateMap<UpdateProjectDto, Project>();
    CreateMap<Project, ProjectDto>();
    CreateMap<ProjectDto, Project>();
    CreateMap<ChangeProjectLikesDto, Project>();
    CreateMap<Project, ChangeProjectLikesDto>();
    CreateMap<ProjectDetail, ProjectDetailCreateDto>();
    CreateMap<ProjectDetailCreateDto, ProjectDetail>();
    CreateMap<ProjectLike, ProjectLikeCreateDto>();
    CreateMap<ProjectLikeCreateDto, ProjectLike>();
    CreateMap<ProjectUpdate, ProjectUpdateCreateDto>();
    CreateMap<ProjectUpdateCreateDto, ProjectUpdate>();
    CreateMap<User, UserDto>();
    CreateMap<UserDto, User>();
    CreateMap<UserUpdateDto, User>();
    CreateMap<User, UserUpdateDto>();
    CreateMap<UserNft, UserNftCreateDto>();
    CreateMap<UserNftCreateDto, UserNft>();
  }
}