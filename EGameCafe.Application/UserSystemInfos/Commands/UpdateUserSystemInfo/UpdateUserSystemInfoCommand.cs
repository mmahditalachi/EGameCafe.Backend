using AutoMapper;
using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.Common.Interfaces;
using EGameCafe.Application.Common.Mappings;
using EGameCafe.Application.Common.Models;
using EGameCafe.Domain.Entities;
using EGameCafe.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EGameCafe.Application.UserSystemInfos.Commands.UpdateUserSystemInfo
{
    public class UpdateUserSystemInfoCommand : IRequest<Result>, IMapFrom<UpdateUserSystemInfoCommand>
    {
        public string UserSystemInfoId { get; set; }

        public SystemManufacturer RamManufacturer { get; set; }
        public int TotalRam { get; set; }
        public SystemManufacturer GraphicCardManufacturer { get; set; }
        public string GraphicCardName { get; set; }
        public CpuManufacturer CpuManufacturer { get; set; }
        public string CpuName { get; set; }
        public SystemManufacturer CaseManufacturer { get; set; }
        public string CaseName { get; set; }
        public SystemManufacturer PowerManufacturer { get; set; }
        public string PowerName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateUserSystemInfoCommand, UserSystemInfo>()
                 .ForMember(dest => dest.UserId, opt => opt.Ignore())
                 .ForMember(dest => dest.UserDetail, opt => opt.Ignore());
        }
    }

    public class Handler : IRequestHandler<UpdateUserSystemInfoCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateUserSystemInfoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var item = await _context.UserSystemInfo.FirstOrDefaultAsync(e => e.UserSystemInfoId == request.UserSystemInfoId);

                if (item == null)
                {
                    throw new NotFoundException();
                }

                _mapper.Map(request, item);

                _context.UserSystemInfo.Update(item);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success(item.UserSystemInfoId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
