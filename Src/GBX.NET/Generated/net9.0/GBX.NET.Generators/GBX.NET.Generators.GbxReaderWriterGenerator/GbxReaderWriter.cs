using System.Diagnostics.CodeAnalysis;

namespace GBX.NET.Serialization;

partial interface IGbxReaderWriter
{
    bool GbxMagic();

    [return: NotNullIfNotNull(nameof(value))]
    byte Byte(byte value = default);
    [return: NotNullIfNotNull(nameof(value))]
    byte? Byte(byte? value, byte defaultValue = default);
    void Byte([NotNullIfNotNull(nameof(value))] ref byte value);
    void Byte([NotNullIfNotNull(nameof(value))] ref byte? value, byte defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    sbyte SByte(sbyte value = default);
    [return: NotNullIfNotNull(nameof(value))]
    sbyte? SByte(sbyte? value, sbyte defaultValue = default);
    void SByte([NotNullIfNotNull(nameof(value))] ref sbyte value);
    void SByte([NotNullIfNotNull(nameof(value))] ref sbyte? value, sbyte defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    short Int16(short value = default);
    [return: NotNullIfNotNull(nameof(value))]
    short? Int16(short? value, short defaultValue = default);
    void Int16([NotNullIfNotNull(nameof(value))] ref short value);
    void Int16([NotNullIfNotNull(nameof(value))] ref short? value, short defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    ushort UInt16(ushort value = default);
    [return: NotNullIfNotNull(nameof(value))]
    ushort? UInt16(ushort? value, ushort defaultValue = default);
    void UInt16([NotNullIfNotNull(nameof(value))] ref ushort value);
    void UInt16([NotNullIfNotNull(nameof(value))] ref ushort? value, ushort defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    int Int32(int value = default);
    [return: NotNullIfNotNull(nameof(value))]
    int? Int32(int? value, int defaultValue = default);
    void Int32([NotNullIfNotNull(nameof(value))] ref int value);
    void Int32([NotNullIfNotNull(nameof(value))] ref int? value, int defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    uint UInt32(uint value = default);
    [return: NotNullIfNotNull(nameof(value))]
    uint? UInt32(uint? value, uint defaultValue = default);
    void UInt32([NotNullIfNotNull(nameof(value))] ref uint value);
    void UInt32([NotNullIfNotNull(nameof(value))] ref uint? value, uint defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    long Int64(long value = default);
    [return: NotNullIfNotNull(nameof(value))]
    long? Int64(long? value, long defaultValue = default);
    void Int64([NotNullIfNotNull(nameof(value))] ref long value);
    void Int64([NotNullIfNotNull(nameof(value))] ref long? value, long defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    ulong UInt64(ulong value = default);
    [return: NotNullIfNotNull(nameof(value))]
    ulong? UInt64(ulong? value, ulong defaultValue = default);
    void UInt64([NotNullIfNotNull(nameof(value))] ref ulong value);
    void UInt64([NotNullIfNotNull(nameof(value))] ref ulong? value, ulong defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    float Single(float value = default);
    [return: NotNullIfNotNull(nameof(value))]
    float? Single(float? value, float defaultValue = default);
    void Single([NotNullIfNotNull(nameof(value))] ref float value);
    void Single([NotNullIfNotNull(nameof(value))] ref float? value, float defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    int HexInt32(int value = default);
    [return: NotNullIfNotNull(nameof(value))]
    int? HexInt32(int? value, int defaultValue = default);
    void HexInt32([NotNullIfNotNull(nameof(value))] ref int value);
    void HexInt32([NotNullIfNotNull(nameof(value))] ref int? value, int defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    uint HexUInt32(uint value = default);
    [return: NotNullIfNotNull(nameof(value))]
    uint? HexUInt32(uint? value, uint defaultValue = default);
    void HexUInt32([NotNullIfNotNull(nameof(value))] ref uint value);
    void HexUInt32([NotNullIfNotNull(nameof(value))] ref uint? value, uint defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    int DataInt32(int value = default);
    [return: NotNullIfNotNull(nameof(value))]
    int? DataInt32(int? value, int defaultValue = default);
    void DataInt32([NotNullIfNotNull(nameof(value))] ref int value);
    void DataInt32([NotNullIfNotNull(nameof(value))] ref int? value, int defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    uint DataUInt32(uint value = default);
    [return: NotNullIfNotNull(nameof(value))]
    uint? DataUInt32(uint? value, uint defaultValue = default);
    void DataUInt32([NotNullIfNotNull(nameof(value))] ref uint value);
    void DataUInt32([NotNullIfNotNull(nameof(value))] ref uint? value, uint defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    long DataInt64(long value = default);
    [return: NotNullIfNotNull(nameof(value))]
    long? DataInt64(long? value, long defaultValue = default);
    void DataInt64([NotNullIfNotNull(nameof(value))] ref long value);
    void DataInt64([NotNullIfNotNull(nameof(value))] ref long? value, long defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    ulong DataUInt64(ulong value = default);
    [return: NotNullIfNotNull(nameof(value))]
    ulong? DataUInt64(ulong? value, ulong defaultValue = default);
    void DataUInt64([NotNullIfNotNull(nameof(value))] ref ulong value);
    void DataUInt64([NotNullIfNotNull(nameof(value))] ref ulong? value, ulong defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    System.Numerics.BigInteger BigInt(System.Numerics.BigInteger value, int byteLength);
    [return: NotNullIfNotNull(nameof(value))]
    System.Numerics.BigInteger? BigInt(System.Numerics.BigInteger? value, int byteLength, System.Numerics.BigInteger defaultValue);
    void BigInt([NotNullIfNotNull(nameof(value))] ref System.Numerics.BigInteger value, int byteLength);
    void BigInt([NotNullIfNotNull(nameof(value))] ref System.Numerics.BigInteger? value, int byteLength, System.Numerics.BigInteger defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    System.Int128 Int128(System.Int128 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    System.Int128? Int128(System.Int128? value, System.Int128 defaultValue = default);
    void Int128([NotNullIfNotNull(nameof(value))] ref System.Int128 value);
    void Int128([NotNullIfNotNull(nameof(value))] ref System.Int128? value, System.Int128 defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    System.UInt128 UInt128(System.UInt128 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    System.UInt128? UInt128(System.UInt128? value, System.UInt128 defaultValue = default);
    void UInt128([NotNullIfNotNull(nameof(value))] ref System.UInt128 value);
    void UInt128([NotNullIfNotNull(nameof(value))] ref System.UInt128? value, System.UInt128 defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.UInt256 UInt256(GBX.NET.UInt256 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.UInt256? UInt256(GBX.NET.UInt256? value, GBX.NET.UInt256 defaultValue = default);
    void UInt256([NotNullIfNotNull(nameof(value))] ref GBX.NET.UInt256 value);
    void UInt256([NotNullIfNotNull(nameof(value))] ref GBX.NET.UInt256? value, GBX.NET.UInt256 defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Checksum128 Checksum128(GBX.NET.Checksum128 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Checksum128? Checksum128(GBX.NET.Checksum128? value, GBX.NET.Checksum128 defaultValue = default);
    void Checksum128([NotNullIfNotNull(nameof(value))] ref GBX.NET.Checksum128 value);
    void Checksum128([NotNullIfNotNull(nameof(value))] ref GBX.NET.Checksum128? value, GBX.NET.Checksum128 defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Checksum256 Checksum256(GBX.NET.Checksum256 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Checksum256? Checksum256(GBX.NET.Checksum256? value, GBX.NET.Checksum256 defaultValue = default);
    void Checksum256([NotNullIfNotNull(nameof(value))] ref GBX.NET.Checksum256 value);
    void Checksum256([NotNullIfNotNull(nameof(value))] ref GBX.NET.Checksum256? value, GBX.NET.Checksum256 defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Int2 Int2(GBX.NET.Int2 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Int2? Int2(GBX.NET.Int2? value, GBX.NET.Int2 defaultValue = default);
    void Int2([NotNullIfNotNull(nameof(value))] ref GBX.NET.Int2 value);
    void Int2([NotNullIfNotNull(nameof(value))] ref GBX.NET.Int2? value, GBX.NET.Int2 defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Int3 Int3(GBX.NET.Int3 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Int3? Int3(GBX.NET.Int3? value, GBX.NET.Int3 defaultValue = default);
    void Int3([NotNullIfNotNull(nameof(value))] ref GBX.NET.Int3 value);
    void Int3([NotNullIfNotNull(nameof(value))] ref GBX.NET.Int3? value, GBX.NET.Int3 defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Int4 Int4(GBX.NET.Int4 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Int4? Int4(GBX.NET.Int4? value, GBX.NET.Int4 defaultValue = default);
    void Int4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Int4 value);
    void Int4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Int4? value, GBX.NET.Int4 defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Byte3 Byte3(GBX.NET.Byte3 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Byte3? Byte3(GBX.NET.Byte3? value, GBX.NET.Byte3 defaultValue = default);
    void Byte3([NotNullIfNotNull(nameof(value))] ref GBX.NET.Byte3 value);
    void Byte3([NotNullIfNotNull(nameof(value))] ref GBX.NET.Byte3? value, GBX.NET.Byte3 defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Vec2 Vec2(GBX.NET.Vec2 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Vec2? Vec2(GBX.NET.Vec2? value, GBX.NET.Vec2 defaultValue = default);
    void Vec2([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec2 value);
    void Vec2([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec2? value, GBX.NET.Vec2 defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Vec3 Vec3(GBX.NET.Vec3 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Vec3? Vec3(GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default);
    void Vec3([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3 value);
    void Vec3([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Vec3 Vec3_10b(GBX.NET.Vec3 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Vec3? Vec3_10b(GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default);
    void Vec3_10b([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3 value);
    void Vec3_10b([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Vec3 Vec3Unit2(GBX.NET.Vec3 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Vec3? Vec3Unit2(GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default);
    void Vec3Unit2([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3 value);
    void Vec3Unit2([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Vec3 Vec3_4(GBX.NET.Vec3 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Vec3? Vec3_4(GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default);
    void Vec3_4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3 value);
    void Vec3_4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Vec3 Vec3_6(GBX.NET.Vec3 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Vec3? Vec3_6(GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default);
    void Vec3_6([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3 value);
    void Vec3_6([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Vec3 Vec3Unit4(GBX.NET.Vec3 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Vec3? Vec3Unit4(GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default);
    void Vec3Unit4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3 value);
    void Vec3Unit4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Vec4 Vec4(GBX.NET.Vec4 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Vec4? Vec4(GBX.NET.Vec4? value, GBX.NET.Vec4 defaultValue = default);
    void Vec4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec4 value);
    void Vec4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec4? value, GBX.NET.Vec4 defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.BoxAligned BoxAligned(GBX.NET.BoxAligned value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.BoxAligned? BoxAligned(GBX.NET.BoxAligned? value, GBX.NET.BoxAligned defaultValue = default);
    void BoxAligned([NotNullIfNotNull(nameof(value))] ref GBX.NET.BoxAligned value);
    void BoxAligned([NotNullIfNotNull(nameof(value))] ref GBX.NET.BoxAligned? value, GBX.NET.BoxAligned defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.BoxInt3 BoxInt3(GBX.NET.BoxInt3 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.BoxInt3? BoxInt3(GBX.NET.BoxInt3? value, GBX.NET.BoxInt3 defaultValue = default);
    void BoxInt3([NotNullIfNotNull(nameof(value))] ref GBX.NET.BoxInt3 value);
    void BoxInt3([NotNullIfNotNull(nameof(value))] ref GBX.NET.BoxInt3? value, GBX.NET.BoxInt3 defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Color Color(GBX.NET.Color value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Color? Color(GBX.NET.Color? value, GBX.NET.Color defaultValue = default);
    void Color([NotNullIfNotNull(nameof(value))] ref GBX.NET.Color value);
    void Color([NotNullIfNotNull(nameof(value))] ref GBX.NET.Color? value, GBX.NET.Color defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Iso4 Iso4(GBX.NET.Iso4 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Iso4? Iso4(GBX.NET.Iso4? value, GBX.NET.Iso4 defaultValue = default);
    void Iso4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Iso4 value);
    void Iso4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Iso4? value, GBX.NET.Iso4 defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Mat3 Mat3(GBX.NET.Mat3 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Mat3? Mat3(GBX.NET.Mat3? value, GBX.NET.Mat3 defaultValue = default);
    void Mat3([NotNullIfNotNull(nameof(value))] ref GBX.NET.Mat3 value);
    void Mat3([NotNullIfNotNull(nameof(value))] ref GBX.NET.Mat3? value, GBX.NET.Mat3 defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Mat4 Mat4(GBX.NET.Mat4 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Mat4? Mat4(GBX.NET.Mat4? value, GBX.NET.Mat4 defaultValue = default);
    void Mat4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Mat4 value);
    void Mat4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Mat4? value, GBX.NET.Mat4 defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Quat Quat(GBX.NET.Quat value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Quat? Quat(GBX.NET.Quat? value, GBX.NET.Quat defaultValue = default);
    void Quat([NotNullIfNotNull(nameof(value))] ref GBX.NET.Quat value);
    void Quat([NotNullIfNotNull(nameof(value))] ref GBX.NET.Quat? value, GBX.NET.Quat defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Quat Quat6(GBX.NET.Quat value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Quat? Quat6(GBX.NET.Quat? value, GBX.NET.Quat defaultValue = default);
    void Quat6([NotNullIfNotNull(nameof(value))] ref GBX.NET.Quat value);
    void Quat6([NotNullIfNotNull(nameof(value))] ref GBX.NET.Quat? value, GBX.NET.Quat defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Rect Rect(GBX.NET.Rect value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Rect? Rect(GBX.NET.Rect? value, GBX.NET.Rect defaultValue = default);
    void Rect([NotNullIfNotNull(nameof(value))] ref GBX.NET.Rect value);
    void Rect([NotNullIfNotNull(nameof(value))] ref GBX.NET.Rect? value, GBX.NET.Rect defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.TransQuat TransQuat(GBX.NET.TransQuat value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.TransQuat? TransQuat(GBX.NET.TransQuat? value, GBX.NET.TransQuat defaultValue = default);
    void TransQuat([NotNullIfNotNull(nameof(value))] ref GBX.NET.TransQuat value);
    void TransQuat([NotNullIfNotNull(nameof(value))] ref GBX.NET.TransQuat? value, GBX.NET.TransQuat defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    bool Boolean(bool value = default);
    [return: NotNullIfNotNull(nameof(value))]
    bool? Boolean(bool? value, bool defaultValue = default);
    void Boolean([NotNullIfNotNull(nameof(value))] ref bool value);
    void Boolean([NotNullIfNotNull(nameof(value))] ref bool? value, bool defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    bool Boolean(bool value, bool asByte);
    [return: NotNullIfNotNull(nameof(value))]
    bool? Boolean(bool? value, bool asByte, bool defaultValue);
    void Boolean([NotNullIfNotNull(nameof(value))] ref bool value, bool asByte);
    void Boolean([NotNullIfNotNull(nameof(value))] ref bool? value, bool asByte, bool defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    bool Boolean(bool value, GBX.NET.Serialization.BoolType type);
    [return: NotNullIfNotNull(nameof(value))]
    bool? Boolean(bool? value, GBX.NET.Serialization.BoolType type, bool defaultValue);
    void Boolean([NotNullIfNotNull(nameof(value))] ref bool value, GBX.NET.Serialization.BoolType type);
    void Boolean([NotNullIfNotNull(nameof(value))] ref bool? value, GBX.NET.Serialization.BoolType type, bool defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    byte[]? Data(byte[]? value = default);
    void Data([NotNullIfNotNull(nameof(value))] ref byte[]? value);

    [return: NotNullIfNotNull(nameof(value))]
    System.Threading.Tasks.Task<byte[]> DataAsync(byte[]? value, System.Threading.CancellationToken cancellationToken = default);

    [return: NotNullIfNotNull(nameof(value))]
    byte[]? Data(byte[]? value, int length);
    void Data([NotNullIfNotNull(nameof(value))] ref byte[]? value, int length);

    [return: NotNullIfNotNull(nameof(value))]
    string? String(string? value = default);
    void String([NotNullIfNotNull(nameof(value))] ref string? value);
    string[]? ArrayString(string[]? value = default);
    string[]? ArrayString(string[]? value, int length);
    string[]? ArrayString_deprec(string[]? value);
    List<string>? ListString(List<string>? value = default);
    List<string>? ListString(List<string>? value, int length);
    List<string>? ListString_deprec(List<string>? value);
    void ArrayString(ref string[]? value);
    void ArrayString(ref string[]? value, int length);
    void ArrayString_deprec(ref string[]? value);
    void ListString(ref List<string>? value);
    void ListString(ref List<string>? value, int length);
    void ListString_deprec(ref List<string>? value);

    [return: NotNullIfNotNull(nameof(value))]
    string? String(string? value, GBX.NET.Serialization.StringLengthPrefix lengthPrefix);
    void String([NotNullIfNotNull(nameof(value))] ref string? value, GBX.NET.Serialization.StringLengthPrefix lengthPrefix);

    [return: NotNullIfNotNull(nameof(value))]
    string? IdAsString(string? value = default);
    void IdAsString([NotNullIfNotNull(nameof(value))] ref string? value);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Id Id(GBX.NET.Id value = default);
    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Id? Id(GBX.NET.Id? value, GBX.NET.Id defaultValue = default);
    void Id([NotNullIfNotNull(nameof(value))] ref GBX.NET.Id value);
    void Id([NotNullIfNotNull(nameof(value))] ref GBX.NET.Id? value, GBX.NET.Id defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Ident? Ident(GBX.NET.Ident? value = default);
    void Ident([NotNullIfNotNull(nameof(value))] ref GBX.NET.Ident? value);
    GBX.NET.Ident[]? ArrayIdent(GBX.NET.Ident[]? value = default);
    GBX.NET.Ident[]? ArrayIdent(GBX.NET.Ident[]? value, int length);
    GBX.NET.Ident[]? ArrayIdent_deprec(GBX.NET.Ident[]? value);
    List<GBX.NET.Ident>? ListIdent(List<GBX.NET.Ident>? value = default);
    List<GBX.NET.Ident>? ListIdent(List<GBX.NET.Ident>? value, int length);
    List<GBX.NET.Ident>? ListIdent_deprec(List<GBX.NET.Ident>? value);
    void ArrayIdent(ref GBX.NET.Ident[]? value);
    void ArrayIdent(ref GBX.NET.Ident[]? value, int length);
    void ArrayIdent_deprec(ref GBX.NET.Ident[]? value);
    void ListIdent(ref List<GBX.NET.Ident>? value);
    void ListIdent(ref List<GBX.NET.Ident>? value, int length);
    void ListIdent_deprec(ref List<GBX.NET.Ident>? value);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.PackDesc? PackDesc(GBX.NET.PackDesc? value = default);
    void PackDesc([NotNullIfNotNull(nameof(value))] ref GBX.NET.PackDesc? value);
    GBX.NET.PackDesc[]? ArrayPackDesc(GBX.NET.PackDesc[]? value = default);
    GBX.NET.PackDesc[]? ArrayPackDesc(GBX.NET.PackDesc[]? value, int length);
    GBX.NET.PackDesc[]? ArrayPackDesc_deprec(GBX.NET.PackDesc[]? value);
    List<GBX.NET.PackDesc>? ListPackDesc(List<GBX.NET.PackDesc>? value = default);
    List<GBX.NET.PackDesc>? ListPackDesc(List<GBX.NET.PackDesc>? value, int length);
    List<GBX.NET.PackDesc>? ListPackDesc_deprec(List<GBX.NET.PackDesc>? value);
    void ArrayPackDesc(ref GBX.NET.PackDesc[]? value);
    void ArrayPackDesc(ref GBX.NET.PackDesc[]? value, int length);
    void ArrayPackDesc_deprec(ref GBX.NET.PackDesc[]? value);
    void ListPackDesc(ref List<GBX.NET.PackDesc>? value);
    void ListPackDesc(ref List<GBX.NET.PackDesc>? value, int length);
    void ListPackDesc_deprec(ref List<GBX.NET.PackDesc>? value);

    [return: NotNullIfNotNull(nameof(value))]
    T? NodeRef<T>(T? value = default) where T : GBX.NET.IClass;
    void NodeRef<T>([NotNullIfNotNull(nameof(value))] ref T? value) where T : GBX.NET.IClass;

    [return: NotNullIfNotNull(nameof(value))]
    T? NodeRef<T>(T? value, ref GBX.NET.Components.GbxRefTableFile? file) where T : GBX.NET.IClass;
    void NodeRef<T>([NotNullIfNotNull(nameof(value))] ref T? value, ref GBX.NET.Components.GbxRefTableFile? file) where T : GBX.NET.IClass;

    [return: NotNullIfNotNull(nameof(value))]
    T? Node<T>(T? value = default) where T : GBX.NET.IClass, new();
    void Node<T>([NotNullIfNotNull(nameof(value))] ref T? value) where T : GBX.NET.IClass, new();

    [return: NotNullIfNotNull(nameof(value))]
    T? MetaRef<T>(T? value = default) where T : GBX.NET.IClass;
    void MetaRef<T>([NotNullIfNotNull(nameof(value))] ref T? value) where T : GBX.NET.IClass;

    [return: NotNullIfNotNull(nameof(value))]
    TmEssentials.TimeInt32 TimeInt32(TmEssentials.TimeInt32 value = default);
    [return: NotNullIfNotNull(nameof(value))]
    TmEssentials.TimeInt32? TimeInt32(TmEssentials.TimeInt32? value, TmEssentials.TimeInt32 defaultValue = default);
    void TimeInt32([NotNullIfNotNull(nameof(value))] ref TmEssentials.TimeInt32 value);
    void TimeInt32([NotNullIfNotNull(nameof(value))] ref TmEssentials.TimeInt32? value, TmEssentials.TimeInt32 defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    TmEssentials.TimeInt32? TimeInt32Nullable(TmEssentials.TimeInt32? value = default);
    void TimeInt32Nullable([NotNullIfNotNull(nameof(value))] ref TmEssentials.TimeInt32? value);

    [return: NotNullIfNotNull(nameof(value))]
    TmEssentials.TimeSingle TimeSingle(TmEssentials.TimeSingle value = default);
    [return: NotNullIfNotNull(nameof(value))]
    TmEssentials.TimeSingle? TimeSingle(TmEssentials.TimeSingle? value, TmEssentials.TimeSingle defaultValue = default);
    void TimeSingle([NotNullIfNotNull(nameof(value))] ref TmEssentials.TimeSingle value);
    void TimeSingle([NotNullIfNotNull(nameof(value))] ref TmEssentials.TimeSingle? value, TmEssentials.TimeSingle defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    TmEssentials.TimeSingle? TimeSingleNullable(TmEssentials.TimeSingle? value = default);
    void TimeSingleNullable([NotNullIfNotNull(nameof(value))] ref TmEssentials.TimeSingle? value);

    [return: NotNullIfNotNull(nameof(value))]
    System.TimeSpan? TimeOfDay(System.TimeSpan? value = default);
    void TimeOfDay([NotNullIfNotNull(nameof(value))] ref System.TimeSpan? value);

    [return: NotNullIfNotNull(nameof(value))]
    System.DateTime? FileTime(System.DateTime? value = default);
    void FileTime([NotNullIfNotNull(nameof(value))] ref System.DateTime? value);

    [return: NotNullIfNotNull(nameof(value))]
    System.DateTime? SystemTime(System.DateTime? value = default);
    void SystemTime([NotNullIfNotNull(nameof(value))] ref System.DateTime? value);

    [return: NotNullIfNotNull(nameof(value))]
    System.DateTimeOffset? UnixTime(System.DateTimeOffset? value = default);
    void UnixTime([NotNullIfNotNull(nameof(value))] ref System.DateTimeOffset? value);

    [return: NotNullIfNotNull(nameof(value))]
    System.Net.IPAddress? IPv4(System.Net.IPAddress? value = default);
    void IPv4([NotNullIfNotNull(nameof(value))] ref System.Net.IPAddress? value);

    [return: NotNullIfNotNull(nameof(value))]
    int SmallLen(int value = default);
    [return: NotNullIfNotNull(nameof(value))]
    int? SmallLen(int? value, int defaultValue = default);
    void SmallLen([NotNullIfNotNull(nameof(value))] ref int value);
    void SmallLen([NotNullIfNotNull(nameof(value))] ref int? value, int defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    string? SmallString(string? value = default);
    void SmallString([NotNullIfNotNull(nameof(value))] ref string? value);

    [return: NotNullIfNotNull(nameof(value))]
    int OptimizedInt(int value, int determineFrom);
    [return: NotNullIfNotNull(nameof(value))]
    int? OptimizedInt(int? value, int determineFrom, int defaultValue);
    void OptimizedInt([NotNullIfNotNull(nameof(value))] ref int value, int determineFrom);
    void OptimizedInt([NotNullIfNotNull(nameof(value))] ref int? value, int determineFrom, int defaultValue = default);

    [return: NotNullIfNotNull(nameof(value))]
    short VarNat15(short value = default);
    [return: NotNullIfNotNull(nameof(value))]
    short? VarNat15(short? value, short defaultValue = default);
    void VarNat15([NotNullIfNotNull(nameof(value))] ref short value);
    void VarNat15([NotNullIfNotNull(nameof(value))] ref short? value, short defaultValue = default);

    void DeprecVersion();

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Vec3[]? ArrayVec3_10b(GBX.NET.Vec3[]? value = default);
    void ArrayVec3_10b([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3[]? value);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Vec3[]? ArrayVec3_10b(GBX.NET.Vec3[]? value, int length);
    void ArrayVec3_10b([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3[]? value, int length);

    [return: NotNullIfNotNull(nameof(value))]
    T[]? Array<T>(T[]? value, int length, bool lengthInBytes = false) where T : struct;
    void Array<T>([NotNullIfNotNull(nameof(value))] ref T[]? value, int length, bool lengthInBytes = false) where T : struct;

    [return: NotNullIfNotNull(nameof(value))]
    T[]? Array<T>(T[]? value, bool lengthInBytes = false) where T : struct;
    void Array<T>([NotNullIfNotNull(nameof(value))] ref T[]? value, bool lengthInBytes = false) where T : struct;

    [return: NotNullIfNotNull(nameof(value))]
    T[]? Array_deprec<T>(T[]? value, int length, bool lengthInBytes = false) where T : struct;
    void Array_deprec<T>([NotNullIfNotNull(nameof(value))] ref T[]? value, int length, bool lengthInBytes = false) where T : struct;

    [return: NotNullIfNotNull(nameof(value))]
    T[]? Array_deprec<T>(T[]? value, bool lengthInBytes = false) where T : struct;
    void Array_deprec<T>([NotNullIfNotNull(nameof(value))] ref T[]? value, bool lengthInBytes = false) where T : struct;

    [return: NotNullIfNotNull(nameof(value))]
    T[][]? JaggedArray<T>(T[][]? value, int? innerLength = default, int? outerLength = default) where T : struct;
    void JaggedArray<T>([NotNullIfNotNull(nameof(value))] ref T[][]? value, int? innerLength = default, int? outerLength = default) where T : struct;

    [return: NotNullIfNotNull(nameof(value))]
    System.Collections.Generic.List<T>? List<T>(System.Collections.Generic.List<T>? value, int length, bool lengthInBytes = false) where T : struct;
    void List<T>([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<T>? value, int length, bool lengthInBytes = false) where T : struct;

    [return: NotNullIfNotNull(nameof(value))]
    System.Collections.Generic.List<T>? List<T>(System.Collections.Generic.List<T>? value, bool lengthInBytes = false) where T : struct;
    void List<T>([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<T>? value, bool lengthInBytes = false) where T : struct;

    [return: NotNullIfNotNull(nameof(value))]
    System.Collections.Generic.List<T>? List_deprec<T>(System.Collections.Generic.List<T>? value, bool lengthInBytes = false) where T : struct;
    void List_deprec<T>([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<T>? value, bool lengthInBytes = false) where T : struct;

    [return: NotNullIfNotNull(nameof(value))]
    T?[]? ArrayNode<T>(T?[]? value = default) where T : GBX.NET.IClass, new();
    void ArrayNode<T>([NotNullIfNotNull(nameof(value))] ref T?[]? value) where T : GBX.NET.IClass, new();

    [return: NotNullIfNotNull(nameof(value))]
    T?[]? ArrayNodeRef<T>(T?[]? value, int length) where T : GBX.NET.IClass;
    void ArrayNodeRef<T>([NotNullIfNotNull(nameof(value))] ref T?[]? value, int length) where T : GBX.NET.IClass;

    [return: NotNullIfNotNull(nameof(value))]
    T?[]? ArrayNodeRef<T>(T?[]? value = default) where T : GBX.NET.IClass;
    void ArrayNodeRef<T>([NotNullIfNotNull(nameof(value))] ref T?[]? value) where T : GBX.NET.IClass;

    [return: NotNullIfNotNull(nameof(value))]
    T?[]? ArrayNodeRef_deprec<T>(T?[]? value = default) where T : GBX.NET.IClass;
    void ArrayNodeRef_deprec<T>([NotNullIfNotNull(nameof(value))] ref T?[]? value) where T : GBX.NET.IClass;

    [return: NotNullIfNotNull(nameof(value))]
    T?[][]? JaggedArrayNodeRef<T>(T?[][]? value, int? innerLength = default, int? outerLength = default) where T : GBX.NET.IClass;
    void JaggedArrayNodeRef<T>([NotNullIfNotNull(nameof(value))] ref T?[][]? value, int? innerLength = default, int? outerLength = default) where T : GBX.NET.IClass;

    [return: NotNullIfNotNull(nameof(value))]
    System.Collections.Generic.List<T?>? ListNodeRef<T>(System.Collections.Generic.List<T?>? value, int length) where T : GBX.NET.IClass;
    void ListNodeRef<T>([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<T?>? value, int length) where T : GBX.NET.IClass;

    [return: NotNullIfNotNull(nameof(value))]
    System.Collections.Generic.List<T?>? ListNodeRef<T>(System.Collections.Generic.List<T?>? value = default) where T : GBX.NET.IClass;
    void ListNodeRef<T>([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<T?>? value) where T : GBX.NET.IClass;

    [return: NotNullIfNotNull(nameof(value))]
    System.Collections.Generic.List<T?>? ListNodeRef_deprec<T>(System.Collections.Generic.List<T?>? value = default) where T : GBX.NET.IClass;
    void ListNodeRef_deprec<T>([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<T?>? value) where T : GBX.NET.IClass;

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.External<T>[]? ArrayExternalNodeRef<T>(GBX.NET.External<T>[]? value, int length) where T : GBX.NET.Engines.MwFoundations.CMwNod;
    void ArrayExternalNodeRef<T>([NotNullIfNotNull(nameof(value))] ref GBX.NET.External<T>[]? value, int length) where T : GBX.NET.Engines.MwFoundations.CMwNod;

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.External<T>[]? ArrayExternalNodeRef<T>(GBX.NET.External<T>[]? value = default) where T : GBX.NET.Engines.MwFoundations.CMwNod;
    void ArrayExternalNodeRef<T>([NotNullIfNotNull(nameof(value))] ref GBX.NET.External<T>[]? value) where T : GBX.NET.Engines.MwFoundations.CMwNod;

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.External<T>[]? ArrayExternalNodeRef_deprec<T>(GBX.NET.External<T>[]? value = default) where T : GBX.NET.Engines.MwFoundations.CMwNod;
    void ArrayExternalNodeRef_deprec<T>([NotNullIfNotNull(nameof(value))] ref GBX.NET.External<T>[]? value) where T : GBX.NET.Engines.MwFoundations.CMwNod;

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.External<T>[][]? JaggedArrayExternalNodeRef<T>(GBX.NET.External<T>[][]? value, int? innerLength = default, int? outerLength = default) where T : GBX.NET.Engines.MwFoundations.CMwNod;
    void JaggedArrayExternalNodeRef<T>([NotNullIfNotNull(nameof(value))] ref GBX.NET.External<T>[][]? value, int? innerLength = default, int? outerLength = default) where T : GBX.NET.Engines.MwFoundations.CMwNod;

    [return: NotNullIfNotNull(nameof(value))]
    System.Collections.Generic.List<GBX.NET.External<T>>? ListExternalNodeRef<T>(System.Collections.Generic.List<GBX.NET.External<T>>? value, int length) where T : GBX.NET.Engines.MwFoundations.CMwNod;
    void ListExternalNodeRef<T>([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<GBX.NET.External<T>>? value, int length) where T : GBX.NET.Engines.MwFoundations.CMwNod;

    [return: NotNullIfNotNull(nameof(value))]
    System.Collections.Generic.List<GBX.NET.External<T>>? ListExternalNodeRef<T>(System.Collections.Generic.List<GBX.NET.External<T>>? value = default) where T : GBX.NET.Engines.MwFoundations.CMwNod;
    void ListExternalNodeRef<T>([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<GBX.NET.External<T>>? value) where T : GBX.NET.Engines.MwFoundations.CMwNod;

    [return: NotNullIfNotNull(nameof(value))]
    System.Collections.Generic.List<GBX.NET.External<T>>? ListExternalNodeRef_deprec<T>(System.Collections.Generic.List<GBX.NET.External<T>>? value = default) where T : GBX.NET.Engines.MwFoundations.CMwNod;
    void ListExternalNodeRef_deprec<T>([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<GBX.NET.External<T>>? value) where T : GBX.NET.Engines.MwFoundations.CMwNod;

    [return: NotNullIfNotNull(nameof(value))]
    string[]? ArrayId(string[]? value, int length);
    void ArrayId([NotNullIfNotNull(nameof(value))] ref string[]? value, int length);

    [return: NotNullIfNotNull(nameof(value))]
    string[]? ArrayId(string[]? value = default);
    void ArrayId([NotNullIfNotNull(nameof(value))] ref string[]? value);

    [return: NotNullIfNotNull(nameof(value))]
    string[]? ArrayId_deprec(string[]? value = default);
    void ArrayId_deprec([NotNullIfNotNull(nameof(value))] ref string[]? value);

    [return: NotNullIfNotNull(nameof(value))]
    string[][]? JaggedArrayId(string[][]? value, int? innerLength = default, int? outerLength = default);
    void JaggedArrayId([NotNullIfNotNull(nameof(value))] ref string[][]? value, int? innerLength = default, int? outerLength = default);

    [return: NotNullIfNotNull(nameof(value))]
    string[][]? JaggedArrayString(string[][]? value, int? innerLength = default, int? outerLength = default);
    void JaggedArrayString([NotNullIfNotNull(nameof(value))] ref string[][]? value, int? innerLength = default, int? outerLength = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.Ident[][]? JaggedArrayIdent(GBX.NET.Ident[][]? value, int? innerLength = default, int? outerLength = default);
    void JaggedArrayIdent([NotNullIfNotNull(nameof(value))] ref GBX.NET.Ident[][]? value, int? innerLength = default, int? outerLength = default);

    [return: NotNullIfNotNull(nameof(value))]
    GBX.NET.PackDesc[][]? JaggedArrayPackDesc(GBX.NET.PackDesc[][]? value, int? innerLength = default, int? outerLength = default);
    void JaggedArrayPackDesc([NotNullIfNotNull(nameof(value))] ref GBX.NET.PackDesc[][]? value, int? innerLength = default, int? outerLength = default);

    [return: NotNullIfNotNull(nameof(value))]
    System.Collections.Generic.List<string>? ListId(System.Collections.Generic.List<string>? value, int length);
    void ListId([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<string>? value, int length);

    [return: NotNullIfNotNull(nameof(value))]
    System.Collections.Generic.List<string>? ListId(System.Collections.Generic.List<string>? value = default);
    void ListId([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<string>? value);

    [return: NotNullIfNotNull(nameof(value))]
    System.Collections.Generic.List<string>? ListId_deprec(System.Collections.Generic.List<string>? value = default);
    void ListId_deprec([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<string>? value);

}

partial class GbxReaderWriter
{
    public bool GbxMagic()
    {
        bool value = default;
        if (Reader is not null) value = Reader.ReadGbxMagic();
        Writer?.WriteGbxMagic();
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public byte Byte(byte value = default)
    {
        if (Reader is not null) value = Reader.ReadByte();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public byte? Byte(byte? value, byte defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadByte();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Byte([NotNullIfNotNull(nameof(value))] ref byte value) => value = Byte(value);

    public void Byte([NotNullIfNotNull(nameof(value))] ref byte? value, byte defaultValue = default) => value = Byte(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public sbyte SByte(sbyte value = default)
    {
        if (Reader is not null) value = Reader.ReadSByte();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public sbyte? SByte(sbyte? value, sbyte defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadSByte();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void SByte([NotNullIfNotNull(nameof(value))] ref sbyte value) => value = SByte(value);

    public void SByte([NotNullIfNotNull(nameof(value))] ref sbyte? value, sbyte defaultValue = default) => value = SByte(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public short Int16(short value = default)
    {
        if (Reader is not null) value = Reader.ReadInt16();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public short? Int16(short? value, short defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadInt16();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Int16([NotNullIfNotNull(nameof(value))] ref short value) => value = Int16(value);

    public void Int16([NotNullIfNotNull(nameof(value))] ref short? value, short defaultValue = default) => value = Int16(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public ushort UInt16(ushort value = default)
    {
        if (Reader is not null) value = Reader.ReadUInt16();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public ushort? UInt16(ushort? value, ushort defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadUInt16();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void UInt16([NotNullIfNotNull(nameof(value))] ref ushort value) => value = UInt16(value);

    public void UInt16([NotNullIfNotNull(nameof(value))] ref ushort? value, ushort defaultValue = default) => value = UInt16(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public int Int32(int value = default)
    {
        if (Reader is not null) value = Reader.ReadInt32();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public int? Int32(int? value, int defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadInt32();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Int32([NotNullIfNotNull(nameof(value))] ref int value) => value = Int32(value);

    public void Int32([NotNullIfNotNull(nameof(value))] ref int? value, int defaultValue = default) => value = Int32(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public uint UInt32(uint value = default)
    {
        if (Reader is not null) value = Reader.ReadUInt32();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public uint? UInt32(uint? value, uint defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadUInt32();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void UInt32([NotNullIfNotNull(nameof(value))] ref uint value) => value = UInt32(value);

    public void UInt32([NotNullIfNotNull(nameof(value))] ref uint? value, uint defaultValue = default) => value = UInt32(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public long Int64(long value = default)
    {
        if (Reader is not null) value = Reader.ReadInt64();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public long? Int64(long? value, long defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadInt64();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Int64([NotNullIfNotNull(nameof(value))] ref long value) => value = Int64(value);

    public void Int64([NotNullIfNotNull(nameof(value))] ref long? value, long defaultValue = default) => value = Int64(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public ulong UInt64(ulong value = default)
    {
        if (Reader is not null) value = Reader.ReadUInt64();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public ulong? UInt64(ulong? value, ulong defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadUInt64();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void UInt64([NotNullIfNotNull(nameof(value))] ref ulong value) => value = UInt64(value);

    public void UInt64([NotNullIfNotNull(nameof(value))] ref ulong? value, ulong defaultValue = default) => value = UInt64(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public float Single(float value = default)
    {
        if (Reader is not null) value = Reader.ReadSingle();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public float? Single(float? value, float defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadSingle();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Single([NotNullIfNotNull(nameof(value))] ref float value) => value = Single(value);

    public void Single([NotNullIfNotNull(nameof(value))] ref float? value, float defaultValue = default) => value = Single(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public int HexInt32(int value = default)
    {
        if (Reader is not null) value = Reader.ReadHexInt32();
        Writer?.WriteHexInt32(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public int? HexInt32(int? value, int defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadHexInt32();
        Writer?.WriteHexInt32(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void HexInt32([NotNullIfNotNull(nameof(value))] ref int value) => value = HexInt32(value);

    public void HexInt32([NotNullIfNotNull(nameof(value))] ref int? value, int defaultValue = default) => value = HexInt32(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public uint HexUInt32(uint value = default)
    {
        if (Reader is not null) value = Reader.ReadHexUInt32();
        Writer?.WriteHexUInt32(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public uint? HexUInt32(uint? value, uint defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadHexUInt32();
        Writer?.WriteHexUInt32(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void HexUInt32([NotNullIfNotNull(nameof(value))] ref uint value) => value = HexUInt32(value);

    public void HexUInt32([NotNullIfNotNull(nameof(value))] ref uint? value, uint defaultValue = default) => value = HexUInt32(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public int DataInt32(int value = default)
    {
        if (Reader is not null) value = Reader.ReadDataInt32();
        Writer?.WriteDataInt32(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public int? DataInt32(int? value, int defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadDataInt32();
        Writer?.WriteDataInt32(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void DataInt32([NotNullIfNotNull(nameof(value))] ref int value) => value = DataInt32(value);

    public void DataInt32([NotNullIfNotNull(nameof(value))] ref int? value, int defaultValue = default) => value = DataInt32(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public uint DataUInt32(uint value = default)
    {
        if (Reader is not null) value = Reader.ReadDataUInt32();
        Writer?.WriteDataUInt32(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public uint? DataUInt32(uint? value, uint defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadDataUInt32();
        Writer?.WriteDataUInt32(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void DataUInt32([NotNullIfNotNull(nameof(value))] ref uint value) => value = DataUInt32(value);

    public void DataUInt32([NotNullIfNotNull(nameof(value))] ref uint? value, uint defaultValue = default) => value = DataUInt32(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public long DataInt64(long value = default)
    {
        if (Reader is not null) value = Reader.ReadDataInt64();
        Writer?.WriteDataInt64(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public long? DataInt64(long? value, long defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadDataInt64();
        Writer?.WriteDataInt64(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void DataInt64([NotNullIfNotNull(nameof(value))] ref long value) => value = DataInt64(value);

    public void DataInt64([NotNullIfNotNull(nameof(value))] ref long? value, long defaultValue = default) => value = DataInt64(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public ulong DataUInt64(ulong value = default)
    {
        if (Reader is not null) value = Reader.ReadDataUInt64();
        Writer?.WriteDataUInt64(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public ulong? DataUInt64(ulong? value, ulong defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadDataUInt64();
        Writer?.WriteDataUInt64(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void DataUInt64([NotNullIfNotNull(nameof(value))] ref ulong value) => value = DataUInt64(value);

    public void DataUInt64([NotNullIfNotNull(nameof(value))] ref ulong? value, ulong defaultValue = default) => value = DataUInt64(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public System.Numerics.BigInteger BigInt(System.Numerics.BigInteger value, int byteLength)
    {
        if (Reader is not null) value = Reader.ReadBigInt(byteLength);
        Writer?.WriteBigInt(value, byteLength);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public System.Numerics.BigInteger? BigInt(System.Numerics.BigInteger? value, int byteLength, System.Numerics.BigInteger defaultValue)
    {
        if (Reader is not null) value = Reader.ReadBigInt(byteLength);
        Writer?.WriteBigInt(value.GetValueOrDefault(defaultValue), byteLength);
        return value;
    }

    public void BigInt([NotNullIfNotNull(nameof(value))] ref System.Numerics.BigInteger value, int byteLength) => value = BigInt(value, byteLength);

    public void BigInt([NotNullIfNotNull(nameof(value))] ref System.Numerics.BigInteger? value, int byteLength, System.Numerics.BigInteger defaultValue = default) => value = BigInt(value, byteLength, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public System.Int128 Int128(System.Int128 value = default)
    {
        if (Reader is not null) value = Reader.ReadInt128();
        Writer?.WriteInt128(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public System.Int128? Int128(System.Int128? value, System.Int128 defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadInt128();
        Writer?.WriteInt128(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Int128([NotNullIfNotNull(nameof(value))] ref System.Int128 value) => value = Int128(value);

    public void Int128([NotNullIfNotNull(nameof(value))] ref System.Int128? value, System.Int128 defaultValue = default) => value = Int128(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public System.UInt128 UInt128(System.UInt128 value = default)
    {
        if (Reader is not null) value = Reader.ReadUInt128();
        Writer?.WriteUInt128(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public System.UInt128? UInt128(System.UInt128? value, System.UInt128 defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadUInt128();
        Writer?.WriteUInt128(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void UInt128([NotNullIfNotNull(nameof(value))] ref System.UInt128 value) => value = UInt128(value);

    public void UInt128([NotNullIfNotNull(nameof(value))] ref System.UInt128? value, System.UInt128 defaultValue = default) => value = UInt128(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.UInt256 UInt256(GBX.NET.UInt256 value = default)
    {
        if (Reader is not null) value = Reader.ReadUInt256();
        Writer?.WriteUInt256(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.UInt256? UInt256(GBX.NET.UInt256? value, GBX.NET.UInt256 defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadUInt256();
        Writer?.WriteUInt256(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void UInt256([NotNullIfNotNull(nameof(value))] ref GBX.NET.UInt256 value) => value = UInt256(value);

    public void UInt256([NotNullIfNotNull(nameof(value))] ref GBX.NET.UInt256? value, GBX.NET.UInt256 defaultValue = default) => value = UInt256(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Checksum128 Checksum128(GBX.NET.Checksum128 value = default)
    {
        if (Reader is not null) value = Reader.ReadChecksum128();
        Writer?.WriteChecksum128(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Checksum128? Checksum128(GBX.NET.Checksum128? value, GBX.NET.Checksum128 defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadChecksum128();
        Writer?.WriteChecksum128(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Checksum128([NotNullIfNotNull(nameof(value))] ref GBX.NET.Checksum128 value) => value = Checksum128(value);

    public void Checksum128([NotNullIfNotNull(nameof(value))] ref GBX.NET.Checksum128? value, GBX.NET.Checksum128 defaultValue = default) => value = Checksum128(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Checksum256 Checksum256(GBX.NET.Checksum256 value = default)
    {
        if (Reader is not null) value = Reader.ReadChecksum256();
        Writer?.WriteChecksum256(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Checksum256? Checksum256(GBX.NET.Checksum256? value, GBX.NET.Checksum256 defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadChecksum256();
        Writer?.WriteChecksum256(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Checksum256([NotNullIfNotNull(nameof(value))] ref GBX.NET.Checksum256 value) => value = Checksum256(value);

    public void Checksum256([NotNullIfNotNull(nameof(value))] ref GBX.NET.Checksum256? value, GBX.NET.Checksum256 defaultValue = default) => value = Checksum256(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Int2 Int2(GBX.NET.Int2 value = default)
    {
        if (Reader is not null) value = Reader.ReadInt2();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Int2? Int2(GBX.NET.Int2? value, GBX.NET.Int2 defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadInt2();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Int2([NotNullIfNotNull(nameof(value))] ref GBX.NET.Int2 value) => value = Int2(value);

    public void Int2([NotNullIfNotNull(nameof(value))] ref GBX.NET.Int2? value, GBX.NET.Int2 defaultValue = default) => value = Int2(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Int3 Int3(GBX.NET.Int3 value = default)
    {
        if (Reader is not null) value = Reader.ReadInt3();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Int3? Int3(GBX.NET.Int3? value, GBX.NET.Int3 defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadInt3();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Int3([NotNullIfNotNull(nameof(value))] ref GBX.NET.Int3 value) => value = Int3(value);

    public void Int3([NotNullIfNotNull(nameof(value))] ref GBX.NET.Int3? value, GBX.NET.Int3 defaultValue = default) => value = Int3(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Int4 Int4(GBX.NET.Int4 value = default)
    {
        if (Reader is not null) value = Reader.ReadInt4();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Int4? Int4(GBX.NET.Int4? value, GBX.NET.Int4 defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadInt4();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Int4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Int4 value) => value = Int4(value);

    public void Int4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Int4? value, GBX.NET.Int4 defaultValue = default) => value = Int4(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Byte3 Byte3(GBX.NET.Byte3 value = default)
    {
        if (Reader is not null) value = Reader.ReadByte3();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Byte3? Byte3(GBX.NET.Byte3? value, GBX.NET.Byte3 defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadByte3();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Byte3([NotNullIfNotNull(nameof(value))] ref GBX.NET.Byte3 value) => value = Byte3(value);

    public void Byte3([NotNullIfNotNull(nameof(value))] ref GBX.NET.Byte3? value, GBX.NET.Byte3 defaultValue = default) => value = Byte3(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Vec2 Vec2(GBX.NET.Vec2 value = default)
    {
        if (Reader is not null) value = Reader.ReadVec2();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Vec2? Vec2(GBX.NET.Vec2? value, GBX.NET.Vec2 defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadVec2();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Vec2([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec2 value) => value = Vec2(value);

    public void Vec2([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec2? value, GBX.NET.Vec2 defaultValue = default) => value = Vec2(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Vec3 Vec3(GBX.NET.Vec3 value = default)
    {
        if (Reader is not null) value = Reader.ReadVec3();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Vec3? Vec3(GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadVec3();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Vec3([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3 value) => value = Vec3(value);

    public void Vec3([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default) => value = Vec3(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Vec3 Vec3_10b(GBX.NET.Vec3 value = default)
    {
        if (Reader is not null) value = Reader.ReadVec3_10b();
        Writer?.WriteVec3_10b(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Vec3? Vec3_10b(GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadVec3_10b();
        Writer?.WriteVec3_10b(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Vec3_10b([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3 value) => value = Vec3_10b(value);

    public void Vec3_10b([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default) => value = Vec3_10b(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Vec3 Vec3Unit2(GBX.NET.Vec3 value = default)
    {
        if (Reader is not null) value = Reader.ReadVec3Unit2();
        Writer?.WriteVec3Unit2(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Vec3? Vec3Unit2(GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadVec3Unit2();
        Writer?.WriteVec3Unit2(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Vec3Unit2([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3 value) => value = Vec3Unit2(value);

    public void Vec3Unit2([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default) => value = Vec3Unit2(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Vec3 Vec3_4(GBX.NET.Vec3 value = default)
    {
        if (Reader is not null) value = Reader.ReadVec3_4();
        Writer?.WriteVec3_4(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Vec3? Vec3_4(GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadVec3_4();
        Writer?.WriteVec3_4(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Vec3_4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3 value) => value = Vec3_4(value);

    public void Vec3_4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default) => value = Vec3_4(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Vec3 Vec3_6(GBX.NET.Vec3 value = default)
    {
        if (Reader is not null) value = Reader.ReadVec3_6();
        Writer?.WriteVec3_6(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Vec3? Vec3_6(GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadVec3_6();
        Writer?.WriteVec3_6(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Vec3_6([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3 value) => value = Vec3_6(value);

    public void Vec3_6([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default) => value = Vec3_6(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Vec3 Vec3Unit4(GBX.NET.Vec3 value = default)
    {
        if (Reader is not null) value = Reader.ReadVec3Unit4();
        Writer?.WriteVec3Unit4(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Vec3? Vec3Unit4(GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadVec3Unit4();
        Writer?.WriteVec3Unit4(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Vec3Unit4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3 value) => value = Vec3Unit4(value);

    public void Vec3Unit4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3? value, GBX.NET.Vec3 defaultValue = default) => value = Vec3Unit4(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Vec4 Vec4(GBX.NET.Vec4 value = default)
    {
        if (Reader is not null) value = Reader.ReadVec4();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Vec4? Vec4(GBX.NET.Vec4? value, GBX.NET.Vec4 defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadVec4();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Vec4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec4 value) => value = Vec4(value);

    public void Vec4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec4? value, GBX.NET.Vec4 defaultValue = default) => value = Vec4(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.BoxAligned BoxAligned(GBX.NET.BoxAligned value = default)
    {
        if (Reader is not null) value = Reader.ReadBoxAligned();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.BoxAligned? BoxAligned(GBX.NET.BoxAligned? value, GBX.NET.BoxAligned defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadBoxAligned();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void BoxAligned([NotNullIfNotNull(nameof(value))] ref GBX.NET.BoxAligned value) => value = BoxAligned(value);

    public void BoxAligned([NotNullIfNotNull(nameof(value))] ref GBX.NET.BoxAligned? value, GBX.NET.BoxAligned defaultValue = default) => value = BoxAligned(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.BoxInt3 BoxInt3(GBX.NET.BoxInt3 value = default)
    {
        if (Reader is not null) value = Reader.ReadBoxInt3();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.BoxInt3? BoxInt3(GBX.NET.BoxInt3? value, GBX.NET.BoxInt3 defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadBoxInt3();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void BoxInt3([NotNullIfNotNull(nameof(value))] ref GBX.NET.BoxInt3 value) => value = BoxInt3(value);

    public void BoxInt3([NotNullIfNotNull(nameof(value))] ref GBX.NET.BoxInt3? value, GBX.NET.BoxInt3 defaultValue = default) => value = BoxInt3(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Color Color(GBX.NET.Color value = default)
    {
        if (Reader is not null) value = Reader.ReadColor();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Color? Color(GBX.NET.Color? value, GBX.NET.Color defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadColor();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Color([NotNullIfNotNull(nameof(value))] ref GBX.NET.Color value) => value = Color(value);

    public void Color([NotNullIfNotNull(nameof(value))] ref GBX.NET.Color? value, GBX.NET.Color defaultValue = default) => value = Color(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Iso4 Iso4(GBX.NET.Iso4 value = default)
    {
        if (Reader is not null) value = Reader.ReadIso4();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Iso4? Iso4(GBX.NET.Iso4? value, GBX.NET.Iso4 defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadIso4();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Iso4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Iso4 value) => value = Iso4(value);

    public void Iso4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Iso4? value, GBX.NET.Iso4 defaultValue = default) => value = Iso4(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Mat3 Mat3(GBX.NET.Mat3 value = default)
    {
        if (Reader is not null) value = Reader.ReadMat3();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Mat3? Mat3(GBX.NET.Mat3? value, GBX.NET.Mat3 defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadMat3();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Mat3([NotNullIfNotNull(nameof(value))] ref GBX.NET.Mat3 value) => value = Mat3(value);

    public void Mat3([NotNullIfNotNull(nameof(value))] ref GBX.NET.Mat3? value, GBX.NET.Mat3 defaultValue = default) => value = Mat3(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Mat4 Mat4(GBX.NET.Mat4 value = default)
    {
        if (Reader is not null) value = Reader.ReadMat4();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Mat4? Mat4(GBX.NET.Mat4? value, GBX.NET.Mat4 defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadMat4();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Mat4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Mat4 value) => value = Mat4(value);

    public void Mat4([NotNullIfNotNull(nameof(value))] ref GBX.NET.Mat4? value, GBX.NET.Mat4 defaultValue = default) => value = Mat4(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Quat Quat(GBX.NET.Quat value = default)
    {
        if (Reader is not null) value = Reader.ReadQuat();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Quat? Quat(GBX.NET.Quat? value, GBX.NET.Quat defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadQuat();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Quat([NotNullIfNotNull(nameof(value))] ref GBX.NET.Quat value) => value = Quat(value);

    public void Quat([NotNullIfNotNull(nameof(value))] ref GBX.NET.Quat? value, GBX.NET.Quat defaultValue = default) => value = Quat(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Quat Quat6(GBX.NET.Quat value = default)
    {
        if (Reader is not null) value = Reader.ReadQuat6();
        Writer?.WriteQuat6(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Quat? Quat6(GBX.NET.Quat? value, GBX.NET.Quat defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadQuat6();
        Writer?.WriteQuat6(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Quat6([NotNullIfNotNull(nameof(value))] ref GBX.NET.Quat value) => value = Quat6(value);

    public void Quat6([NotNullIfNotNull(nameof(value))] ref GBX.NET.Quat? value, GBX.NET.Quat defaultValue = default) => value = Quat6(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Rect Rect(GBX.NET.Rect value = default)
    {
        if (Reader is not null) value = Reader.ReadRect();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Rect? Rect(GBX.NET.Rect? value, GBX.NET.Rect defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadRect();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Rect([NotNullIfNotNull(nameof(value))] ref GBX.NET.Rect value) => value = Rect(value);

    public void Rect([NotNullIfNotNull(nameof(value))] ref GBX.NET.Rect? value, GBX.NET.Rect defaultValue = default) => value = Rect(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.TransQuat TransQuat(GBX.NET.TransQuat value = default)
    {
        if (Reader is not null) value = Reader.ReadTransQuat();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.TransQuat? TransQuat(GBX.NET.TransQuat? value, GBX.NET.TransQuat defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadTransQuat();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void TransQuat([NotNullIfNotNull(nameof(value))] ref GBX.NET.TransQuat value) => value = TransQuat(value);

    public void TransQuat([NotNullIfNotNull(nameof(value))] ref GBX.NET.TransQuat? value, GBX.NET.TransQuat defaultValue = default) => value = TransQuat(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public bool Boolean(bool value = default)
    {
        if (Reader is not null) value = Reader.ReadBoolean();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public bool? Boolean(bool? value, bool defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadBoolean();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Boolean([NotNullIfNotNull(nameof(value))] ref bool value) => value = Boolean(value);

    public void Boolean([NotNullIfNotNull(nameof(value))] ref bool? value, bool defaultValue = default) => value = Boolean(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public bool Boolean(bool value, bool asByte)
    {
        if (Reader is not null) value = Reader.ReadBoolean(asByte);
        Writer?.Write(value, asByte);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public bool? Boolean(bool? value, bool asByte, bool defaultValue)
    {
        if (Reader is not null) value = Reader.ReadBoolean(asByte);
        Writer?.Write(value.GetValueOrDefault(defaultValue), asByte);
        return value;
    }

    public void Boolean([NotNullIfNotNull(nameof(value))] ref bool value, bool asByte) => value = Boolean(value, asByte);

    public void Boolean([NotNullIfNotNull(nameof(value))] ref bool? value, bool asByte, bool defaultValue = default) => value = Boolean(value, asByte, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public bool Boolean(bool value, GBX.NET.Serialization.BoolType type)
    {
        if (Reader is not null) value = Reader.ReadBoolean(type);
        Writer?.Write(value, type);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public bool? Boolean(bool? value, GBX.NET.Serialization.BoolType type, bool defaultValue)
    {
        if (Reader is not null) value = Reader.ReadBoolean(type);
        Writer?.Write(value.GetValueOrDefault(defaultValue), type);
        return value;
    }

    public void Boolean([NotNullIfNotNull(nameof(value))] ref bool value, GBX.NET.Serialization.BoolType type) => value = Boolean(value, type);

    public void Boolean([NotNullIfNotNull(nameof(value))] ref bool? value, GBX.NET.Serialization.BoolType type, bool defaultValue = default) => value = Boolean(value, type, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public byte[]? Data(byte[]? value = default)
    {
        if (Reader is not null) value = Reader.ReadData();
        Writer?.WriteData(value);
        return value;
    }

    public void Data([NotNullIfNotNull(nameof(value))] ref byte[]? value) => value = Data(value);

    [return: NotNullIfNotNull(nameof(value))]
    public async System.Threading.Tasks.Task<byte[]> DataAsync(byte[]? value, System.Threading.CancellationToken cancellationToken = default)
    {
        if (Reader is not null) value = await Reader.ReadDataAsync(cancellationToken);
        if (Writer is not null) await Writer.WriteDataAsync(value, cancellationToken);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public byte[]? Data(byte[]? value, int length)
    {
        if (Reader is not null) value = Reader.ReadData(length);
        Writer?.WriteData(value, length);
        return value;
    }

    public void Data([NotNullIfNotNull(nameof(value))] ref byte[]? value, int length) => value = Data(value, length);

    [return: NotNullIfNotNull(nameof(value))]
    public string? String(string? value = default)
    {
        if (Reader is not null) value = Reader.ReadString();
        Writer?.Write(value);
        return value;
    }

    public void String([NotNullIfNotNull(nameof(value))] ref string? value) => value = String(value);

    [return: NotNullIfNotNull(nameof(value))]
    public string[]? ArrayString(string[]? value = default)
    {
        if (Reader is not null) value = Reader.ReadArrayString();
        Writer?.WriteArray(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public string[]? ArrayString(string[]? value, int length)
    {
        if (Reader is not null) value = Reader.ReadArrayString(length);
        Writer?.WriteArray(value, length);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public string[]? ArrayString_deprec(string[]? value = default)
    {
        if (Reader is not null) value = Reader.ReadArrayString_deprec();
        Writer?.WriteArray_deprec(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public List<string>? ListString(List<string>? value = default)
    {
        if (Reader is not null) value = Reader.ReadListString();
        Writer?.WriteList(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public List<string>? ListString(List<string>? value, int length)
    {
        if (Reader is not null) value = Reader.ReadListString(length);
        Writer?.WriteList(value, length);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public List<string>? ListString_deprec(List<string>? value = default)
    {
        if (Reader is not null) value = Reader.ReadListString_deprec();
        Writer?.WriteList_deprec(value);
        return value;
    }

    public void ArrayString([NotNullIfNotNull(nameof(value))] ref string[]? value) => value = ArrayString(value);

    public void ArrayString([NotNullIfNotNull(nameof(value))] ref string[]? value, int length) => value = ArrayString(value, length);

    public void ArrayString_deprec([NotNullIfNotNull(nameof(value))] ref string[]? value) => value = ArrayString_deprec(value);

    public void ListString([NotNullIfNotNull(nameof(value))] ref List<string>? value) => value = ListString(value);

    public void ListString([NotNullIfNotNull(nameof(value))] ref List<string>? value, int length) => value = ListString(value, length);

    public void ListString_deprec([NotNullIfNotNull(nameof(value))] ref List<string>? value) => value = ListString_deprec(value);

    [return: NotNullIfNotNull(nameof(value))]
    public string? String(string? value, GBX.NET.Serialization.StringLengthPrefix lengthPrefix)
    {
        if (Reader is not null) value = Reader.ReadString(lengthPrefix);
        Writer?.Write(value, lengthPrefix);
        return value;
    }

    public void String([NotNullIfNotNull(nameof(value))] ref string? value, GBX.NET.Serialization.StringLengthPrefix lengthPrefix) => value = String(value, lengthPrefix);

    [return: NotNullIfNotNull(nameof(value))]
    public string? IdAsString(string? value = default)
    {
        if (Reader is not null) value = Reader.ReadIdAsString();
        Writer?.WriteIdAsString(value);
        return value;
    }

    public void IdAsString([NotNullIfNotNull(nameof(value))] ref string? value) => value = IdAsString(value);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Id Id(GBX.NET.Id value = default)
    {
        if (Reader is not null) value = Reader.ReadId();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Id? Id(GBX.NET.Id? value, GBX.NET.Id defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadId();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void Id([NotNullIfNotNull(nameof(value))] ref GBX.NET.Id value) => value = Id(value);

    public void Id([NotNullIfNotNull(nameof(value))] ref GBX.NET.Id? value, GBX.NET.Id defaultValue = default) => value = Id(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Ident? Ident(GBX.NET.Ident? value = default)
    {
        if (Reader is not null) value = Reader.ReadIdent();
        Writer?.Write(value);
        return value;
    }

    public void Ident([NotNullIfNotNull(nameof(value))] ref GBX.NET.Ident? value) => value = Ident(value);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Ident[]? ArrayIdent(GBX.NET.Ident[]? value = default)
    {
        if (Reader is not null) value = Reader.ReadArrayIdent();
        Writer?.WriteArray(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Ident[]? ArrayIdent(GBX.NET.Ident[]? value, int length)
    {
        if (Reader is not null) value = Reader.ReadArrayIdent(length);
        Writer?.WriteArray(value, length);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Ident[]? ArrayIdent_deprec(GBX.NET.Ident[]? value = default)
    {
        if (Reader is not null) value = Reader.ReadArrayIdent_deprec();
        Writer?.WriteArray_deprec(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public List<GBX.NET.Ident>? ListIdent(List<GBX.NET.Ident>? value = default)
    {
        if (Reader is not null) value = Reader.ReadListIdent();
        Writer?.WriteList(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public List<GBX.NET.Ident>? ListIdent(List<GBX.NET.Ident>? value, int length)
    {
        if (Reader is not null) value = Reader.ReadListIdent(length);
        Writer?.WriteList(value, length);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public List<GBX.NET.Ident>? ListIdent_deprec(List<GBX.NET.Ident>? value = default)
    {
        if (Reader is not null) value = Reader.ReadListIdent_deprec();
        Writer?.WriteList_deprec(value);
        return value;
    }

    public void ArrayIdent([NotNullIfNotNull(nameof(value))] ref GBX.NET.Ident[]? value) => value = ArrayIdent(value);

    public void ArrayIdent([NotNullIfNotNull(nameof(value))] ref GBX.NET.Ident[]? value, int length) => value = ArrayIdent(value, length);

    public void ArrayIdent_deprec([NotNullIfNotNull(nameof(value))] ref GBX.NET.Ident[]? value) => value = ArrayIdent_deprec(value);

    public void ListIdent([NotNullIfNotNull(nameof(value))] ref List<GBX.NET.Ident>? value) => value = ListIdent(value);

    public void ListIdent([NotNullIfNotNull(nameof(value))] ref List<GBX.NET.Ident>? value, int length) => value = ListIdent(value, length);

    public void ListIdent_deprec([NotNullIfNotNull(nameof(value))] ref List<GBX.NET.Ident>? value) => value = ListIdent_deprec(value);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.PackDesc? PackDesc(GBX.NET.PackDesc? value = default)
    {
        if (Reader is not null) value = Reader.ReadPackDesc();
        Writer?.Write(value);
        return value;
    }

    public void PackDesc([NotNullIfNotNull(nameof(value))] ref GBX.NET.PackDesc? value) => value = PackDesc(value);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.PackDesc[]? ArrayPackDesc(GBX.NET.PackDesc[]? value = default)
    {
        if (Reader is not null) value = Reader.ReadArrayPackDesc();
        Writer?.WriteArray(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.PackDesc[]? ArrayPackDesc(GBX.NET.PackDesc[]? value, int length)
    {
        if (Reader is not null) value = Reader.ReadArrayPackDesc(length);
        Writer?.WriteArray(value, length);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.PackDesc[]? ArrayPackDesc_deprec(GBX.NET.PackDesc[]? value = default)
    {
        if (Reader is not null) value = Reader.ReadArrayPackDesc_deprec();
        Writer?.WriteArray_deprec(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public List<GBX.NET.PackDesc>? ListPackDesc(List<GBX.NET.PackDesc>? value = default)
    {
        if (Reader is not null) value = Reader.ReadListPackDesc();
        Writer?.WriteList(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public List<GBX.NET.PackDesc>? ListPackDesc(List<GBX.NET.PackDesc>? value, int length)
    {
        if (Reader is not null) value = Reader.ReadListPackDesc(length);
        Writer?.WriteList(value, length);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public List<GBX.NET.PackDesc>? ListPackDesc_deprec(List<GBX.NET.PackDesc>? value = default)
    {
        if (Reader is not null) value = Reader.ReadListPackDesc_deprec();
        Writer?.WriteList_deprec(value);
        return value;
    }

    public void ArrayPackDesc([NotNullIfNotNull(nameof(value))] ref GBX.NET.PackDesc[]? value) => value = ArrayPackDesc(value);

    public void ArrayPackDesc([NotNullIfNotNull(nameof(value))] ref GBX.NET.PackDesc[]? value, int length) => value = ArrayPackDesc(value, length);

    public void ArrayPackDesc_deprec([NotNullIfNotNull(nameof(value))] ref GBX.NET.PackDesc[]? value) => value = ArrayPackDesc_deprec(value);

    public void ListPackDesc([NotNullIfNotNull(nameof(value))] ref List<GBX.NET.PackDesc>? value) => value = ListPackDesc(value);

    public void ListPackDesc([NotNullIfNotNull(nameof(value))] ref List<GBX.NET.PackDesc>? value, int length) => value = ListPackDesc(value, length);

    public void ListPackDesc_deprec([NotNullIfNotNull(nameof(value))] ref List<GBX.NET.PackDesc>? value) => value = ListPackDesc_deprec(value);

    [return: NotNullIfNotNull(nameof(value))]
    public T? NodeRef<T>(T? value = default) where T : GBX.NET.IClass
    {
        if (Reader is not null) value = Reader.ReadNodeRef<T>();
        Writer?.WriteNodeRef(value);
        return value;
    }

    public void NodeRef<T>([NotNullIfNotNull(nameof(value))] ref T? value) where T : GBX.NET.IClass => value = NodeRef(value);

    [return: NotNullIfNotNull(nameof(value))]
    public T? NodeRef<T>(T? value, ref GBX.NET.Components.GbxRefTableFile? file) where T : GBX.NET.IClass
    {
        if (Reader is not null) value = Reader.ReadNodeRef<T>(out file);
        Writer?.WriteNodeRef(value, file);
        return value;
    }

    public void NodeRef<T>([NotNullIfNotNull(nameof(value))] ref T? value, ref GBX.NET.Components.GbxRefTableFile? file) where T : GBX.NET.IClass => value = NodeRef(value, ref file);

    [return: NotNullIfNotNull(nameof(value))]
    public T? Node<T>(T? value = default) where T : GBX.NET.IClass, new()
    {
        if (Reader is not null) value = Reader.ReadNode<T>();
        Writer?.WriteNode(value);
        return value;
    }

    public void Node<T>([NotNullIfNotNull(nameof(value))] ref T? value) where T : GBX.NET.IClass, new() => value = Node(value);

    [return: NotNullIfNotNull(nameof(value))]
    public T? MetaRef<T>(T? value = default) where T : GBX.NET.IClass
    {
        if (Reader is not null) value = Reader.ReadMetaRef<T>();
        Writer?.WriteMetaRef(value);
        return value;
    }

    public void MetaRef<T>([NotNullIfNotNull(nameof(value))] ref T? value) where T : GBX.NET.IClass => value = MetaRef(value);

    [return: NotNullIfNotNull(nameof(value))]
    public TmEssentials.TimeInt32 TimeInt32(TmEssentials.TimeInt32 value = default)
    {
        if (Reader is not null) value = Reader.ReadTimeInt32();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public TmEssentials.TimeInt32? TimeInt32(TmEssentials.TimeInt32? value, TmEssentials.TimeInt32 defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadTimeInt32();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void TimeInt32([NotNullIfNotNull(nameof(value))] ref TmEssentials.TimeInt32 value) => value = TimeInt32(value);

    public void TimeInt32([NotNullIfNotNull(nameof(value))] ref TmEssentials.TimeInt32? value, TmEssentials.TimeInt32 defaultValue = default) => value = TimeInt32(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public TmEssentials.TimeInt32? TimeInt32Nullable(TmEssentials.TimeInt32? value = default)
    {
        if (Reader is not null) value = Reader.ReadTimeInt32Nullable();
        Writer?.WriteTimeInt32Nullable(value);
        return value;
    }

    public void TimeInt32Nullable([NotNullIfNotNull(nameof(value))] ref TmEssentials.TimeInt32? value) => value = TimeInt32Nullable(value);

    [return: NotNullIfNotNull(nameof(value))]
    public TmEssentials.TimeSingle TimeSingle(TmEssentials.TimeSingle value = default)
    {
        if (Reader is not null) value = Reader.ReadTimeSingle();
        Writer?.Write(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public TmEssentials.TimeSingle? TimeSingle(TmEssentials.TimeSingle? value, TmEssentials.TimeSingle defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadTimeSingle();
        Writer?.Write(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void TimeSingle([NotNullIfNotNull(nameof(value))] ref TmEssentials.TimeSingle value) => value = TimeSingle(value);

    public void TimeSingle([NotNullIfNotNull(nameof(value))] ref TmEssentials.TimeSingle? value, TmEssentials.TimeSingle defaultValue = default) => value = TimeSingle(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public TmEssentials.TimeSingle? TimeSingleNullable(TmEssentials.TimeSingle? value = default)
    {
        if (Reader is not null) value = Reader.ReadTimeSingleNullable();
        Writer?.WriteTimeSingleNullable(value);
        return value;
    }

    public void TimeSingleNullable([NotNullIfNotNull(nameof(value))] ref TmEssentials.TimeSingle? value) => value = TimeSingleNullable(value);

    [return: NotNullIfNotNull(nameof(value))]
    public System.TimeSpan? TimeOfDay(System.TimeSpan? value = default)
    {
        if (Reader is not null) value = Reader.ReadTimeOfDay();
        Writer?.WriteTimeOfDay(value);
        return value;
    }

    public void TimeOfDay([NotNullIfNotNull(nameof(value))] ref System.TimeSpan? value) => value = TimeOfDay(value);

    [return: NotNullIfNotNull(nameof(value))]
    public System.DateTime? FileTime(System.DateTime? value = default)
    {
        if (Reader is not null) value = Reader.ReadFileTime();
        Writer?.WriteFileTime(value);
        return value;
    }

    public void FileTime([NotNullIfNotNull(nameof(value))] ref System.DateTime? value) => value = FileTime(value);

    [return: NotNullIfNotNull(nameof(value))]
    public System.DateTime? SystemTime(System.DateTime? value = default)
    {
        if (Reader is not null) value = Reader.ReadSystemTime();
        Writer?.WriteSystemTime(value);
        return value;
    }

    public void SystemTime([NotNullIfNotNull(nameof(value))] ref System.DateTime? value) => value = SystemTime(value);

    [return: NotNullIfNotNull(nameof(value))]
    public System.DateTimeOffset? UnixTime(System.DateTimeOffset? value = default)
    {
        if (Reader is not null) value = Reader.ReadUnixTime();
        Writer?.WriteUnixTime(value);
        return value;
    }

    public void UnixTime([NotNullIfNotNull(nameof(value))] ref System.DateTimeOffset? value) => value = UnixTime(value);

    [return: NotNullIfNotNull(nameof(value))]
    public System.Net.IPAddress? IPv4(System.Net.IPAddress? value = default)
    {
        if (Reader is not null) value = Reader.ReadIPv4();
        Writer?.WriteIPv4(value);
        return value;
    }

    public void IPv4([NotNullIfNotNull(nameof(value))] ref System.Net.IPAddress? value) => value = IPv4(value);

    [return: NotNullIfNotNull(nameof(value))]
    public int SmallLen(int value = default)
    {
        if (Reader is not null) value = Reader.ReadSmallLen();
        Writer?.WriteSmallLen(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public int? SmallLen(int? value, int defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadSmallLen();
        Writer?.WriteSmallLen(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void SmallLen([NotNullIfNotNull(nameof(value))] ref int value) => value = SmallLen(value);

    public void SmallLen([NotNullIfNotNull(nameof(value))] ref int? value, int defaultValue = default) => value = SmallLen(value, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public string? SmallString(string? value = default)
    {
        if (Reader is not null) value = Reader.ReadSmallString();
        Writer?.WriteSmallString(value);
        return value;
    }

    public void SmallString([NotNullIfNotNull(nameof(value))] ref string? value) => value = SmallString(value);

    [return: NotNullIfNotNull(nameof(value))]
    public int OptimizedInt(int value, int determineFrom)
    {
        if (Reader is not null) value = Reader.ReadOptimizedInt(determineFrom);
        Writer?.WriteOptimizedInt(value, determineFrom);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public int? OptimizedInt(int? value, int determineFrom, int defaultValue)
    {
        if (Reader is not null) value = Reader.ReadOptimizedInt(determineFrom);
        Writer?.WriteOptimizedInt(value.GetValueOrDefault(defaultValue), determineFrom);
        return value;
    }

    public void OptimizedInt([NotNullIfNotNull(nameof(value))] ref int value, int determineFrom) => value = OptimizedInt(value, determineFrom);

    public void OptimizedInt([NotNullIfNotNull(nameof(value))] ref int? value, int determineFrom, int defaultValue = default) => value = OptimizedInt(value, determineFrom, defaultValue);

    [return: NotNullIfNotNull(nameof(value))]
    public short VarNat15(short value = default)
    {
        if (Reader is not null) value = Reader.ReadVarNat15();
        Writer?.WriteVarNat15(value);
        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public short? VarNat15(short? value, short defaultValue = default)
    {
        if (Reader is not null) value = Reader.ReadVarNat15();
        Writer?.WriteVarNat15(value.GetValueOrDefault(defaultValue));
        return value;
    }

    public void VarNat15([NotNullIfNotNull(nameof(value))] ref short value) => value = VarNat15(value);

    public void VarNat15([NotNullIfNotNull(nameof(value))] ref short? value, short defaultValue = default) => value = VarNat15(value, defaultValue);

    public void DeprecVersion()
    {
        Reader?.ReadDeprecVersion();
        Writer?.WriteDeprecVersion();
    }

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Vec3[]? ArrayVec3_10b(GBX.NET.Vec3[]? value = default)
    {
        if (Reader is not null) value = Reader.ReadArrayVec3_10b();
        Writer?.WriteArrayVec3_10b(value);
        return value;
    }

    public void ArrayVec3_10b([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3[]? value) => value = ArrayVec3_10b(value);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Vec3[]? ArrayVec3_10b(GBX.NET.Vec3[]? value, int length)
    {
        if (Reader is not null) value = Reader.ReadArrayVec3_10b(length);
        Writer?.WriteArrayVec3_10b(value, length);
        return value;
    }

    public void ArrayVec3_10b([NotNullIfNotNull(nameof(value))] ref GBX.NET.Vec3[]? value, int length) => value = ArrayVec3_10b(value, length);

    [return: NotNullIfNotNull(nameof(value))]
    public T[]? Array<T>(T[]? value, int length, bool lengthInBytes = false) where T : struct
    {
        if (Reader is not null) value = Reader.ReadArray<T>(length, lengthInBytes);
        Writer?.WriteArray(value, length, lengthInBytes);
        return value;
    }

    public void Array<T>([NotNullIfNotNull(nameof(value))] ref T[]? value, int length, bool lengthInBytes = false) where T : struct => value = Array(value, length, lengthInBytes);

    [return: NotNullIfNotNull(nameof(value))]
    public T[]? Array<T>(T[]? value, bool lengthInBytes = false) where T : struct
    {
        if (Reader is not null) value = Reader.ReadArray<T>(lengthInBytes);
        Writer?.WriteArray(value, lengthInBytes);
        return value;
    }

    public void Array<T>([NotNullIfNotNull(nameof(value))] ref T[]? value, bool lengthInBytes = false) where T : struct => value = Array(value, lengthInBytes);

    [return: NotNullIfNotNull(nameof(value))]
    public T[]? Array_deprec<T>(T[]? value, int length, bool lengthInBytes = false) where T : struct
    {
        if (Reader is not null) value = Reader.ReadArray_deprec<T>(length, lengthInBytes);
        Writer?.WriteArray_deprec(value, length, lengthInBytes);
        return value;
    }

    public void Array_deprec<T>([NotNullIfNotNull(nameof(value))] ref T[]? value, int length, bool lengthInBytes = false) where T : struct => value = Array_deprec(value, length, lengthInBytes);

    [return: NotNullIfNotNull(nameof(value))]
    public T[]? Array_deprec<T>(T[]? value, bool lengthInBytes = false) where T : struct
    {
        if (Reader is not null) value = Reader.ReadArray_deprec<T>(lengthInBytes);
        Writer?.WriteArray_deprec(value, lengthInBytes);
        return value;
    }

    public void Array_deprec<T>([NotNullIfNotNull(nameof(value))] ref T[]? value, bool lengthInBytes = false) where T : struct => value = Array_deprec(value, lengthInBytes);

    [return: NotNullIfNotNull(nameof(value))]
    public T[][]? JaggedArray<T>(T[][]? value, int? innerLength = default, int? outerLength = default) where T : struct
    {
        if (Reader is not null) value = Reader.ReadJaggedArray<T>(innerLength, outerLength);
        Writer?.WriteJaggedArray(value, innerLength, outerLength);
        return value;
    }

    public void JaggedArray<T>([NotNullIfNotNull(nameof(value))] ref T[][]? value, int? innerLength = default, int? outerLength = default) where T : struct => value = JaggedArray(value, innerLength, outerLength);

    [return: NotNullIfNotNull(nameof(value))]
    public System.Collections.Generic.List<T>? List<T>(System.Collections.Generic.List<T>? value, int length, bool lengthInBytes = false) where T : struct
    {
        if (Reader is not null) value = Reader.ReadList<T>(length, lengthInBytes);
        Writer?.WriteList(value, length, lengthInBytes);
        return value;
    }

    public void List<T>([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<T>? value, int length, bool lengthInBytes = false) where T : struct => value = List(value, length, lengthInBytes);

    [return: NotNullIfNotNull(nameof(value))]
    public System.Collections.Generic.List<T>? List<T>(System.Collections.Generic.List<T>? value, bool lengthInBytes = false) where T : struct
    {
        if (Reader is not null) value = Reader.ReadList<T>(lengthInBytes);
        Writer?.WriteList(value, lengthInBytes);
        return value;
    }

    public void List<T>([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<T>? value, bool lengthInBytes = false) where T : struct => value = List(value, lengthInBytes);

    [return: NotNullIfNotNull(nameof(value))]
    public System.Collections.Generic.List<T>? List_deprec<T>(System.Collections.Generic.List<T>? value, bool lengthInBytes = false) where T : struct
    {
        if (Reader is not null) value = Reader.ReadList_deprec<T>(lengthInBytes);
        Writer?.WriteList_deprec(value, lengthInBytes);
        return value;
    }

    public void List_deprec<T>([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<T>? value, bool lengthInBytes = false) where T : struct => value = List_deprec(value, lengthInBytes);

    [return: NotNullIfNotNull(nameof(value))]
    public T?[]? ArrayNode<T>(T?[]? value = default) where T : GBX.NET.IClass, new()
    {
        if (Reader is not null) value = Reader.ReadArrayNode<T>();
        Writer?.WriteArrayNode(value);
        return value;
    }

    public void ArrayNode<T>([NotNullIfNotNull(nameof(value))] ref T?[]? value) where T : GBX.NET.IClass, new() => value = ArrayNode(value);

    [return: NotNullIfNotNull(nameof(value))]
    public T?[]? ArrayNodeRef<T>(T?[]? value, int length) where T : GBX.NET.IClass
    {
        if (Reader is not null) value = Reader.ReadArrayNodeRef<T>(length);
        Writer?.WriteArrayNodeRef(value, length);
        return value;
    }

    public void ArrayNodeRef<T>([NotNullIfNotNull(nameof(value))] ref T?[]? value, int length) where T : GBX.NET.IClass => value = ArrayNodeRef(value, length);

    [return: NotNullIfNotNull(nameof(value))]
    public T?[]? ArrayNodeRef<T>(T?[]? value = default) where T : GBX.NET.IClass
    {
        if (Reader is not null) value = Reader.ReadArrayNodeRef<T>();
        Writer?.WriteArrayNodeRef(value);
        return value;
    }

    public void ArrayNodeRef<T>([NotNullIfNotNull(nameof(value))] ref T?[]? value) where T : GBX.NET.IClass => value = ArrayNodeRef(value);

    [return: NotNullIfNotNull(nameof(value))]
    public T?[]? ArrayNodeRef_deprec<T>(T?[]? value = default) where T : GBX.NET.IClass
    {
        if (Reader is not null) value = Reader.ReadArrayNodeRef_deprec<T>();
        Writer?.WriteArrayNodeRef_deprec(value);
        return value;
    }

    public void ArrayNodeRef_deprec<T>([NotNullIfNotNull(nameof(value))] ref T?[]? value) where T : GBX.NET.IClass => value = ArrayNodeRef_deprec(value);

    [return: NotNullIfNotNull(nameof(value))]
    public T?[][]? JaggedArrayNodeRef<T>(T?[][]? value, int? innerLength = default, int? outerLength = default) where T : GBX.NET.IClass
    {
        if (Reader is not null) value = Reader.ReadJaggedArrayNodeRef<T>(innerLength, outerLength);
        Writer?.WriteJaggedArrayNodeRef(value, innerLength, outerLength);
        return value;
    }

    public void JaggedArrayNodeRef<T>([NotNullIfNotNull(nameof(value))] ref T?[][]? value, int? innerLength = default, int? outerLength = default) where T : GBX.NET.IClass => value = JaggedArrayNodeRef(value, innerLength, outerLength);

    [return: NotNullIfNotNull(nameof(value))]
    public System.Collections.Generic.List<T?>? ListNodeRef<T>(System.Collections.Generic.List<T?>? value, int length) where T : GBX.NET.IClass
    {
        if (Reader is not null) value = Reader.ReadListNodeRef<T>(length);
        Writer?.WriteListNodeRef(value, length);
        return value;
    }

    public void ListNodeRef<T>([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<T?>? value, int length) where T : GBX.NET.IClass => value = ListNodeRef(value, length);

    [return: NotNullIfNotNull(nameof(value))]
    public System.Collections.Generic.List<T?>? ListNodeRef<T>(System.Collections.Generic.List<T?>? value = default) where T : GBX.NET.IClass
    {
        if (Reader is not null) value = Reader.ReadListNodeRef<T>();
        Writer?.WriteListNodeRef(value);
        return value;
    }

    public void ListNodeRef<T>([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<T?>? value) where T : GBX.NET.IClass => value = ListNodeRef(value);

    [return: NotNullIfNotNull(nameof(value))]
    public System.Collections.Generic.List<T?>? ListNodeRef_deprec<T>(System.Collections.Generic.List<T?>? value = default) where T : GBX.NET.IClass
    {
        if (Reader is not null) value = Reader.ReadListNodeRef_deprec<T>();
        Writer?.WriteListNodeRef_deprec(value);
        return value;
    }

    public void ListNodeRef_deprec<T>([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<T?>? value) where T : GBX.NET.IClass => value = ListNodeRef_deprec(value);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.External<T>[]? ArrayExternalNodeRef<T>(GBX.NET.External<T>[]? value, int length) where T : GBX.NET.Engines.MwFoundations.CMwNod
    {
        if (Reader is not null) value = Reader.ReadArrayExternalNodeRef<T>(length);
        Writer?.WriteArrayExternalNodeRef(value, length);
        return value;
    }

    public void ArrayExternalNodeRef<T>([NotNullIfNotNull(nameof(value))] ref GBX.NET.External<T>[]? value, int length) where T : GBX.NET.Engines.MwFoundations.CMwNod => value = ArrayExternalNodeRef(value, length);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.External<T>[]? ArrayExternalNodeRef<T>(GBX.NET.External<T>[]? value = default) where T : GBX.NET.Engines.MwFoundations.CMwNod
    {
        if (Reader is not null) value = Reader.ReadArrayExternalNodeRef<T>();
        Writer?.WriteArrayExternalNodeRef(value);
        return value;
    }

    public void ArrayExternalNodeRef<T>([NotNullIfNotNull(nameof(value))] ref GBX.NET.External<T>[]? value) where T : GBX.NET.Engines.MwFoundations.CMwNod => value = ArrayExternalNodeRef(value);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.External<T>[]? ArrayExternalNodeRef_deprec<T>(GBX.NET.External<T>[]? value = default) where T : GBX.NET.Engines.MwFoundations.CMwNod
    {
        if (Reader is not null) value = Reader.ReadArrayExternalNodeRef_deprec<T>();
        Writer?.WriteArrayExternalNodeRef_deprec(value);
        return value;
    }

    public void ArrayExternalNodeRef_deprec<T>([NotNullIfNotNull(nameof(value))] ref GBX.NET.External<T>[]? value) where T : GBX.NET.Engines.MwFoundations.CMwNod => value = ArrayExternalNodeRef_deprec(value);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.External<T>[][]? JaggedArrayExternalNodeRef<T>(GBX.NET.External<T>[][]? value, int? innerLength = default, int? outerLength = default) where T : GBX.NET.Engines.MwFoundations.CMwNod
    {
        if (Reader is not null) value = Reader.ReadJaggedArrayExternalNodeRef<T>(innerLength, outerLength);
        Writer?.WriteJaggedArrayExternalNodeRef(value, innerLength, outerLength);
        return value;
    }

    public void JaggedArrayExternalNodeRef<T>([NotNullIfNotNull(nameof(value))] ref GBX.NET.External<T>[][]? value, int? innerLength = default, int? outerLength = default) where T : GBX.NET.Engines.MwFoundations.CMwNod => value = JaggedArrayExternalNodeRef(value, innerLength, outerLength);

    [return: NotNullIfNotNull(nameof(value))]
    public System.Collections.Generic.List<GBX.NET.External<T>>? ListExternalNodeRef<T>(System.Collections.Generic.List<GBX.NET.External<T>>? value, int length) where T : GBX.NET.Engines.MwFoundations.CMwNod
    {
        if (Reader is not null) value = Reader.ReadListExternalNodeRef<T>(length);
        Writer?.WriteListExternalNodeRef(value, length);
        return value;
    }

    public void ListExternalNodeRef<T>([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<GBX.NET.External<T>>? value, int length) where T : GBX.NET.Engines.MwFoundations.CMwNod => value = ListExternalNodeRef(value, length);

    [return: NotNullIfNotNull(nameof(value))]
    public System.Collections.Generic.List<GBX.NET.External<T>>? ListExternalNodeRef<T>(System.Collections.Generic.List<GBX.NET.External<T>>? value = default) where T : GBX.NET.Engines.MwFoundations.CMwNod
    {
        if (Reader is not null) value = Reader.ReadListExternalNodeRef<T>();
        Writer?.WriteListExternalNodeRef(value);
        return value;
    }

    public void ListExternalNodeRef<T>([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<GBX.NET.External<T>>? value) where T : GBX.NET.Engines.MwFoundations.CMwNod => value = ListExternalNodeRef(value);

    [return: NotNullIfNotNull(nameof(value))]
    public System.Collections.Generic.List<GBX.NET.External<T>>? ListExternalNodeRef_deprec<T>(System.Collections.Generic.List<GBX.NET.External<T>>? value = default) where T : GBX.NET.Engines.MwFoundations.CMwNod
    {
        if (Reader is not null) value = Reader.ReadListExternalNodeRef_deprec<T>();
        Writer?.WriteListExternalNodeRef_deprec(value);
        return value;
    }

    public void ListExternalNodeRef_deprec<T>([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<GBX.NET.External<T>>? value) where T : GBX.NET.Engines.MwFoundations.CMwNod => value = ListExternalNodeRef_deprec(value);

    [return: NotNullIfNotNull(nameof(value))]
    public string[]? ArrayId(string[]? value, int length)
    {
        if (Reader is not null) value = Reader.ReadArrayId(length);
        Writer?.WriteArrayId(value, length);
        return value;
    }

    public void ArrayId([NotNullIfNotNull(nameof(value))] ref string[]? value, int length) => value = ArrayId(value, length);

    [return: NotNullIfNotNull(nameof(value))]
    public string[]? ArrayId(string[]? value = default)
    {
        if (Reader is not null) value = Reader.ReadArrayId();
        Writer?.WriteArrayId(value);
        return value;
    }

    public void ArrayId([NotNullIfNotNull(nameof(value))] ref string[]? value) => value = ArrayId(value);

    [return: NotNullIfNotNull(nameof(value))]
    public string[]? ArrayId_deprec(string[]? value = default)
    {
        if (Reader is not null) value = Reader.ReadArrayId_deprec();
        Writer?.WriteArrayId_deprec(value);
        return value;
    }

    public void ArrayId_deprec([NotNullIfNotNull(nameof(value))] ref string[]? value) => value = ArrayId_deprec(value);

    [return: NotNullIfNotNull(nameof(value))]
    public string[][]? JaggedArrayId(string[][]? value, int? innerLength = default, int? outerLength = default)
    {
        if (Reader is not null) value = Reader.ReadJaggedArrayId(innerLength, outerLength);
        Writer?.WriteJaggedArrayId(value, innerLength, outerLength);
        return value;
    }

    public void JaggedArrayId([NotNullIfNotNull(nameof(value))] ref string[][]? value, int? innerLength = default, int? outerLength = default) => value = JaggedArrayId(value, innerLength, outerLength);

    [return: NotNullIfNotNull(nameof(value))]
    public string[][]? JaggedArrayString(string[][]? value, int? innerLength = default, int? outerLength = default)
    {
        if (Reader is not null) value = Reader.ReadJaggedArrayString(innerLength, outerLength);
        Writer?.WriteJaggedArrayString(value, innerLength, outerLength);
        return value;
    }

    public void JaggedArrayString([NotNullIfNotNull(nameof(value))] ref string[][]? value, int? innerLength = default, int? outerLength = default) => value = JaggedArrayString(value, innerLength, outerLength);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.Ident[][]? JaggedArrayIdent(GBX.NET.Ident[][]? value, int? innerLength = default, int? outerLength = default)
    {
        if (Reader is not null) value = Reader.ReadJaggedArrayIdent(innerLength, outerLength);
        Writer?.WriteJaggedArrayIdent(value, innerLength, outerLength);
        return value;
    }

    public void JaggedArrayIdent([NotNullIfNotNull(nameof(value))] ref GBX.NET.Ident[][]? value, int? innerLength = default, int? outerLength = default) => value = JaggedArrayIdent(value, innerLength, outerLength);

    [return: NotNullIfNotNull(nameof(value))]
    public GBX.NET.PackDesc[][]? JaggedArrayPackDesc(GBX.NET.PackDesc[][]? value, int? innerLength = default, int? outerLength = default)
    {
        if (Reader is not null) value = Reader.ReadJaggedArrayPackDesc(innerLength, outerLength);
        Writer?.WriteJaggedArrayPackDesc(value, innerLength, outerLength);
        return value;
    }

    public void JaggedArrayPackDesc([NotNullIfNotNull(nameof(value))] ref GBX.NET.PackDesc[][]? value, int? innerLength = default, int? outerLength = default) => value = JaggedArrayPackDesc(value, innerLength, outerLength);

    [return: NotNullIfNotNull(nameof(value))]
    public System.Collections.Generic.List<string>? ListId(System.Collections.Generic.List<string>? value, int length)
    {
        if (Reader is not null) value = Reader.ReadListId(length);
        Writer?.WriteListId(value, length);
        return value;
    }

    public void ListId([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<string>? value, int length) => value = ListId(value, length);

    [return: NotNullIfNotNull(nameof(value))]
    public System.Collections.Generic.List<string>? ListId(System.Collections.Generic.List<string>? value = default)
    {
        if (Reader is not null) value = Reader.ReadListId();
        Writer?.WriteListId(value);
        return value;
    }

    public void ListId([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<string>? value) => value = ListId(value);

    [return: NotNullIfNotNull(nameof(value))]
    public System.Collections.Generic.List<string>? ListId_deprec(System.Collections.Generic.List<string>? value = default)
    {
        if (Reader is not null) value = Reader.ReadListId_deprec();
        Writer?.WriteListId_deprec(value);
        return value;
    }

    public void ListId_deprec([NotNullIfNotNull(nameof(value))] ref System.Collections.Generic.List<string>? value) => value = ListId_deprec(value);

}
