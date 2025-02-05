namespace Models
{
    public class CryptoModel
    {
        public string EncryptPurpose { get; set; }
        public string DecryptPurpose { get; set; }
        public string DataToEncrypt { get; set; }
        public string EncryptedData { get; set; }
        public string DataToDecrypt { get; set; }
        public string DecryptedData { get; set; }
        public string Exception { get; set; }
    }
}