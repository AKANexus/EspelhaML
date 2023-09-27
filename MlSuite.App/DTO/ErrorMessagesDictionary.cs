namespace MlSuite.App.DTO
{
    public static class ErrorMessagesStrings
    {
        public const string ValidEmailAddress = "{PropertyName} te que ser um endereço de email válido";
        public const string NotNullMessage = "{PropertyName} não pode estar em branco";
        public const string MaxLengthMessage = "{PropertyName} só pode ter até {MaxLength} caracteres";
        public const string PositiveValueMessage = "{PropertyName} deve ser um valor positivo";
        public const string PasswordsMustMatch = "A senha e a confirmação não são iguais";
        public const string MustBeGuid = "{PropertyName} deve estar no formato de GUID";
    }
}
