namespace NotesWebApi.Services;
/// <summary>
///  Интерфейс сервиса хеширования и верификации паролей
/// </summary>
public interface IPasswordService
{
    /// <summary>
    ///  Создание хеша пароля, склеенного с солью
    /// </summary>
    public string CreatePasswordHash(string password);
    /// <summary>
    ///  Верификация пароля с извлечением соли
    /// </summary>
    /// <param name="hash">Хэшированный пароль с солью из базы данных</param>
    /// <param name="password">Пароль, предназначенный для верификации</param>
    public bool VerifyPasswordHash(string hash, string password);
}
