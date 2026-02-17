namespace DIOFuncValidadorCPF.DTO;

public sealed class ResponseHttp
{
    #region Propriedades

    public bool IsValid { get; set; }

    public string Message { get; set; } = string.Empty;

    #endregion
}