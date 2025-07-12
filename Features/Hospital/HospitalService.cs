using Microsoft.EntityFrameworkCore;
using BloodDonationBE.Data;
using BloodDonationBE.Features.Hospitals.DTOs;

namespace BloodDonationBE.Features.Hospitals;

public class HospitalService : IHospitalService
{
    private readonly AppDbContext _context;

    public HospitalService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<HospitalResponseDto> CreateHospitalAsync(CreateHospitalDto dto)
    {
        // Kiểm tra xem tên bệnh viện đã tồn tại chưa
        if (await _context.Hospitals.AnyAsync(h => h.Name == dto.Name))
        {
            throw new BadHttpRequestException($"Bệnh viện với tên '{dto.Name}' đã tồn tại.");
        }

        var hospital = new Hospital
        {
            Name = dto.Name,
            Address = dto.Address,
            ContactInfo = dto.ContactInfo
        };

        await _context.Hospitals.AddAsync(hospital);
        await _context.SaveChangesAsync();

        return HospitalResponseDto.FromEntity(hospital);
    }

    public async Task<IEnumerable<HospitalResponseDto>> GetAllHospitalsAsync()
    {
        var hospitals = await _context.Hospitals.ToListAsync();
        return hospitals.Select(HospitalResponseDto.FromEntity);
    }

    public async Task<HospitalResponseDto> GetHospitalByIdAsync(int id)
    {
        var hospital = await _context.Hospitals.FindAsync(id);
        if (hospital == null)
        {
            throw new KeyNotFoundException($"Không tìm thấy bệnh viện với ID {id}");
        }
        return HospitalResponseDto.FromEntity(hospital);
    }

    public async Task<HospitalResponseDto> UpdateHospitalAsync(int id, UpdateHospitalDto dto)
    {
        var hospital = await _context.Hospitals.FindAsync(id);
        if (hospital == null)
        {
            throw new KeyNotFoundException($"Không tìm thấy bệnh viện với ID {id}");
        }

        // Cập nhật các trường nếu chúng được cung cấp trong DTO
        if (!string.IsNullOrEmpty(dto.Name)) hospital.Name = dto.Name;
        if (!string.IsNullOrEmpty(dto.Address)) hospital.Address = dto.Address;
        if (dto.ContactInfo != null) hospital.ContactInfo = dto.ContactInfo;

        await _context.SaveChangesAsync();
        return HospitalResponseDto.FromEntity(hospital);
    }

    public async Task DeleteHospitalAsync(int id)
    {
        var hospital = await _context.Hospitals.FindAsync(id);
        if (hospital == null)
        {
            throw new KeyNotFoundException($"Không tìm thấy bệnh viện với ID {id}");
        }

        _context.Hospitals.Remove(hospital);
        await _context.SaveChangesAsync();
    }
}
