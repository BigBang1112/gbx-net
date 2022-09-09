using GbxExplorer.Client.Models;
using System.Reflection;

namespace GbxExplorer.Client.Services;

public interface ITypeCacheService
{
    (TypeInfoModel, bool isNullable) GetTypeInfoModel(Type type, PropertyInfo? property);
}