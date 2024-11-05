using AutoMapper;
using MongoDB.Bson;
using Post.Model;

namespace Post.Mapper
{
	public class MappingProfile: Profile
	{
		public MappingProfile() {
			CreateMap<Posts, postDTO>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
				
			CreateMap<postDTO, Posts>()
		   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => ObjectId.Parse(src.Id)));
			
		}
	}
}
