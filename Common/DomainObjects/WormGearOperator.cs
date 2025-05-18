using Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class WormGearOperator : IntIdentityBase
{
    [Required, MaxLength(50)]
    public string PartNumber { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Family { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Size { get; set; } = string.Empty;

    public int TorqueRatingNm { get; set; }

    [MaxLength(50)]
    public string MountingStandard { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Material { get; set; } = string.Empty;

    [MaxLength(50)]
    public string OperationType { get; set; } = "Manual";

    public bool HandwheelIncluded { get; set; } = true;

    public bool Weatherproof { get; set; } = true;

    public string? Notes { get; set; }
}
