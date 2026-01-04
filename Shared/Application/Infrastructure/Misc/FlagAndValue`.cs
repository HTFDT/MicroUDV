namespace Shared.Application.Infrastructure.Misc;

public class FlagAndValue<T>(bool flag, T? value = default) : Tuple<bool, T?>(flag, value);