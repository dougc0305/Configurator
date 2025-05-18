using Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class SamboGearMountingBase : IntIdentityBase
{
    public int SamboGearId { get; set; }

    [MaxLength(50)]
    public string MountingBaseCode { get; set; } = string.Empty; // e.g. "S1", "S2"

    [ForeignKey(nameof(SamboGearId))]
    public SamboGear SamboGear { get; set; } = null!;
}
