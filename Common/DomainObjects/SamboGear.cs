using Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class SamboGear : IntIdentityBase
{
    [Required, MaxLength(50)]
    public string ModelCode { get; set; } = string.Empty; // e.g. "SB-SR-3"

    [MaxLength(50)]
    public string Family { get; set; } = string.Empty; // e.g. "SB-SR"

    [MaxLength(20)]
    public string SizeCode { get; set; } = string.Empty; // e.g. "3"

    public int? OutputTorqueNm { get; set; }

    public int? MaxThrustN { get; set; }

    public decimal? StemBoreMm { get; set; }

    public decimal? StemBoreInches { get; set; }

    public int? TurnsToOpen { get; set; }

    public decimal? EfficiencyRun { get; set; }

    public decimal? EfficiencyStall { get; set; }

    public int? HandwheelDiameterMm { get; set; }

    public decimal? WeightKg { get; set; }

    [MaxLength(100)]
    public string? KeySize { get; set; }

    [MaxLength(50)]
    public string? MountingIso { get; set; }

    public string? Notes { get; set; }

    public ICollection<SamboGearMountingBase> MountingBases { get; set; } = new List<SamboGearMountingBase>();
}
