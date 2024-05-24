using GbxExplorerOld.Client.Models;
using System.Reflection;

namespace GbxExplorerOld.Client.Services;

public interface ITypeCacheService
{
    (TypeInfoModel, bool isNullable) GetTypeInfoModel(Type type, PropertyInfo? property);
}