namespace DIOFuncValidadorCPF.DTO;

public sealed class ResponseHttp
{
    #region Propriedades

    public bool IsValid { get; set; } = false;

    public string Message { get; set; } = string.Empty;

    #endregion
}