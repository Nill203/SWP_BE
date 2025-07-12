using System.Runtime.Serialization;

namespace BloodDonationBE.Common.Enums;

public enum BloodType
{
    [EnumMember(Value = "A+")]
    A_POS,

    [EnumMember(Value = "A-")]
    A_NEG,

    [EnumMember(Value = "B+")]
    B_POS,
    
    [EnumMember(Value = "B-")]
    B_NEG,

    [EnumMember(Value = "AB+")]
    AB_POS,

    [EnumMember(Value = "AB-")]
    AB_NEG,

    [EnumMember(Value = "O+")]
    O_POS,

    [EnumMember(Value = "O-")]
    O_NEG,
    
    None
}